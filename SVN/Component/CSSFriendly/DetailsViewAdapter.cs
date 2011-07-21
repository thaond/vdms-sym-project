using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
	public class DetailsViewAdapter : CompositeDataBoundControlAdapter
	{
		protected override string HeaderText { get { return ControlAsDetailsView.HeaderText; } }
		protected override string FooterText { get { return ControlAsDetailsView.FooterText; } }
		protected override ITemplate HeaderTemplate { get { return ControlAsDetailsView.HeaderTemplate; } }
		protected override ITemplate FooterTemplate { get { return ControlAsDetailsView.FooterTemplate; } }
		protected override ITemplate PagerTemplate { get { return ControlAsDetailsView.PagerTemplate; } }
		protected override TableRow HeaderRow { get { return ControlAsDetailsView.HeaderRow; } }
		protected override TableRow FooterRow { get { return ControlAsDetailsView.FooterRow; } }
		protected override TableRow TopPagerRow { get { return ControlAsDetailsView.TopPagerRow; } }
		protected override TableRow BottomPagerRow { get { return ControlAsDetailsView.BottomPagerRow; } }
		protected override bool AllowPaging { get { return ControlAsDetailsView.AllowPaging; } }
		protected override int DataItemCount { get { return ControlAsDetailsView.DataItemCount; } }
		protected override int DataItemIndex { get { return ControlAsDetailsView.DataItemIndex; } }
		protected override int PageIndex { get { return ControlAsDetailsView.PageIndex; } }
		protected override PagerSettings PagerSettings { get { return ControlAsDetailsView.PagerSettings; } }

		public DetailsViewAdapter()
		{
			_classMain = "AspNet-DetailsView";
			_classHeader = "AspNet-DetailsView-Header";
			_classData = "AspNet-DetailsView-Data";
			_classFooter = "AspNet-DetailsView-Footer";
			_classPagination = "AspNet-DetailsView-Pagination";
			_classOtherPage = "AspNet-DetailsView-OtherPage";
			_classActivePage = "AspNet-DetailsView-ActivePage";
			_classEmptyData = "AspNet-DetailsView-EmptyData";

			_classFirstPrevPagination = "AspNet-DetailsView-FirstPreviousPagination";
			_classNextLastPagination = "AspNet-DetailsView-NextLastPagination";
			_classFirstPage = "AspNet-DetailsView-FirstPage";
			_classNextPage = "AspNet-DetailsView-NextPage";
			_classPreviousPage = "AspNet-DetailsView-PreviousPage";
			_classLastPage = "AspNet-DetailsView-LastPage";
			_classPagingLinkDisabled = "AspNet-DetailsView-PagingDisabled";
		}

		/// <remarks>
		/// Applied CodePlex patch #337/338 provided by tonyhild to apply the <c>AspNet-DetailsView-RowCommand</c> class to the
		/// list item row for command buttons.
		/// </remarks>
		protected override void BuildItem(HtmlTextWriter writer)
		{
			if (IsDetailsView && (ControlAsDetailsView.Rows.Count > 0))
			{
				writer.WriteLine();
				writer.WriteBeginTag("div");
				writer.WriteAttribute("class", _classData);
				writer.Write(HtmlTextWriter.TagRightChar);
				writer.Indent++;

				// Check for empty data set
				// If no data is provided, render EmptyDataTemplate or EmptyDataText
				if (ControlAsDetailsView.DataItemCount == 0 && ControlAsDetailsView.CurrentMode != DetailsViewMode.Insert)
				{
					writer.WriteBeginTag("div");
					writer.WriteAttribute("class", _classEmptyData);
					writer.Write(HtmlTextWriter.TagRightChar);
					writer.Indent++;

					PlaceHolder placeholder = new PlaceHolder();

					// render EmptyDataTemplate or EmptyDataText
					if (ControlAsDetailsView.EmptyDataTemplate != null)
					{
						ControlAsDetailsView.EmptyDataTemplate.InstantiateIn(placeholder);
					}
					else
					{
						Literal emptydatatext = new Literal();
						emptydatatext.Text = ControlAsDetailsView.EmptyDataText;
						placeholder.Controls.Add(emptydatatext);
					}

					placeholder.RenderControl(writer);

					writer.Indent--;
					writer.WriteLine();
					writer.WriteEndTag("div");
				}
				else
				{
					// render normally
					writer.WriteLine();
					writer.WriteBeginTag("ul");
					writer.Write(HtmlTextWriter.TagRightChar);
					writer.Indent++;

					bool useFields = (!ControlAsDetailsView.AutoGenerateRows) && (ControlAsDetailsView.Fields.Count == ControlAsDetailsView.Rows.Count);
					int countRenderedRows = 0;
					for (int iRow = 0; iRow < ControlAsDetailsView.Rows.Count; iRow++)
					{
						if ((!useFields) || (useFields && ControlAsDetailsView.Fields[iRow].Visible) && ControlAsDetailsView.Rows[iRow].Visible)
						{
							DetailsViewRow row = ControlAsDetailsView.Rows[iRow];
							if ((!ControlAsDetailsView.AutoGenerateRows) &&
									((row.RowState & DataControlRowState.Insert) == DataControlRowState.Insert) &&
									(iRow < ControlAsDetailsView.Fields.Count) &&
									(!ControlAsDetailsView.Fields[row.RowIndex].InsertVisible))
							{
								continue;
							}

							writer.WriteLine();
							writer.WriteBeginTag("li");
							bool isRowCommand = (row.Cells[0] as DataControlFieldCell).ContainingField is ButtonFieldBase;
							string theClass = string.Empty;
							if (isRowCommand)
							{
								theClass = "AspNet-DetailsView-RowCommand";
							}
							else
							{
								theClass = ((countRenderedRows % 2) == 1) ? "AspNet-DetailsView-Alternate" : "";
							}
							if (useFields && (ControlAsDetailsView.Fields[iRow].ItemStyle != null) && (!String.IsNullOrEmpty(ControlAsDetailsView.Fields[iRow].ItemStyle.CssClass)))
							{
								if (!String.IsNullOrEmpty(theClass))
								{
									theClass += " ";
								}
								theClass += ControlAsDetailsView.Fields[iRow].ItemStyle.CssClass;
							}
							if (!String.IsNullOrEmpty(theClass))
							{
								writer.WriteAttribute("class", theClass);
							}
							writer.Write(HtmlTextWriter.TagRightChar);
							writer.Indent++;
							writer.WriteLine();

							for (int iCell = 0; iCell < row.Cells.Count; iCell++)
							{
								TableCell cell = row.Cells[iCell];
								if (!isRowCommand)
								{
									writer.WriteBeginTag("span");
									if (iCell == 0)
									{
										writer.WriteAttribute("class", "AspNet-DetailsView-Name");
									}
									else if (iCell == 1)
									{
										writer.WriteAttribute("class", "AspNet-DetailsView-Value");
									}
									else
									{
										writer.WriteAttribute("class", "AspNet-DetailsView-Misc");
									}
									writer.Write(HtmlTextWriter.TagRightChar);
								}
								if (!String.IsNullOrEmpty(cell.Text))
								{
									writer.Write(cell.Text);
								}
								foreach (Control cellChildControl in cell.Controls)
								{
									cellChildControl.RenderControl(writer);
								}
								if (!isRowCommand)
									writer.WriteEndTag("span");
							}

							writer.Indent--;
							writer.WriteLine();
							writer.WriteEndTag("li");
							countRenderedRows++;
						}
					}

					writer.Indent--;
					writer.WriteLine();
					writer.WriteEndTag("ul");
				}

				writer.Indent--;
				writer.WriteLine();
				writer.WriteEndTag("div");
			}
		}

		protected override void RegisterScripts()
		{
			base.RegisterScripts();

			/* 
			 * Modified for support of compiled CSSFriendly assembly
			 * 
			 * We will search for embedded CSS files. If they are 
			 * found, we include them.
			 */

			Type type = typeof(CSSFriendly.DetailsViewAdapter);
			Helpers.RegisterEmbeddedCSS("CSSFriendly.CSS.DetailsView.css", type, this.Page);
		}
	}
}
