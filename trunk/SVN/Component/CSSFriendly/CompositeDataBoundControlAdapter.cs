using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
	public abstract class CompositeDataBoundControlAdapter : System.Web.UI.WebControls.Adapters.DataBoundControlAdapter
	{
		private WebControlAdapterExtender _extender = null;
		private WebControlAdapterExtender Extender
		{
			get
			{
				if (((_extender == null) && (Control != null)) ||
					((_extender != null) && (Control != _extender.AdaptedControl)))
				{
					_extender = new WebControlAdapterExtender(Control);
				}

				System.Diagnostics.Debug.Assert(_extender != null, "CSS Friendly adapters internal error", "Null extender instance");
				return _extender;
			}
		}

		protected string _classMain = "";
		protected string _classHeader = "";
		protected string _classData = "";
		protected string _classFooter = "";
		protected string _classPagination = "";
		protected string _classOtherPage = "";
		protected string _classActivePage = "";
		protected string _classEmptyData = "";

		protected string _classFirstPrevPagination = "";
		protected string _classFirstPage = "";
		protected string _classPreviousPage = "";
		protected string _classNextLastPagination = "";
		protected string _classNextPage = "";
		protected string _classLastPage = "";
		protected string _classPagingLinkDisabled = "";

		protected CompositeDataBoundControl View
		{
			get { return Control as CompositeDataBoundControl; }
		}

		protected DetailsView ControlAsDetailsView
		{
			get { return Control as DetailsView; }
		}

		protected bool IsDetailsView
		{
			get { return ControlAsDetailsView != null; }
		}

		protected FormView ControlAsFormView
		{
			get { return Control as FormView; }
		}

		protected bool IsFormView
		{
			get { return ControlAsFormView != null; }
		}

		protected abstract string HeaderText { get; }
		protected abstract string FooterText { get; }
		protected abstract ITemplate HeaderTemplate { get; }
		protected abstract ITemplate FooterTemplate { get; }
		protected abstract ITemplate PagerTemplate { get; }
		protected abstract TableRow HeaderRow { get; }
		protected abstract TableRow FooterRow { get; }
		protected abstract TableRow TopPagerRow { get; }
		protected abstract TableRow BottomPagerRow { get; }
		protected abstract bool AllowPaging { get; }
		protected abstract int DataItemCount { get; }
		protected abstract int DataItemIndex { get; }
		protected abstract int PageIndex { get; }
		protected abstract PagerSettings PagerSettings { get; }

		/// ///////////////////////////////////////////////////////////////////////////////
		/// METHODS

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (Extender.AdapterEnabled)
			{
				RegisterScripts();
			}
		}

		protected override void RenderBeginTag(HtmlTextWriter writer)
		{
			if (Extender.AdapterEnabled)
			{
				Extender.RenderBeginTag(writer, _classMain);
			}
			else
			{
				base.RenderBeginTag(writer);
			}
		}

		protected override void RenderEndTag(HtmlTextWriter writer)
		{
			if (Extender.AdapterEnabled)
			{
				Extender.RenderEndTag(writer);
			}
			else
			{
				base.RenderEndTag(writer);
			}
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			if (Extender.AdapterEnabled)
			{
				if (View != null)
				{
					writer.Indent++;

					BuildPaging(writer, PagerPosition.Top);
					BuildRow(HeaderRow, _classHeader, writer);
					BuildItem(writer);
					BuildRow(FooterRow, _classFooter, writer);
					BuildPaging(writer, PagerPosition.Bottom);

					writer.Indent--;
					writer.WriteLine();
				}
			}
			else
			{
				base.RenderContents(writer);
			}
		}

		protected virtual void BuildItem(HtmlTextWriter writer)
		{
		}

		protected virtual void BuildRow(TableRow row, string cssClass, HtmlTextWriter writer)
		{
			if ((row != null) && row.Visible)
			{
				// If there isn't any content, don't render anything.

				bool bHasContent = false;
				TableCell cell = null;
				for (int iCell = 0; iCell < row.Cells.Count; iCell++)
				{
					cell = row.Cells[iCell];
					if ((!String.IsNullOrEmpty(cell.Text)) || (cell.Controls.Count > 0))
					{
						bHasContent = true;
						break;
					}
				}

				if (bHasContent)
				{
					writer.WriteLine();
					writer.WriteBeginTag("div");
					writer.WriteAttribute("class", cssClass);
					writer.Write(HtmlTextWriter.TagRightChar);
					writer.Indent++;
					writer.WriteLine();

					for (int iCell = 0; iCell < row.Cells.Count; iCell++)
					{
						cell = row.Cells[iCell];
						if (!String.IsNullOrEmpty(cell.Text))
						{
							writer.Write(cell.Text);
						}
						foreach (Control cellChildControl in cell.Controls)
						{
							cellChildControl.RenderControl(writer);
						}
					}

					writer.Indent--;
					writer.WriteLine();
					writer.WriteEndTag("div");
				}
			}
		}

		protected virtual void WritePagingLink(HtmlTextWriter writer, string linkText, params string[] cssClass)
		{
			WritePagingLink(writer, linkText, -1, cssClass);
		}

		protected virtual void WritePagingLink(HtmlTextWriter writer, string linkText, int pageNumber, params string[] cssClass)
		{
			#region Combine cssClass params
			string classes = String.Empty;
			if (cssClass.Length > 0)
			{
				foreach (string cl in cssClass)
				{
					classes += String.Format(" {0} ", cl);
				}
			}
			classes = classes.Trim();
			#endregion

			if (pageNumber > -1)
			{
				writer.WriteBeginTag("a");
				writer.WriteAttribute("class", classes);
				writer.WriteAttribute("href", Page.ClientScript.GetPostBackClientHyperlink(Control, "Page$" + pageNumber.ToString(), true));
				writer.Write(HtmlTextWriter.TagRightChar);
				writer.Write(linkText);
				writer.WriteEndTag("a");
			}
			else
			{
				writer.WriteBeginTag("span");
				writer.WriteAttribute("class", classes);
				writer.Write(HtmlTextWriter.TagRightChar);
				writer.Write(linkText);
				writer.WriteEndTag("span");
			}
		}

		protected virtual void BuildPaging(HtmlTextWriter writer, PagerPosition position)
		{
			if (AllowPaging && (DataItemCount > 0) && PagerSettings.Visible &&
				(PagerSettings.Position == position || PagerSettings.Position == PagerPosition.TopAndBottom))
			{
				if (PagerTemplate == null)
				{
					writer.WriteLine();
					writer.WriteBeginTag("div");
					writer.WriteAttribute("class", _classPagination);
					writer.Write(HtmlTextWriter.TagRightChar);
					writer.Indent++;

					int start = 0;
					int end = this.DataItemCount;
					int pages = end;

					//  Check if we need to display the first and last links
					bool bIncludeFirstLast = (this.PagerSettings.Mode == PagerButtons.NextPreviousFirstLast)
						|| (this.PagerSettings.Mode == PagerButtons.NumericFirstLast);

					//  Identify which mode of paging we are using
					switch (this.PagerSettings.Mode)
					{
						case PagerButtons.NumericFirstLast:
						case PagerButtons.Numeric:
							// Update start/end/pages
							bool bExceededPageButtonCount = pages > this.PagerSettings.PageButtonCount;
							if (bExceededPageButtonCount)
							{
								start = (PageIndex / PagerSettings.PageButtonCount) * PagerSettings.PageButtonCount;
								end = Math.Min(start + PagerSettings.PageButtonCount, DataItemCount);
							}

							// Write first page link
							if (bIncludeFirstLast)
							{
								if (this.PageIndex > 0)
								{
									WritePagingLink(writer, this.PagerSettings.FirstPageText, 1, _classFirstPage);
								}
								else
								{
									WritePagingLink(writer, this.PagerSettings.FirstPageText, _classFirstPage, _classPagingLinkDisabled);
								}
							}

							// Write first other pages link
							if (bExceededPageButtonCount && (start > 0))
							{
								WritePagingLink(writer, "...", start, _classOtherPage);
							}

							// Write page links
							for (int iDataItem = start; iDataItem < end; iDataItem++)
							{
								string strPage = (iDataItem + 1).ToString();
								if (PageIndex == iDataItem)
								{
									WritePagingLink(writer, strPage, _classOtherPage, _classActivePage);
								}
								else
								{
									WritePagingLink(writer, strPage, iDataItem + 1, _classOtherPage);
								}
							}

							// Write last other pages link
							if (bExceededPageButtonCount && (end < DataItemCount))
							{
								WritePagingLink(writer, "...", end + 1, _classOtherPage);
							}

							// Write last page link
							if (bIncludeFirstLast)
							{
								if (this.PageIndex < (this.DataItemCount - 1))
								{
									WritePagingLink(writer, this.PagerSettings.LastPageText, this.DataItemCount, _classLastPage);
								}
								else
								{
									WritePagingLink(writer, this.PagerSettings.LastPageText, _classLastPage, _classPagingLinkDisabled);
								}

							}
							break;

						case PagerButtons.NextPreviousFirstLast:
						case PagerButtons.NextPrevious:
							// Write first page link
							// Only implemented when displaying paging links as text instead of an image
							if (bIncludeFirstLast)
							{
								if (String.IsNullOrEmpty(this.PagerSettings.FirstPageImageUrl))
								{
									if (this.PageIndex > start)
									{
										WritePagingLink(writer, this.PagerSettings.FirstPageText, start + 1, _classFirstPage);
									}
									else
									{
										WritePagingLink(writer, this.PagerSettings.FirstPageText, _classFirstPage, _classActivePage);
									}
								}
							}

							// Write prev page link
							// Only implemented when displaying paging links as text instead of an image
							if (String.IsNullOrEmpty(this.PagerSettings.PreviousPageImageUrl))
							{
								// Check if Previous link needs to be disabled
								if (this.PageIndex > start)
								{
									WritePagingLink(writer, this.PagerSettings.PreviousPageText, this.PageIndex, _classPreviousPage);
								}
								else
								{
									WritePagingLink(writer, this.PagerSettings.PreviousPageText, _classPreviousPage, _classPagingLinkDisabled);
								}
							}

							// Write next page link
							// Only implemented when displaying paging links as text instead of an image
							if (String.IsNullOrEmpty(this.PagerSettings.NextPageImageUrl))
							{
								// Check if Next link needs to be disabled
								if (PageIndex < (end - 1))
								{
									WritePagingLink(writer, this.PagerSettings.NextPageText, this.PageIndex + 2, _classNextPage);
								}
								else
								{
									WritePagingLink(writer, this.PagerSettings.NextPageText, _classNextPage, _classPagingLinkDisabled);
								}
							}

							// Write last page link
							// Only implemented when displaying paging links as text instead of an image
							if (bIncludeFirstLast)
							{
								if (String.IsNullOrEmpty(this.PagerSettings.LastPageImageUrl))
								{
									if (this.PageIndex < (end - 1))
									{
										WritePagingLink(writer, this.PagerSettings.LastPageText, pages, _classLastPage);
									}
									else
									{
										WritePagingLink(writer, this.PagerSettings.LastPageText, _classLastPage, _classActivePage);
									}
								}
							}
							break;
					}

					writer.Indent--;
					writer.WriteLine();
					writer.WriteEndTag("div");
				}
				else // PagerTemplate != null
				{
					writer.WriteLine();
					writer.WriteBeginTag("div");
					writer.WriteAttribute("class", _classPagination);
					writer.Write(HtmlTextWriter.TagRightChar);
					writer.Indent++;

					TableRow pagerRow;

					if (position == PagerPosition.Top)
					{
						pagerRow = TopPagerRow;
					}
					else
					{
						pagerRow = BottomPagerRow;
					}

					foreach (TableCell cell in pagerRow.Cells)
					{
						foreach (Control ctrl in cell.Controls)
						{
							ctrl.RenderControl(writer);
						}
					}

					writer.Indent--;
					writer.WriteEndTag("div");
				}
			}
		}

		protected virtual void RegisterScripts()
		{
		}
	}
}
