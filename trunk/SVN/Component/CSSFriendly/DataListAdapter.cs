using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
	public class DataListAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
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

		private int RepeatColumns
		{
			get
			{
				DataList dataList = Control as DataList;
				int nRet = 1;
				if (dataList != null)
				{
					if (dataList.RepeatColumns == 0)
					{
						if (dataList.RepeatDirection == RepeatDirection.Horizontal)
						{
							nRet = dataList.Items.Count;
						}
					}
					else
					{
						nRet = dataList.RepeatColumns;
					}
				}
				return nRet;
			}
		}

		/// ///////////////////////////////////////////////////////////////////////////////
		/// PROTECTED        

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
				Extender.RenderBeginTag(writer, "AspNet-DataList");
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
				DataList dataList = Control as DataList;
				int columns = (dataList.SeparatorTemplate == null ? RepeatColumns : RepeatColumns * 2);

				if (dataList != null)
				{
					writer.Indent++;
					writer.WriteLine();
					writer.WriteBeginTag("table");
					writer.WriteAttribute("cellpadding", dataList.CellPadding.ToString());
					writer.WriteAttribute("cellspacing", dataList.CellSpacing.ToString());
					writer.WriteAttribute("summary", Control.ToolTip);
					writer.Write(HtmlTextWriter.TagRightChar);
					writer.Indent++;

					if (dataList.HeaderTemplate != null && dataList.ShowHeader)
					{
						if (!String.IsNullOrEmpty(dataList.Caption))
						{
							writer.WriteLine();
							writer.WriteBeginTag("caption");
							writer.Write(HtmlTextWriter.TagRightChar);
							writer.Write(dataList.Caption);
							writer.WriteEndTag("caption");
						}

						writer.WriteLine();
						writer.WriteBeginTag("thead");
						writer.Write(HtmlTextWriter.TagRightChar);
						writer.Indent++;

						writer.WriteLine();
						writer.WriteBeginTag("tr");
						writer.Write(HtmlTextWriter.TagRightChar);
						writer.Indent++;

						writer.WriteLine();
						writer.WriteBeginTag("th");
						writer.WriteAttribute("colspan", columns.ToString());
						string className = "AspNet-DataList-Header ";
						if (dataList.HeaderStyle != null)
						{
							className += dataList.HeaderStyle.CssClass;
						}
						writer.WriteAttribute("class", className);
						writer.Write(HtmlTextWriter.TagRightChar);
						writer.Indent++;
						
						PlaceHolder container = new PlaceHolder();
						dataList.HeaderTemplate.InstantiateIn(container);
						container.DataBind();
						container.RenderControl(writer);

						writer.Indent--;
						writer.WriteLine();
						writer.WriteEndTag("th");

						writer.Indent--;
						writer.WriteLine();
						writer.WriteEndTag("tr");

						writer.Indent--;
						writer.WriteLine();
						writer.WriteEndTag("thead");

					}

					if (dataList.FooterTemplate != null && dataList.ShowFooter)
					{
						writer.WriteLine();
						writer.WriteBeginTag("tfoot");
						writer.Write(HtmlTextWriter.TagRightChar);
						writer.Indent++;

						writer.WriteLine();
						writer.WriteBeginTag("tr");
						writer.Write(HtmlTextWriter.TagRightChar);
						writer.Indent++;

						writer.WriteLine();
						writer.WriteBeginTag("td");
						writer.WriteAttribute("colspan", columns.ToString());
						string className = "AspNet-DataList-Footer ";
						if (dataList.FooterStyle != null)
						{
							className += dataList.FooterStyle.CssClass;
						}
						writer.WriteAttribute("class", className);
						writer.Write(HtmlTextWriter.TagRightChar);
						writer.Indent++;

						PlaceHolder container = new PlaceHolder();
						dataList.FooterTemplate.InstantiateIn(container);
						container.DataBind();
						container.RenderControl(writer);

						writer.Indent--;
						writer.WriteLine();
						writer.WriteEndTag("td");

						writer.Indent--;
						writer.WriteLine();
						writer.WriteEndTag("tr");

						writer.Indent--;
						writer.WriteLine();
						writer.WriteEndTag("tfoot");
					}

					if (dataList.ItemTemplate != null)
					{
						writer.WriteLine();
						writer.WriteBeginTag("tbody");
						writer.Write(HtmlTextWriter.TagRightChar);
						writer.Indent++;

						int nItemsInColumn = (int)Math.Ceiling(((Double)dataList.Items.Count) / ((Double)RepeatColumns));
						for (int iItem = 0; iItem < dataList.Items.Count; iItem++)
						{
							int nRow = iItem / RepeatColumns;
							int nCol = iItem % RepeatColumns;
							int nDesiredIndex = iItem;
							if (dataList.RepeatDirection == RepeatDirection.Vertical)
							{
								nDesiredIndex = (nCol * nItemsInColumn) + nRow;
							}

							if ((iItem % RepeatColumns) == 0)
							{
								writer.WriteLine();
								writer.WriteBeginTag("tr");
								writer.Write(HtmlTextWriter.TagRightChar);
								writer.Indent++;
							}

							writer.WriteLine();
							writer.WriteBeginTag("td");

							string className = GetRowClass(dataList, dataList.Items[iItem]);
							if (!String.IsNullOrEmpty(className))
							{
								writer.WriteAttribute("class", className);
							}

							writer.Write(HtmlTextWriter.TagRightChar);
							writer.Indent++;

							foreach (Control itemCtrl in dataList.Items[iItem].Controls)
							{
								itemCtrl.RenderControl(writer);
							}

							writer.Indent--;
							writer.WriteLine();
							writer.WriteEndTag("td");

							if (dataList.SeparatorTemplate != null && (iItem+1 < dataList.Items.Count))
							{
								writer.WriteLine();
								writer.WriteBeginTag("td");
								className = "AspNet-DataList-Separator ";
								if (dataList.SeparatorStyle != null)
								{
									className += dataList.SeparatorStyle.CssClass;
								}
								writer.WriteAttribute("class", className.Trim());
								writer.Write(HtmlTextWriter.TagRightChar);
								writer.Indent++;

								PlaceHolder container = new PlaceHolder();
								dataList.SeparatorTemplate.InstantiateIn(container);
								container.DataBind();
								container.RenderControl(writer);

								writer.Indent--;
								writer.WriteLine();
								writer.WriteEndTag("td");
							}

							if (((iItem + 1) % RepeatColumns) == 0)
							{
								writer.Indent--;
								writer.WriteLine();
								writer.WriteEndTag("tr");
							}
						}

						if ((dataList.Items.Count % RepeatColumns) != 0)
						{
							writer.Indent--;
							writer.WriteLine();
							writer.WriteEndTag("tr");
						}

						writer.Indent--;
						writer.WriteLine();
						writer.WriteEndTag("tbody");
					}

					writer.Indent--;
					writer.WriteLine();
					writer.WriteEndTag("table");

					writer.Indent--;
					writer.WriteLine();
				}
			}
			else
			{
				base.RenderContents(writer);
			}
		}

		/// ///////////////////////////////////////////////////////////////////////////////
		/// PRIVATE        

		private void RegisterScripts()
		{
		}

		/// <summary>
		/// Gets the row's CSS class.
		/// </summary>
		/// <param name="DataList">The data list.</param>
		/// <param name="Item">The current item.</param>
		/// <returns>The CSS class.</returns>
		/// <remarks>
		/// Added 10/31/2007 by SelArom based on similar code for GridView control.
		/// </remarks>
		private string GetRowClass(DataList list, DataListItem item)
		{
			string className = item.CssClass;

			if (list.ItemStyle != null)
			{
				className += list.ItemStyle.CssClass;
			}

			switch (item.ItemType)
			{
				case ListItemType.Item:
					className += " AspNet-DataList-Item ";
					if (list.ItemStyle != null)
					{
						className += list.AlternatingItemStyle.CssClass;
					}
					break;
				case ListItemType.AlternatingItem:
					className += " AspNet-DataList-Alternate ";
					if (list.AlternatingItemStyle != null)
					{
						className += list.AlternatingItemStyle.CssClass;
					}
					break;
				case ListItemType.EditItem:
					className += " AspNet-DataList-EditItem ";
					if (list.EditItemStyle != null)
					{
						className += list.EditItemStyle.CssClass;
					}
					break;
				case ListItemType.SelectedItem:
					className += " AspNet-DataList-SelectedItem ";
					if (list.SelectedItemStyle != null)
					{
						className += list.SelectedItemStyle.CssClass;
					}
					break;
			}

			return className.Trim();
		}
	}
}
