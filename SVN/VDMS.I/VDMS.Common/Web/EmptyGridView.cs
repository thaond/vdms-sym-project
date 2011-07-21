using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VDMS.Common.Web
{
    public class EmptyGridView : GridView
    {
        #region Properties

        /// <summary>
        /// Enable or Disable generating an empty table if no data rows in source
        /// </summary>
        [
        Description("Enable or disable generating an empty table with headers if no data rows in source"),
        Category("Misc"),
        DefaultValue("true"),
        ]
        public bool ShowEmptyTable
        {
            get
            {
                object o = ViewState["ShowEmptyTable"];
                return (o != null ? (bool)o : true);
            }
            set
            {
                ViewState["ShowEmptyTable"] = value;
            }
        }

        /// <summary>
        /// Get or Set Text to display in empty data row
        /// </summary>
        [
        Description("Text to display in empty data row"),
        Category("Misc"),
        DefaultValue(""),
        ]
        public string EmptyTableRowText
        {
            get
            {
                object o = ViewState["EmptyTableRowText"];
                return (o != null ? o.ToString() : "");
            }
            set
            {
                ViewState["EmptyTableRowText"] = value;
            }
        }

        #endregion


        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            int numRows = base.CreateChildControls(dataSource, dataBinding);

            //no data rows created, create empty table if enabled
            if (numRows == 0 && ShowEmptyTable)
            {
                //create table
                Table table = new Table();
                table.ID = this.ID;

                //create a new header row
                GridViewRow row = base.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);

                //convert the exisiting columns into an array and initialize
                DataControlField[] fields = new DataControlField[this.Columns.Count];
                this.Columns.CopyTo(fields, 0);
                this.InitializeRow(row, fields);
                table.Rows.Add(row);

                //create the empty row
                row = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
                TableCell cell = new TableCell();
                cell.ColumnSpan = this.Columns.Count;
                cell.Width = Unit.Percentage(100);
                cell.Controls.Add(new LiteralControl(EmptyTableRowText));
                row.Cells.Add(cell);
                table.Rows.Add(row);

                this.Controls.Add(table);
            }

            return numRows;
        }
    }
}