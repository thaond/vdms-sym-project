using System;
using System.Web.UI.WebControls;
using Resources;

namespace VDMS.II.WebControls
{
	public class PageGridView : GridView
	{
		public string DealerCode { get; set; }
		public string PagerInfoCssClass { get; set; }

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			//this.AllowPaging = true;
			//this.PagerSettings.Position = PagerPosition.Bottom;
			//this.PagerSettings.Mode = PagerButtons.NumericFirstLast;
		}

		protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
		{
			base.InitializePager(row, columnSpan, pagedDataSource);
			if (pagedDataSource.IsPagingEnabled && !pagedDataSource.IsCustomPagingEnabled)
			{
				//FormatString:  Page: {0}/{1} (Total: {2} items)
				string text = string.Format(Message.PagingInfo, pagedDataSource.CurrentPageIndex + 1, pagedDataSource.PageCount, pagedDataSource.DataSourceCount);
				TableCell cell = new TableCell() { Text = text };
				Table tbl = (Table)row.Cells[0].Controls[0];

				cell.CssClass = PagerInfoCssClass;
				cell.Width = Unit.Percentage(100);
				tbl.Controls[0].Controls.Add(cell);
			}
		}

	}
}