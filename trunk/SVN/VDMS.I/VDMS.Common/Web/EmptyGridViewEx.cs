using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Common.Data;

namespace VDMS
{
    namespace Common.Web
    {
        public partial class WebTools
        {
            public delegate bool ItemValidator(object c);

            public static void FindControls(string id, Control ctrlToFind, ref ArrayList ctrls, ItemValidator valid)
            {
                foreach (Control ctrl in ctrlToFind.Controls)
                {
                    if (ctrl.ID == id && valid(ctrl)) ctrls.Add(ctrl);
                    FindControls(id, ctrl, ref ctrls, valid);
                }
            }
            public static void FindControls(string id, Control ctrlToFind, ref ArrayList ctrls)
            {
                foreach (Control ctrl in ctrlToFind.Controls)
                {
                    if (ctrl.ID == id) ctrls.Add(ctrl);
                    FindControls(id, ctrl, ref ctrls);
                }
            }
            public static Control FindControlById(string id, Control parent)
            {
                Control obj;
                foreach (Control ctrl in parent.Controls)
                {
                    if (!string.IsNullOrEmpty(ctrl.ID) && ctrl.ID == id) return ctrl;
                    obj = FindControlById(id, ctrl);
                    if (obj != null) return obj;
                }
                return null;
            }
            public static Control FindControlByType(Type ctrlType, Control parent)
            {
                Control obj;
                foreach (Control ctrl in parent.Controls)
                {
                    if (ctrl.GetType() == ctrlType) return ctrl;
                    obj = FindControlByType(ctrlType, ctrl);
                    if (obj != null) return obj;
                }
                return null;
            }
            private static void ColectControlByType(Type ctrlType, Control parent, Hashtable list)
            {
                if (list == null) return;
                foreach (Control ctrl in parent.Controls)
                {
                    if ((ctrl.GetType() == ctrlType) && (ctrl.ID != null) && (!list.Contains(ctrl.ID))) list.Add(ctrl.ID, ctrl);
                    ColectControlByType(ctrlType, ctrl, list);
                }
            }
            public static Hashtable FindAllControlByType(Type ctrlType, Control parent)
            {
                Hashtable list = new Hashtable();
                ColectControlByType(ctrlType, parent, list);
                return (list.Count > 0) ? list : null;
            }
        }

        /// <summary>
        /// Insert command: "InsertSimpleRow"
        /// When Inserting, updating use "NewSimpleRow" to access data
        /// Use in page: <%@ Register TagPrefix="SCS" Namespace="SCS.Common.Web" %>
        /// </summary>
        public class EmptyGridViewEx : GridView
        {
            // use this to install GridView: <%@ Register TagPrefix="SCS" Namespace="SCS.Common.Web"%>
            private DataTable _dataTable = null, _dataObjectTableSchema = null, spanDataTable = null;
            private DataRow _insertRow = null, _oldRow = null;
            private object _dataObject = null;
            private string _idPrefix, _emptyTableRowText = "";
            private ArrayList[] _columnsLevelList = null;
            private bool _allowInsertEmpty = false, _includeChildsListInLevel = false, _gennerateSpanTable = false, _showEmptyTable = true, _showEmptyFooter = true;
            private int _realPageSize;

            public bool _debugGridOnObject = false;

            #region Properties

            /// <summary>
            /// Enable or Disable generating an empty table if no data rows in source
            /// </summary>
            [
            Description("Generate data table which all deleted cell are null"),
            Category("Misc"),
            DefaultValue("false"),
            ]
            public bool GennerateSpanDataTable
            {
                get { return _gennerateSpanTable; }
                set { _gennerateSpanTable = value; }
            }

            [
            Description("Enable or disable generating an empty table with headers if no data rows in source"),
            Category("Misc"),
            DefaultValue("true"),
            ]
            public bool ShowEmptyTable
            {
                get { return _showEmptyTable; }
                set { _showEmptyTable = value; }
            }           
            public bool ShowEmptyFooter
            {
                get { return _showEmptyFooter; }
                set { _showEmptyFooter = value; }
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
                get { return (_emptyTableRowText); }
                set { _emptyTableRowText = value; }
            }

            /// <summary>
            /// Id prefix of controls that submit value to GridView
            /// </summary>
            [
            Description("Id prefix of controls that submit value to GridView"),
            Category("Misc"),
            DefaultValue("sub"),
            ]
            public string SubmitControlIdPrefix
            {
                get { return string.IsNullOrEmpty(_idPrefix) ? "sub" : _idPrefix; }
                set { _idPrefix = value.Trim(); }
            }

