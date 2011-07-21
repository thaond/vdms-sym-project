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
	public class GridViewAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
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
				Extender.RenderBeginTag(writer, "AspNet-GridView");
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
				GridView gridView = Control as GridView;
				if (gridView != null)
				{
					writer.Indent++;
					WritePagerSection(writer, PagerPosition.Top);

					writer.WriteLine();
					writer.WriteBeginTag("table");
					writer.WriteAttribute("cellpadding", "0");
					writer.WriteAttribute("cellspacing", "0");
					writer.WriteAttribute("summary", Control.ToolTip);

					if (!String.IsNullOrEmpty(gridView.CssClass))
					{
						writer.WriteAttribute("class", gridView.CssClass);
					}

					writer.Write(HtmlTextWriter.TagRightChar);
					writer.Indent++;

					///////////////////// CAPTION /////////////////////////////

					if (!String.IsNullOrEmpty(gridView.Caption))
					{
						writer.WriteLine();
						writer.WriteBeginTag("caption");
						writer.WriteAttribute("class", "AspNet-GridView-Caption");
						writer.Write(HtmlTextWriter.TagRightChar);
						writer.WriteEncodedText(gridView.Caption);
						writer.WriteEndTag("caption");
					}

					ArrayList rows = new ArrayList();

					/* ADDED ON 3/7/07, source: http://forums.asp.net/thread/1518559.aspx */
					/////////////// EmptyDataTemplate ///////////////////////
					if (gridView.Rows.Count == 0)
					{
						//Control[0].Control[0] s/b the EmptyDataTemplate.
						if (gridView.HasControls())
						{
							if (gridView.Controls[0].HasControls())
							{
								Control c = gridView.Controls[0].Controls[0];
								if (c is GridViewRow)
								{
									rows.Clear();
									rows.Add(c);
									WriteRows(writer, gridView,
											  new GridViewRowCollection(rows), "tfoot");
								}
							}
						}
					}
					/* END ADD */
					else
					{

						///////////////////// HEAD /////////////////////////////

						rows.Clear();
						if (gridView.ShowHeader && (gridView.HeaderRow != null))
						{
							rows.Add(gridView.HeaderRow);
							WriteRows(writer, gridView, new GridViewRowCollection(rows), "thead");
						}

						///////////////////// FOOT /////////////////////////////

						rows.Clear();
						if (gridView.ShowFooter && (gridView.FooterRow != null))
						{
							rows.Add(gridView.FooterRow);
							WriteRows(writer, gridView, new GridViewRowCollection(rows), "tfoot");
						}

						///////////////////// BODY /////////////////////////////

						WriteRows(writer, gridView, gridView.Rows, "tbody");

						////////////////////////////////////////////////////////
					}


					writer.Indent--;
					writer.WriteLine();
					writer.WriteEndTag("table");

					WritePagerSection(writer, PagerPosition.Bottom);

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

		private void WriteRows(HtmlTextWriter writer, GridView gridView, GridViewRowCollection rows, string tableSection)
		{
			if (rows.Count > 0)
			{
				writer.WriteLine();
				writer.WriteBeginTag(tableSection);
				writer.Write(HtmlTextWriter.TagRightChar);
				writer.Indent++;

				foreach (GridViewRow row in rows)
				{
					if (!row.Visible)
						continue;

					writer.WriteLine();
					writer.WriteBeginTag("tr");

					string className = GetRowClass(gridView, row);
					if (!String.IsNullOrEmpty(className))
					{
						writer.WriteAttribute("class", className);
					}

					//  Uncomment the following block of code if you want to add arbitrary attributes.
					/*
					foreach (string key in row.Attributes.Keys)
					{
							writer.WriteAttribute(key, row.Attributes[key]);
					}
					*/

					writer.Write(HtmlTextWriter.TagRightChar);
					writer.Indent++;

					foreach (TableCell cell in row.Cells)
					{
						DataControlFieldCell fieldCell = cell as DataControlFieldCell;
						if ((fieldCell != null) && (fieldCell.ContainingField != null))
						{
							DataControlField field = fieldCell.ContainingField;
							if (!field.Visible)
							{
								cell.Visible = false;
							}

							// Apply item style CSS class
							TableItemStyle itemStyle;
							switch (row.RowType)
							{
								case DataControlRowType.Header:
									itemStyle = field.HeaderStyle;
									break;
								case DataControlRowType.Footer:
									itemStyle = field.FooterStyle;
									break;
								default:
									itemStyle = field.ItemStyle;
									break;
							}
							if (itemStyle != null && !String.IsNullOrEmpty(itemStyle.CssClass))
							{
								if (!String.IsNullOrEmpty(cell.CssClass))
									cell.CssClass += " ";
								cell.CssClass += itemStyle.CssClass;
							}
						}

						writer.WriteLine();
						cell.RenderControl(writer);
					}

					writer.Indent--;
					writer.WriteLine();
					writer.WriteEndTag("tr");
				}

				writer.Indent--;
				writer.WriteLine();
				writer.WriteEndTag(tableSection);
			}
		}

		/// <summary>
		/// Gets the row's CSS class.
		/// </summary>
		/// <param name="gridView">The grid view.</param>
		/// <param name="row">The row.</param>
		/// <returns>The CSS class.</returns>
		/// <remarks>
		/// Modified 10/31/2007 by SelArom to create CSS classes for all different row types and states.
		/// </remarks>
		private string GetRowClass(GridView gridView, GridViewRow row)
		{
			string className = row.CssClass;

			switch (row.RowType)
			{
				case DataControlRowType.Header:
					className += " AspNet-GridView-Header " + gridView.HeaderStyle.CssClass;
					break;
				case DataControlRowType.Footer:
					className += " AspNet-GridView-Footer " + gridView.FooterStyle.CssClass;
					break;
				case DataControlRowType.EmptyDataRow:
					className += " AspNet-GridView-Empty " + gridView.EmptyDataRowStyle.CssClass;
					break;
				case DataControlRowType.Separator:
					className += " AspNet-GridView-Separator ";
					break;
				case DataControlRowType.Pager:
					className += " AspNet-GridView-Pagination " + gridView.PagerStyle.CssClass;
					break;
				case DataControlRowType.DataRow:
					switch (row.RowState)
					{
						case DataControlRowState.Normal:
							className += " AspNet-GridView-Normal " + gridView.RowStyle.CssClass;
							break;
						case DataControlRowState.Alternate:
							className += " AspNet-GridView-Alternate " + gridView.AlternatingRowStyle.CssClass;
							break;
						case DataControlRowState.Selected | DataControlRowState.Normal:
						case DataControlRowState.Selected | DataControlRowState.Alternate:
							className += " AspNet-GridView-Selected " + gridView.SelectedRowStyle.CssClass;
							break;
						case DataControlRowState.Edit | DataControlRowState.Normal:
						case DataControlRowState.Edit | DataControlRowState.Alternate:
							className += " AspNet-GridView-Edit " + gridView.EditRowStyle.CssClass;
							break;
						case DataControlRowState.Insert:
							className += " AspNet-GridView-Insert ";
							break;
					}
					break;
			}

			return className.Trim();
		}

		/// <remarks>
		/// Patch provided by Wizzard to support PagerTemplate (CodePlex issue #3368).
		/// </remarks>
		private void WritePagerSection(HtmlTextWriter writer, PagerPosition pos)
		{
			GridView gridView = Control as GridView;
			if ((gridView != null) &&
					gridView.AllowPaging &&
				gridView.PagerSettings.Visible &&
					(gridView.PageCount > 1) &&
					((gridView.PagerSettings.Position == pos) || (gridView.PagerSettings.Position == PagerPosition.TopAndBottom)))
			{
				GridViewRow pagerRow = (pos == PagerPosition.Top) ? gridView.TopPagerRow : gridView.BottomPagerRow;
				string className = GetRowClass(gridView, pagerRow);
				className += " AspNet-GridView-" + (pos == PagerPosition.Top ? "Top " : "Bottom ");

				//check for PagerTemplate
				if (gridView.PagerTemplate != null)
				{
					if (gridView.PagerStyle != null)
					{
						className += gridView.PagerStyle.CssClass;
					}
					className = className.Trim();

					writer.WriteLine();
					writer.WriteBeginTag("div");
					writer.WriteAttribute("class", className);
					writer.Write(HtmlTextWriter.TagRightChar);
					writer.Indent++;

					if (pagerRow != null)
					{
						foreach (TableCell cell in pagerRow.Cells)
						{
							foreach (Control ctrl in cell.Controls)
							{
								ctrl.RenderControl(writer);
							}
						}
					}

					writer.Indent--;
					writer.WriteEndTag("div");
				}
				else //if not a PagerTemplate 
				{
					Table innerTable = null;
					if ((pos == PagerPosition.Top) &&
							(gridView.TopPagerRow != null) &&
							(gridView.TopPagerRow.Cells.Count == 1) &&
							(gridView.TopPagerRow.Cells[0].Controls.Count == 1) &&
							typeof(Table).IsAssignableFrom(gridView.TopPagerRow.Cells[0].Controls[0].GetType()))
					{
						innerTable = gridView.TopPagerRow.Cells[0].Controls[0] as Table;
					}
					else if ((pos == PagerPosition.Bottom) &&
							(gridView.BottomPagerRow != null) &&
							(gridView.BottomPagerRow.Cells.Count == 1) &&
							(gridView.BottomPagerRow.Cells[0].Controls.Count == 1) &&
							typeof(Table).IsAssignableFrom(gridView.BottomPagerRow.Cells[0].Controls[0].GetType()))
					{
						innerTable = gridView.BottomPagerRow.Cells[0].Controls[0] as Table;
					}

					if ((innerTable != null) && (innerTable.Rows.Count == 1))
					{
						if (gridView.PagerStyle != null)
						{
							className += gridView.PagerStyle.CssClass;
						}
						className = className.Trim();

						writer.WriteLine();
						writer.WriteBeginTag("div");
						writer.WriteAttribute("class", className);
						writer.Write(HtmlTextWriter.TagRightChar);
						writer.Indent++;

						TableRow row = innerTable.Rows[0];
						foreach (TableCell cell in row.Cells)
						{
							foreach (Control ctrl in cell.Controls)
							{
								writer.WriteLine();
								ctrl.RenderControl(writer);
							}
						}

						writer.Indent--;
						writer.WriteLine();
						writer.WriteEndTag("div");
					}
				}
			}
		}
	}
}
