using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace VDMS.II.Common.Web
{
	public class WebUtils
	{
		public static void ShowGridAlways(ref GridView gridView, ref DataTable dt, string emptyMessage)
		{
			if (dt.Rows.Count == 0)
			{
				// Remove contraints so an empty row can be added.
				dt.Constraints.Clear();
				foreach (DataColumn dc in dt.Columns)
					dc.AllowDBNull = true;

				// Add a blank row to the dataset
				dt.Columns[0].AllowDBNull = true;
				dt.Rows.Add(dt.NewRow());
				// Bind the DataSet to the GridView
				gridView.DataSource = dt;
				gridView.DataBind();
				// Get the number of columns to know what the Column Span should be
				int columnCount = gridView.Rows[0].Cells.Count;
				// Call the clear method to clear out any controls that you use in the columns.  I use a dropdown list in one of the column so this was necessary.
				gridView.Rows[0].Cells.Clear();
				gridView.Rows[0].Cells.Add(new TableCell());
				gridView.Rows[0].Cells[0].ColumnSpan = columnCount;
				gridView.Rows[0].Cells[0].Text = emptyMessage;
			}
			else
			{
				gridView.DataSource = dt;
				gridView.DataBind();
			}
		}

		public static void ShowEmptyGridView<T>(GridView gv) where T : new()
		{
			List<T> items = new List<T>();
			items.Add(new T());
			gv.DataSource = items;
			gv.DataBind();
			gv.Rows[0].Cells[0].ColumnSpan = gv.Columns.Count;
			gv.Rows[0].Cells[0].Text = gv.EmptyDataText;
			gv.Rows[0].Cells[0].CssClass = gv.EmptyDataRowStyle.CssClass;
			for (int i = 1; i < gv.Columns.Count; i++)
			{
				gv.Rows[0].Cells[i].Visible = false;
			}
		}
	}
}