            /// <summary>
            /// Allow insert empty row in Simple data source mode
            /// </summary>
            [
            Description("Allow insert empty row in Simple data source mode"),
            Category("Misc"),
            DefaultValue("false"),
            ]
            public bool AllowInsertEmptyRow
            {
                get { return _allowInsertEmpty; }
                set { _allowInsertEmpty = value; }
            }
            /// <summary>
            /// Include childs list's property to its container level. In Object data source mode
            /// </summary>
            [
            Description("Include childs list's property to its container level. In Object data source mode"),
            Category("Misc"),
            DefaultValue("false"),
            ]
            public bool IncludeChildsListInLevel
            {
                get { return _includeChildsListInLevel; }
                set { _includeChildsListInLevel = value; }
            }

            /// <summary>
            /// DataTable hold data for GridView
            /// </summary>
            public DataTable DataSourceTable
            {
                get { return _dataTable; }
                set { _dataTable = value; }
            }
            public DataTable SpanDataTable
            {
                get { return spanDataTable; }
                set { spanDataTable = value; }
            }
            public object DataSourceObject
            {
                get { return _dataObject; }
                set { _dataObject = value; }
            }
            public DataTable DataObjectTableSchema
            {
                get { return _dataObjectTableSchema; }
                set { _dataObjectTableSchema = value; }
            }
            public ArrayList[] ColumnsLevelList
            {
                get { return _columnsLevelList; }
                set { _columnsLevelList = value; }
            }
            public bool IsSimpleDataSounceUsed
            {
                get { return ((DataSourceTable != null) && string.IsNullOrEmpty(DataSourceID) && (DataSourceObject == null) && ((DataSource == null) || (DataSource == DataSourceTable))); }
            }
            public DataRow NewSimpleRow
            {
                get { return _insertRow; }
                set { _insertRow = value; }
            }
            public DataRow OldSimpleRow
            {
                get { return _oldRow; }
                set { _oldRow = value; }
            }
            public int RealPageSize
            {
                get { return (this.DataSourceObject != null) ? _realPageSize : this.PageSize; }
                set { if (this.DataSourceObject != null) _realPageSize = value; else PageSize = value; }
            }
            public GridViewRow FootRow
            {
                get
                {
                    if (base.FooterRow != null) return base.FooterRow;
                    Table tbl = (Table)this.FindControl(this.ID);
                    return ((tbl == null) || (tbl.Rows.Count == 0)) ? null : (GridViewRow)tbl.Rows[tbl.Rows.Count - 1];
                }
            }
            public GridViewRow HeadRow
            {
                get
                {
                    if (base.HeaderRow != null) return base.HeaderRow;
                    Table tbl = (Table)this.FindControl(this.ID);
                    return ((tbl == null) || (tbl.Rows.Count == 0)) ? null : (GridViewRow)tbl.Rows[0];
                }
            }
            #endregion

            #region SaveControlState
            protected override void OnInit(EventArgs e)
            {
                Page.RegisterRequiresControlState(this);
                base.OnInit(e);
            }
            protected override object SaveControlState()
            {
                object[] ctlState = new object[12];
                ctlState[0] = base.SaveControlState();
                ctlState[1] = _dataTable;
                ctlState[2] = _idPrefix;
                ctlState[3] = _allowInsertEmpty;
                ctlState[4] = _dataObjectTableSchema;
                ctlState[5] = _columnsLevelList;
                ctlState[6] = _includeChildsListInLevel;
                ctlState[7] = _realPageSize;
                ctlState[8] = _gennerateSpanTable;
                ctlState[9] = _showEmptyTable; ;
                ctlState[10] = _showEmptyFooter; ;
                ctlState[11] = _emptyTableRowText;
                return ctlState;
            }
            protected override void LoadControlState(object state)
            {
                if (state != null)
                {
                    object[] ctlState = (object[])state;
                    base.LoadControlState(ctlState[0]);
                    _dataTable = (DataTable)ctlState[1];
                    _idPrefix = (string)ctlState[2];
                    _allowInsertEmpty = (bool)ctlState[3];
                    _dataObjectTableSchema = (DataTable)ctlState[4];
                    _columnsLevelList = (ArrayList[])ctlState[5];
                    _includeChildsListInLevel = (bool)ctlState[6];
                    _realPageSize = (int)ctlState[7];
                    _gennerateSpanTable = (bool)ctlState[8];
                    _showEmptyTable = (bool)ctlState[9];
                    _showEmptyFooter = (bool)ctlState[10];
                    _emptyTableRowText = (string)ctlState[11];
                }
            }
            #endregion

            private void PrepareSpanningGrid(int level, int startRow, object listObject)
            {
                TreeListObject item;
                ArrayList list = ((TreeListObjects)listObject).Items;
                int dataColIndex = ((IncludeChildsListInLevel) ? level : level * 2);
                int keyColIndex = ((IncludeChildsListInLevel) ? level : level * 2 + 1);

                if (list == null) return;
                if (ColumnsLevelList == null) return;
                if ((keyColIndex >= ColumnsLevelList.Length) || (dataColIndex >= ColumnsLevelList.Length)) return;
                if (this.GennerateSpanDataTable) spanDataTable = (this.DataSource as DataTable).Copy();

                ArrayList dataColList = ColumnsLevelList[dataColIndex];
                ArrayList keyColList = ColumnsLevelList[keyColIndex];
                // loop for top level items
                for (int i = 0; i < list.Count; i++)
                {
                    item = (TreeListObject)list[i];

                    // mark row span for cell in current level data columns
                    if (!IncludeChildsListInLevel)
                    {
                        foreach (int colIndex in dataColList)
                        {
                            // span the first cell(top of span group) in column
                            if (!_debugGridOnObject)
                            {
                                if ((item.RowSpan > 1) && (startRow < this.Rows.Count)) this.Rows[startRow].Cells[colIndex].RowSpan = item.RowSpan;
                            }
                            else if ((item.RowSpan > 1) && (startRow < this.Rows.Count)) this.Rows[startRow].Cells[colIndex].CssClass = "value1";
                            // other will be delete 
                            for (int rowIndex = startRow + 1; rowIndex < (startRow + item.RowSpan); rowIndex++)
                            {
                                this.Rows[rowIndex].Cells[colIndex].RowSpan = 1; //(default=0)
                                if (this.GennerateSpanDataTable) SpanDataTable.Rows[rowIndex][colIndex] = null;
                            }
                        }
                    }

                    // loop for each childs list property
                    foreach (PropertyInfo prop in item.GetSortedBindProperties())
                    {
                        TreeListObjects childItems = (TreeListObjects)prop.GetValue(item, null);
                        // recursion down to process child level
                        PrepareSpanningGrid(level + 1, startRow, childItems);
                        // mark row span for cell in current level of childs's key columns
                        foreach (int colIndex in keyColList)
                        {
                            // span the first cell(top of span group) in column
                            if (!_debugGridOnObject)
                            {
                                if ((childItems.RowSpan > 1) && (startRow < this.Rows.Count)) this.Rows[startRow].Cells[colIndex].RowSpan = childItems.RowSpan;
                            }
                            else if ((childItems.RowSpan > 1) && (startRow < this.Rows.Count)) this.Rows[startRow].Cells[colIndex].CssClass = "value1";
                            // other will be delete 
                            for (int rowIndex = startRow + 1; rowIndex < (startRow + childItems.RowSpan); rowIndex++)
                            {
                                this.Rows[rowIndex].Cells[colIndex].RowSpan = 1; //(default=0)
                                if (this.GennerateSpanDataTable) SpanDataTable.Rows[rowIndex][colIndex] = null;
                            }
                        }
                        // add current level rows count to starting row Index for the rest of items in list
                        startRow += childItems.RowSpan;
                    }
                }
            }
            private void DoSpanningGrid()
            {
                foreach (GridViewRow row in this.Rows)
                {
                    int cellIndex = 0;
                    while (cellIndex < row.Cells.Count)
                    {
                        TableCell cell = row.Cells[cellIndex];
                        if (!_debugGridOnObject)
                        {
                            if (cell.RowSpan == 1) { row.Cells.Remove(cell); }
                            else cellIndex++;
                        }
                        else
                        {
                            if (cell.RowSpan > 1) { cell.CssClass = "value1"; }
                            else if (cell.RowSpan == 1) { cell.CssClass = "value2"; }
                            cellIndex++;
                        }
                    }
                }
            }
            // table==null => sum on (DataObjectTableSchema or DataSource)
            private long SumByColumnOn(string colName, DataTable table)
            {
                long cell, sum = 0;
                try
                {
                    DataTable tbl;
                    if (table != null) tbl = table;
                    else
                    {
                        if (this.DataSourceObject != null)
                        {
                            if (this.DataSource == null) // chua fill data from objects
                            {
                                tbl = this.DataObjectTableSchema;
                                ((TreeListObjects)this.DataSourceObject).FillDataTable(tbl);
                            }
                            else    // data filled to <DataSource>
                            { tbl = (DataTable)this.DataSource; }
                        }
                        else tbl = this.DataSourceTable;
                    }
                    foreach (DataRow row in tbl.Rows)
                    {
                        if (row[colName] != null)
                            if (long.TryParse(row[colName].ToString(), out cell)) sum += cell;
                    }
                }
                catch { }
                return sum;
            }
            public long SumBy(string colName)
            {
                return SumByColumnOn(colName, null);
            }
            public long GridSumBy(string columnName)
            {
                return SumByColumnOn(columnName, SpanDataTable);
            }

            public void _InitializeRow(GridViewRow row, DataControlField[] fields)
            {
                base.InitializeRow(row, fields);
            }
            public GridViewRow _CreateRow(int rowIndex, int dataIndex, DataControlRowType rowType, DataControlRowState rowState)
            {
                return base.CreateRow(rowIndex, dataIndex, rowType, rowState);
            }

            public DataRow GetSimpleRowByKey(string key, string value, bool allCharCase)
            {
                if ((!IsSimpleDataSounceUsed) || (DataSourceTable == null)) return null;
                string val1, val2;
                foreach (DataRow row in DataSourceTable.Rows)
                {
                    for (int i = 0; i < DataSourceTable.Columns.Count; i++)
                    {
                        val1 = (allCharCase) ? row[key].ToString().Trim().ToLower() : row[key].ToString().Trim();
                        val2 = (allCharCase) ? value.Trim().ToLower() : value.Trim();
                        if (val1 == val2) return row;
                    }
                }
                return null;
            }
            public void InsertSimpleRow(Hashtable values)
            {
                if ((this.IsSimpleDataSounceUsed) && (values != null) && (values.Count > 0))
                {
                    DataRow row = this.DataSourceTable.NewRow();
                    foreach (DictionaryEntry value in values)
                    {
                        if (this.DataSourceTable.Columns.Contains(value.Key.ToString()))
                            row[value.Key.ToString()] = value.Value;
                    }
                    this.DataSourceTable.Rows.Add(row);
                }
            }

            protected DataRow GetSubmitSimpleRow(Control parent)
            {
                // check for dataSourceTableExist and has at least 1 column to fill data
                if ((DataSourceTable == null) || (DataSourceTable.Columns.Count < 1)) return null;

                // colect all textbox hold data to insert in footer
                Hashtable listCol = WebTools.FindAllControlByType(typeof(TextBox), parent);
                if (listCol == null) return null;

                // fill data to new tablerow
                TextBox tb;
                DataRow row = DataSourceTable.NewRow();
                string colName, txt;
                for (int i = 0; i < DataSourceTable.Columns.Count; i++)
                {
                    txt = "";
                    colName = DataSourceTable.Columns[i].ColumnName.Trim();
                    if (colName == "") continue;

                    // check submited value in form
                    //if (!string.IsNullOrEmpty(Page.Request.Form[colName])) txt = Page.Request.Form[colName].Trim();
                    // find value in control which id is same with column name
                    else if (listCol.Contains(SubmitControlIdPrefix + colName))
                    {
                        tb = (TextBox)listCol[SubmitControlIdPrefix + colName];
                        if (tb != null) txt = tb.Text.Trim();
                    }
                    // set value to cell
                    row[colName] = txt;
                }

                // Check for empty row
                bool empty = true;
                for (int i = 0; i < DataSourceTable.Columns.Count; i++)
                {
                    if (!string.IsNullOrEmpty(row[i].ToString())) { empty = false; break; }
                }

                return ((!empty) || AllowInsertEmptyRow) ? row : null;
            }
            protected override void OnDataBinding(EventArgs e)
            {
                if ((this.DataSourceTable != null) && (this.DataSourceObject != null))
                { throw new Exception("Cannot set both DataSourceTable and DataSourceObject."); }

                if ((string.IsNullOrEmpty(this.DataSourceID)) && (this.DataSource == null))
                {
                    if (this.DataSourceTable != null) this.DataSource = this.DataSourceTable;
                    else if (this.DataSourceObject != null)
                    {
                        try
                        {
                            TreeListObjects list = (TreeListObjects)this.DataSourceObject;
                            DataTable tbl = DataObjectTableSchema.Clone();
                            list.FillDataTable(tbl);
                            this.DataSource = tbl;
                            if (this.AllowPaging) this.PageSize = tbl.Rows.Count;
                        }
                        catch { }
                    }
                }
                base.OnDataBinding(e);
            }
            protected override void OnRowDeleting(GridViewDeleteEventArgs e)
            {
                base.OnRowDeleting(e);
                if (IsSimpleDataSounceUsed && (!e.Cancel) && (e.RowIndex < DataSourceTable.Rows.Count))
                {
                    DataSourceTable.Rows.RemoveAt(e.RowIndex);
                    base.OnRowDeleted(new GridViewDeletedEventArgs(1, null));
                }
                //if (e.Cancel) this.OnRowCancelingEdit(new GridViewCancelEditEventArgs(e.RowIndex));
            }
            protected override void OnRowEditing(GridViewEditEventArgs e)
            {
                if ((IsSimpleDataSounceUsed) && (e.NewEditIndex >= DataSourceTable.Rows.Count)) { e.Cancel = true; return; }
                EditIndex = e.NewEditIndex;
                base.OnRowEditing(e);
            }
            protected override void OnRowUpdating(GridViewUpdateEventArgs e)
            {
                if (IsSimpleDataSounceUsed)
                {
                    NewSimpleRow = GetSubmitSimpleRow(this.Rows[e.RowIndex]);
                    OldSimpleRow = (e.RowIndex < DataSourceTable.Rows.Count) ? DataSourceTable.Rows[e.RowIndex] : null;
                }
                base.OnRowUpdating(e);
                if (IsSimpleDataSounceUsed && (!e.Cancel) && (NewSimpleRow != null))
                {
                    string colName;
                    for (int i = 0; i < DataSourceTable.Columns.Count; i++)
                    {
                        colName = DataSourceTable.Columns[i].ColumnName;
                        DataSourceTable.Rows[e.RowIndex][colName] = NewSimpleRow[colName].ToString();
                    }
                    this.OnRowUpdated(new GridViewUpdatedEventArgs(1, null));
                }
                if (e.Cancel) this.OnRowCancelingEdit(new GridViewCancelEditEventArgs(e.RowIndex));
            }
            protected override void OnRowUpdated(GridViewUpdatedEventArgs e)
            {
                //if (IsSimpleDataSounceUsed) 
                EditIndex = -1;
                base.OnRowUpdated(e);
            }
            protected override void OnRowCancelingEdit(GridViewCancelEditEventArgs e)
            {
                //if (IsSimpleDataSounceUsed) 
                EditIndex = -1;
                base.OnRowCancelingEdit(e);
            }
            protected override void OnRowCommand(GridViewCommandEventArgs e)
            {
                if (IsSimpleDataSounceUsed)
                    switch (e.CommandName)
                    {
                        case "InsertSimpleRow": NewSimpleRow = GetSubmitSimpleRow(this); break;
                    }

                // call next
                base.OnRowCommand(e);

                if (IsSimpleDataSounceUsed)
                    switch (e.CommandName)
                    {
                        case "InsertSimpleRow": if (NewSimpleRow != null) this.DataSourceTable.Rows.Add(NewSimpleRow); break;
                    }
            }
            protected override void OnRowDataBound(GridViewRowEventArgs e)
            {
                //e.Row.RowState = DataControlRowState.
                base.OnRowDataBound(e);
            }
            protected override void OnDataBound(EventArgs e)
            {
                base.OnDataBound(e);

                if (DataSourceObject != null)
                {
                    PrepareSpanningGrid(0, 0, DataSourceObject);
                    DoSpanningGrid();
                }
                //if ((this.DataSourceTable != null) || (this.DataSourceObject != null)) this.DataSource = null;

                //create a new header row
                //GridViewRow row = this.CreateRow(0, -1, DataControlRowType.Pager, DataControlRowState.Normal);

                //convert the exisiting columns into an array and initialize
                //DataControlField[] fields = new DataControlField[this.Columns.Count];
                //this.Columns.CopyTo(fields, 0);
                //this.InitializeRow(row, fields);
                //this.Controls[0].Controls.AddAt(0, row);
            }

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

                    if (this.ShowFooter && this.ShowEmptyFooter)
                    {
                        GridViewRow foot = base.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);
                        this.Columns.CopyTo(fields, 0);
                        this.InitializeRow(foot, fields);
                        table.Rows.Add(foot);
                    }

                    this.Controls.Add(table);
                }

                return numRows;
            }
        }
    }
}