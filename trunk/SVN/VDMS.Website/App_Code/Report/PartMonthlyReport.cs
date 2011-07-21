using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using CarlosAg.ExcelXmlWriter;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.Report
{
	public class MonthlyReportPart : MonthlyReportItem
	{
		public string PartCode { get; set; }
		public string PartType { get; set; }
		public string EnglishName { get; set; }
		public string VietnamName { get; set; }
		public long PartInfoId { get; set; }
		public IEnumerable<MonthlyReportItem> ByWarehouses { get; set; }
		public IEnumerable<MonthlyReportItem> ByDealers { get; set; }
	}

	public class MonthlyReportObject : MonthlyReportItem
	{
		//public string PlaceType { get; set; }
		//public string PlaceCode { get; set; }
		//public string PlaceName { get; set; }
		public IEnumerable<MonthlyReportPart> PartList { get; set; }
		public IEnumerable<MonthlyReportPart> AccList { get; set; }
	}

	public class MonthlyReportItem
	{
		public string PlaceType { get; set; }
		public string PlaceCode { get; set; }
		public string PlaceName { get; set; }
		public int BeginQuantity { get; set; }
		public int InQuantity { get; set; }
		public int OutQuantity { get; set; }
		public int InAmount { get; set; }
		public int OutAmount { get; set; }
		public int Balance { get; set; }
	}

	public class MonthlyReportPartInfo
	{
		public string PartCode { get; set; }
		public string EnglishName { get; set; }
		public string VietnamName { get; set; }
	}

	public class PartMonthlyReport : IDisposable
	{
		private PartDataContext dc;
		private PartDataContext dcObj;

		public string WarehouseId { get; set; }
		public string DealerCode { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }

		public static string ReportPath
		{
			get { return @"Part\Report\Monthly\"; }
		}

		public PartMonthlyReport(string wid, string dealerCode, int month, int year)
		{
			WarehouseId = wid;
			DealerCode = dealerCode;
			Month = month;
			Year = year;
		}

		public void DoReport()
		{
			//ThreadStart job = new ThreadStart(CreateExcelReport);
			Thread tr = new Thread(CreateExcelReport); tr.Start();
			Thread trObj = new Thread(CreateExcelReportForObj); trObj.Start();
		}

		void AddMonthlyReportDetailRows(Worksheet sheet, IEnumerable<MonthlyReportItem> items, string oStyle, string eStyle, string ocStyle, string ecStyle)
		{
			if (items == null) return;

			WorksheetCell cell;
			WorksheetRow row;
			int dIndex = 0;
			foreach (var item in items)
			{
				dIndex++;
				string dStyle = dIndex % 2 == 1 ? oStyle : eStyle;
				string dcStyle = dIndex % 2 == 1 ? ocStyle : ecStyle;

				row = sheet.Table.Rows.Add();
				cell = row.Cells.Add(item.PlaceType, DataType.String, dcStyle); cell.Index = 6;
				cell = row.Cells.Add(item.PlaceCode, DataType.String, dStyle);
				cell = row.Cells.Add(item.PlaceName, DataType.String, dStyle);
				cell = row.Cells.Add(item.BeginQuantity.ToString(), DataType.Number, dStyle);
				cell = row.Cells.Add(item.InQuantity.ToString(), DataType.Number, dStyle);
				cell = row.Cells.Add(item.InAmount.ToString(), DataType.Number, dStyle);
				cell = row.Cells.Add(item.OutQuantity.ToString(), DataType.Number, dStyle);
				cell = row.Cells.Add(item.OutAmount.ToString(), DataType.Number, dStyle);
				cell = row.Cells.Add(item.Balance.ToString(), DataType.Number, dStyle);
			}
		}
		void AddMonthlyReportObjDetailRows(ref int index, Worksheet sheet, IEnumerable<MonthlyReportPart> items, string oStyle, string eStyle, string ocStyle, string ecStyle)
		{
			if (items == null) return;

			WorksheetCell cell;
			WorksheetRow row;
			foreach (var item in items)
			{
				index++;
				string dStyle = index % 2 == 1 ? oStyle : eStyle;
				string dcStyle = index % 2 == 1 ? ocStyle : ecStyle;

				row = sheet.Table.Rows.Add();
				cell = row.Cells.Add(index.ToString(), DataType.Number, dcStyle); cell.Index = 4;
				cell = row.Cells.Add(item.PartType, DataType.String, dcStyle);
				cell = row.Cells.Add(item.PartCode, DataType.String, dStyle);
				cell = row.Cells.Add(item.EnglishName, DataType.String, dStyle);
				cell = row.Cells.Add(item.VietnamName, DataType.String, dStyle);
				cell = row.Cells.Add(item.BeginQuantity.ToString(), DataType.Number, dStyle);
				cell = row.Cells.Add(item.InQuantity.ToString(), DataType.Number, dStyle);
				cell = row.Cells.Add(item.InAmount.ToString(), DataType.Number, dStyle);
				cell = row.Cells.Add(item.OutQuantity.ToString(), DataType.Number, dStyle);
				cell = row.Cells.Add(item.OutAmount.ToString(), DataType.Number, dStyle);
				cell = row.Cells.Add(item.Balance.ToString(), DataType.Number, dStyle);
			}
		}

		public void CreateExcelReport()
		{
			dc = new PartDataContext();
			//Control ctrl = new Page().LoadControl("~/Controls/ExcelTemplate/MonthlyReport.ascx");
			//ListView lv = (ListView)ctrl.Controls[0];
			Warehouse w = string.IsNullOrEmpty(WarehouseId) ? null : dc.ActiveWarehouses.SingleOrDefault(wh => wh.WarehouseId == long.Parse(WarehouseId));
			Dealer d = dc.Dealers.SingleOrDefault(dl => dl.DealerCode == DealerCode);
			if (d == null) return;

			string path = Path.Combine(HttpRuntime.AppDomainAppPath, PartMonthlyReport.GetMonthlyReportFilePath(w == null ? null : w.Code, DealerCode, Month, Year));

			List<MonthlyReportPart> data = null;
			//for (int i = 0; i < 40; i++)
			{
				data = GetMonthlyReportItems(w, d, Month, Year);
			}

			Workbook book = new Workbook();
			Worksheet sheet = book.Worksheets.Add(string.Format("{0}.{1}", DealerCode, w == null ? "All" : w.Code));

			#region Setting - styles

			// Some optional properties of the Document
			book.ExcelWorkbook.ActiveSheetIndex = 1;
			book.Properties.Author = "ThangLong";
			book.Properties.Title = "Stock monthly report";
			book.Properties.Created = DateTime.Now;

			// Add some styles to the Workbook
			WorksheetStyle style = book.Styles.Add("HeaderStyle");
			//style.Font.FontName = "Tahoma";
			//style.Font.Size = 13;
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous).Weight = 1;
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous).Weight = 1;
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous).Weight = 1;
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous).Weight = 1;
			style.Font.Bold = true;
			style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
			style.Alignment.Vertical = StyleVerticalAlignment.Center;
			style.Font.Color = "White";
			style.Interior.Color = "#555555";
			style.Interior.Pattern = StyleInteriorPattern.Solid;

			style = book.Styles.Add("Group");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Vertical = StyleVerticalAlignment.Center;
			style.Interior.Color = "#CCCCCC";
			style.Interior.Pattern = StyleInteriorPattern.Solid;

			style = book.Styles.Add("GroupCenter");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Vertical = StyleVerticalAlignment.Center;
			style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
			style.Interior.Color = "#CCCCCC";
			style.Interior.Pattern = StyleInteriorPattern.Solid;

			style = book.Styles.Add("DetailOdd");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Vertical = StyleVerticalAlignment.Center;

			style = book.Styles.Add("DetailEven");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Vertical = StyleVerticalAlignment.Center;
			style.Interior.Color = "#f1f5fa";
			style.Interior.Pattern = StyleInteriorPattern.Solid;

			style = book.Styles.Add("DetailOddCenter");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
			style.Alignment.Vertical = StyleVerticalAlignment.Center;

			style = book.Styles.Add("DetailEvenCenter");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
			style.Alignment.Vertical = StyleVerticalAlignment.Center;
			style.Interior.Color = "#f1f5fa";
			style.Interior.Pattern = StyleInteriorPattern.Solid;

			// Create the Default Style to use for everyone
			style = book.Styles.Add("Default");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Vertical = StyleVerticalAlignment.Center;

			#endregion

			#region column settings

			WorksheetColumn col;
			col = new WorksheetColumn(20); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // No
			col = new WorksheetColumn(30); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // pType
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // pCode
			col = new WorksheetColumn(100); sheet.Table.Columns.Add(col); col.AutoFitWidth = false; // pEname
			col = new WorksheetColumn(100); sheet.Table.Columns.Add(col); col.AutoFitWidth = false; // pVname
			col = new WorksheetColumn(30); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // plType
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // plCode
			col = new WorksheetColumn(100); sheet.Table.Columns.Add(col); col.AutoFitWidth = false; // plName
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // Begin
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // In
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // InAmount
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // Out
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // OutAmount
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // Balance

			#endregion

			#region header rows

			WorksheetCell cell;
			WorksheetRow row = sheet.Table.Rows.Add();
			cell = row.Cells.Add("Part", DataType.String, "HeaderStyle"); cell.MergeAcross = 4;
			cell = row.Cells.Add("Report for", DataType.String, "HeaderStyle"); cell.MergeAcross = 2;
			cell = row.Cells.Add("Begin", DataType.String, "HeaderStyle"); cell.MergeDown = 1;
			cell = row.Cells.Add("In", DataType.String, "HeaderStyle"); cell.MergeAcross = 1;
			cell = row.Cells.Add("Out", DataType.String, "HeaderStyle"); cell.MergeAcross = 1;
			cell = row.Cells.Add("Balance", DataType.String, "HeaderStyle"); cell.MergeDown = 1;

			row = sheet.Table.Rows.Add();
			cell = row.Cells.Add("No", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Type", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Code", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("English name", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Vietnamese name", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Type", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Code", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Name", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Quantity", DataType.String, "HeaderStyle"); cell.Index = 10;
			cell = row.Cells.Add("Amount", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Quantity", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Amount", DataType.String, "HeaderStyle");

			#endregion

			#region bind data

			string pOddStyle = w == null ? "Group" : "DetailOdd",
				   pEvenStyle = w == null ? "Group" : "DetailOdd",
				   pOddCenterStyle = w == null ? "GroupCenter" : "DetailOddCenter",
				   EvenCenterStyle = w == null ? "GroupCenter" : "DetailOddCenter",

				   dOddStyle = "DetailOdd", dEvenStyle = "DetailEven",
				   dOddCenterStyle = "DetailOddCenter", dEvenCenterStyle = "DetailEvenCenter";

			int pIndex = 0;
			data.ForEach(part =>
			{
				pIndex++;
				bool odd = pIndex % 2 == 1;
				string pStyle = odd ? pOddStyle : pEvenStyle;
				string pcStyle = odd ? pOddCenterStyle : EvenCenterStyle;
				// group line
				row = sheet.Table.Rows.Add();
				cell = row.Cells.Add(pIndex.ToString(), DataType.Number, pcStyle);
				cell = row.Cells.Add(part.PartType, DataType.String, pcStyle);
				cell = row.Cells.Add(part.PartCode, DataType.String, pStyle);
				cell = row.Cells.Add(part.EnglishName, DataType.String, pStyle);
				cell = row.Cells.Add(part.VietnamName, DataType.String, pStyle);
				cell = row.Cells.Add(part.PlaceType, DataType.String, pcStyle);
				cell = row.Cells.Add(part.PlaceCode, DataType.String, pStyle);
				cell = row.Cells.Add(part.PlaceName, DataType.String, pStyle);
				cell = row.Cells.Add(part.BeginQuantity.ToString(), DataType.Number, pStyle);
				cell = row.Cells.Add(part.InQuantity.ToString(), DataType.Number, pStyle);
				cell = row.Cells.Add(part.InAmount.ToString(), DataType.Number, pStyle);
				cell = row.Cells.Add(part.OutQuantity.ToString(), DataType.Number, pStyle);
				cell = row.Cells.Add(part.OutAmount.ToString(), DataType.Number, pStyle);
				cell = row.Cells.Add(part.Balance.ToString(), DataType.Number, pStyle);
				// detail lines
				AddMonthlyReportDetailRows(sheet, part.ByWarehouses, dOddStyle, dEvenStyle, dOddCenterStyle, dEvenCenterStyle);
				AddMonthlyReportDetailRows(sheet, part.ByDealers, dOddStyle, dEvenStyle, dOddCenterStyle, dEvenCenterStyle);
			});

			#endregion

			dc.Dispose(); dc = null;
			// Save the file 
			book.Save(path);
		}
		public void CreateExcelReportForObj()
		{
			dcObj = new PartDataContext();
            Warehouse w = string.IsNullOrEmpty(WarehouseId) ? null : dcObj.ActiveWarehouses.SingleOrDefault(wh => wh.WarehouseId == long.Parse(WarehouseId));
			Dealer d = dcObj.Dealers.SingleOrDefault(dl => dl.DealerCode == DealerCode);
			if (d == null) return;

			string path = Path.Combine(HttpRuntime.AppDomainAppPath, PartMonthlyReport.GetMonthlyReportObjFilePath(w == null ? null : w.Code, DealerCode, Month, Year));

			List<MonthlyReportObject> data = null;
			//for (int i = 0; i < 40; i++)
			{
				data = GetMonthlyReportObjects(w, d, Month, Year);
			}

			Workbook book = new Workbook();
			Worksheet sheet = book.Worksheets.Add(string.Format("{0}.{1}", DealerCode, w == null ? "All" : w.Code));

			#region Setting - styles

			// Some optional properties of the Document
			book.ExcelWorkbook.ActiveSheetIndex = 1;
			book.Properties.Author = "ThangLong";
			book.Properties.Title = "Stock monthly report by components";
			book.Properties.Created = DateTime.Now;

			// Add some styles to the Workbook
			WorksheetStyle style = book.Styles.Add("HeaderStyle");
			//style.Font.FontName = "Tahoma";
			//style.Font.Size = 13;
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous).Weight = 1;
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous).Weight = 1;
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous).Weight = 1;
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous).Weight = 1;
			style.Font.Bold = true;
			style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
			style.Alignment.Vertical = StyleVerticalAlignment.Center;
			style.Font.Color = "White";
			style.Interior.Color = "#555555";
			style.Interior.Pattern = StyleInteriorPattern.Solid;

			style = book.Styles.Add("Group");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Vertical = StyleVerticalAlignment.Center;
			style.Interior.Color = "#CCCCCC";
			style.Interior.Pattern = StyleInteriorPattern.Solid;

			style = book.Styles.Add("GroupCenter");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Vertical = StyleVerticalAlignment.Center;
			style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
			style.Interior.Color = "#CCCCCC";
			style.Interior.Pattern = StyleInteriorPattern.Solid;

			style = book.Styles.Add("DetailOdd");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Vertical = StyleVerticalAlignment.Center;

			style = book.Styles.Add("DetailEven");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Vertical = StyleVerticalAlignment.Center;
			style.Interior.Color = "#f1f5fa";
			style.Interior.Pattern = StyleInteriorPattern.Solid;

			style = book.Styles.Add("DetailOddCenter");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
			style.Alignment.Vertical = StyleVerticalAlignment.Center;

			style = book.Styles.Add("DetailEvenCenter");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
			style.Alignment.Vertical = StyleVerticalAlignment.Center;
			style.Interior.Color = "#f1f5fa";
			style.Interior.Pattern = StyleInteriorPattern.Solid;

			// Create the Default Style to use for everyone
			style = book.Styles.Add("Default");
			style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous);
			style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous);
			style.Alignment.Vertical = StyleVerticalAlignment.Center;

			#endregion

			#region column settings

			WorksheetColumn col;
			col = new WorksheetColumn(30); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // plType
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // plCode
			col = new WorksheetColumn(100); sheet.Table.Columns.Add(col); col.AutoFitWidth = false; // plName 
			col = new WorksheetColumn(20); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // No
			col = new WorksheetColumn(30); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // pType
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // pCode
			col = new WorksheetColumn(100); sheet.Table.Columns.Add(col); col.AutoFitWidth = false; // pEname
			col = new WorksheetColumn(100); sheet.Table.Columns.Add(col); col.AutoFitWidth = false; // pVname

			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // Begin
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // In
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // InAmount
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // Out
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // OutAmount
			col = new WorksheetColumn(); sheet.Table.Columns.Add(col); col.AutoFitWidth = true;  // Balance

			#endregion

			#region header rows

			WorksheetCell cell;
			WorksheetRow row = sheet.Table.Rows.Add();
			cell = row.Cells.Add("Report for", DataType.String, "HeaderStyle"); cell.MergeAcross = 2;
			cell = row.Cells.Add("Part", DataType.String, "HeaderStyle"); cell.MergeAcross = 4;
			cell = row.Cells.Add("Begin", DataType.String, "HeaderStyle"); cell.MergeDown = 1;
			cell = row.Cells.Add("In", DataType.String, "HeaderStyle"); cell.MergeAcross = 1;
			cell = row.Cells.Add("Out", DataType.String, "HeaderStyle"); cell.MergeAcross = 1;
			cell = row.Cells.Add("Balance", DataType.String, "HeaderStyle"); cell.MergeDown = 1;

			row = sheet.Table.Rows.Add();
			cell = row.Cells.Add("Type", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Code", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Name", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("No", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Type", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Code", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("English name", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Vietnamese name", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Quantity", DataType.String, "HeaderStyle"); cell.Index = 10;
			cell = row.Cells.Add("Amount", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Quantity", DataType.String, "HeaderStyle");
			cell = row.Cells.Add("Amount", DataType.String, "HeaderStyle");

			#endregion

			#region bind data

			string pOddStyle = w == null ? "Group" : "DetailOdd",
				   pEvenStyle = w == null ? "Group" : "DetailOdd",
				   pOddCenterStyle = w == null ? "GroupCenter" : "DetailOddCenter",
				   EvenCenterStyle = w == null ? "GroupCenter" : "DetailOddCenter",

				   dOddStyle = "DetailOdd", dEvenStyle = "DetailEven",
				   dOddCenterStyle = "DetailOddCenter", dEvenCenterStyle = "DetailEvenCenter";

			int pIndex = 0;
			data.ForEach(part =>
			{
				pIndex++;
				bool odd = pIndex % 2 == 1;
				string pStyle = odd ? pOddStyle : pEvenStyle;
				string pcStyle = odd ? pOddCenterStyle : EvenCenterStyle;
				// group line
				row = sheet.Table.Rows.Add();
				cell = row.Cells.Add(part.PlaceType, DataType.String, pcStyle);
				cell = row.Cells.Add(part.PlaceCode, DataType.String, pStyle);
				cell = row.Cells.Add(part.PlaceName, DataType.String, pStyle);

				cell = row.Cells.Add("", DataType.String, pcStyle);
				cell = row.Cells.Add("", DataType.String, pcStyle);
				cell = row.Cells.Add("", DataType.String, pStyle);
				cell = row.Cells.Add("", DataType.String, pStyle);
				cell = row.Cells.Add("", DataType.String, pStyle);

				cell = row.Cells.Add(part.BeginQuantity.ToString(), DataType.Number, pStyle); //cell.Index = 9;
				cell = row.Cells.Add(part.InQuantity.ToString(), DataType.Number, pStyle);
				cell = row.Cells.Add(part.InAmount.ToString(), DataType.Number, pStyle);
				cell = row.Cells.Add(part.OutQuantity.ToString(), DataType.Number, pStyle);
				cell = row.Cells.Add(part.OutAmount.ToString(), DataType.Number, pStyle);
				cell = row.Cells.Add(part.Balance.ToString(), DataType.Number, pStyle);
				// detail lines
				int index = 0;
				AddMonthlyReportObjDetailRows(ref index, sheet, part.PartList, dOddStyle, dEvenStyle, dOddCenterStyle, dEvenCenterStyle);
				AddMonthlyReportObjDetailRows(ref index, sheet, part.AccList, dOddStyle, dEvenStyle, dOddCenterStyle, dEvenCenterStyle);
			});

			#endregion

			dcObj.Dispose(); dcObj = null;
			// Save the file 
			book.Save(path);
		}
        public static string CleanCode(string code)
        {
            return code.Replace('/', '-').Replace('\\', '-');
        }
		public static string GetMonthlyReportFileName(string whCode, string dealerCode, int month, int year)
		{
            string wh = whCode == null ? "All" : CleanCode(whCode);
			string fileName = string.Format("Stock.report.{0}.{1}.{2}.{3}.xml", CleanCode(dealerCode), wh, year.ToString().PadLeft(4, '0'), month.ToString().PadLeft(2, '0'));
			return fileName;
		}
		public static string GetMonthlyReportObjFileName(string whCode, string dealerCode, int month, int year)
		{
			string wh = whCode == null ? "All" : CleanCode(whCode);
			string fileName = string.Format("Stock.report2.{0}.{1}.{2}.{3}.xml", CleanCode(dealerCode), wh, year.ToString().PadLeft(4, '0'), month.ToString().PadLeft(2, '0'));
			return fileName;
		}

		public static string GetMonthlyReportFilePath(string whCode, string dealerCode, int month, int year)
		{
			string filePath = string.Format("{0}{1}", PartMonthlyReport.ReportPath, PartMonthlyReport.GetMonthlyReportFileName(whCode, dealerCode, month, year));
			return filePath;
		}
		public static string GetMonthlyReportObjFilePath(string whCode, string dealerCode, int month, int year)
		{
			string filePath = string.Format("{0}{1}", PartMonthlyReport.ReportPath, PartMonthlyReport.GetMonthlyReportObjFileName(whCode, dealerCode, month, year));
			return filePath;
		}

		public static string GetMonthlyReportFileSearch(string whCode, string dealerCode)
		{
			string wh = whCode == null ? "All" : CleanCode(whCode);
			string fileName = string.Format("Stock.report.{0}.{1}.????.??.xml", CleanCode(dealerCode), wh);
			return fileName;
		}
		public static string GetMonthlyReportObjFileSearch(string whCode, string dealerCode)
		{
			string wh = whCode == null ? "All" : CleanCode(whCode);
			string fileName = string.Format("Stock.report2.{0}.{1}.????.??.xml", CleanCode(dealerCode), wh);
			return fileName;
		}

		public List<MonthlyReportPart> GetMonthlyReportItems(Warehouse wh, Dealer dealer, int month, int year)
		{
			string dbCode;
			//IQueryable<PartInfo> pis;
			IQueryable<MonthlyReportPartInfo> pisP;
			IQueryable<MonthlyReportPartInfo> pisA;
			List<MonthlyReportPart> res = new List<MonthlyReportPart>();

			int bgMonth = month - 1, bgYear = year;
			if (bgMonth == 0) { bgMonth = 12; bgYear--; }
			// for transaction histories
			DateTime dtTHBegin = new DateTime(year, month, 1);
			DateTime dtTHEnd = dtTHBegin.AddMonths(1);

			#region get parts list

			if (wh == null)
			{
				if (dealer == null) throw new Exception("Invalid dealer!");

				//pis = dc.PartInfos.Where(pi => dealers.Contains(pi.DealerCode) || pi.DealerCode == dealer.DealerCode);
				pisP = dc.PartInfos.Where(pi => pi.PartType == PartType.Part)
								   .Where(pi => pi.Dealer.ParentCode == dealer.DealerCode || pi.DealerCode == dealer.DealerCode)
								   .GroupBy(pi => pi.PartCode).Select(g => new MonthlyReportPartInfo { PartCode = g.Key });

				pisA = dc.PartInfos.Where(pi => pi.PartType == PartType.Accessory)
								   .Where(pi => pi.Dealer.ParentCode == dealer.DealerCode || pi.DealerCode == dealer.DealerCode)
								   .Select(g => new MonthlyReportPartInfo { PartCode = g.PartCode, EnglishName = g.Accessory.EnglishName, VietnamName = g.Accessory.VietnamName });
				dbCode = dealer.DatabaseCode;
			}
			else
			{
				//pis = dc.PartInfos.Where(pi => pi.PartSafeties.Where(ps => ps.WarehouseId == wh.WarehouseId).Count() > 0);
				pisP = dc.PartInfos.Where(pi => pi.PartType == PartType.Part)
								   .Where(pi => pi.PartSafeties.Count(ps => ps.WarehouseId == wh.WarehouseId) > 0)
								   .GroupBy(pi => pi.PartCode).Select(g => new MonthlyReportPartInfo { PartCode = g.Key });
				pisA = dc.PartInfos.Where(pi => pi.PartType == PartType.Accessory)
								   .Where(pi => pi.PartSafeties.Where(ps => ps.WarehouseId == wh.WarehouseId).Count() > 0)
								   .Select(g => new MonthlyReportPartInfo { PartCode = g.PartCode, EnglishName = g.Accessory.EnglishName, VietnamName = g.Accessory.VietnamName });
				dbCode = wh.Dealer.DatabaseCode;
			}

			#endregion

			//var pisP = pis.Where(pi => pi.PartType == PartType.Part);
			//var pisA = pis.Where(pi => pi.PartType == PartType.Accessory);

			if (wh == null)
			{
				#region Part
				res = pisP.Join(dc.Parts.Where(p => p.DatabaseCode == dbCode), pi => pi.PartCode, p => p.PartCode,
					(pi, p) => new MonthlyReportPart
					{
						PartCode = pi.PartCode,
						PartType = PartType.Part,
						EnglishName = p.EnglishName,
						VietnamName = p.VietnamName,
						// warehouses
                        ByWarehouses = dc.Warehouses.Where(w => (w.Status != WarehouseStatus.Deleted || w.Status == null) && w.DealerCode == dealer.DealerCode && w.Type == WarehouseType.Part
											&& w.PartSafeties.Where(ps => ps.PartInfo.PartCode == pi.PartCode).Count() > 0).Select(pl => new MonthlyReportItem
											{
												PlaceCode = pl.Code,
												PlaceName = pl.Address,
												PlaceType = "W",
                                                BeginQuantity = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfo.PartCode == pi.PartCode && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == pl.WarehouseId).Quantity,
                                                Balance = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfo.PartCode == pi.PartCode && iv.Month == month && iv.Year == year && iv.WarehouseId == pl.WarehouseId).Quantity,
                                                InQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == pl.WarehouseId).Sum(th => th.Quantity),
                                                InAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == pl.WarehouseId).Sum(th => th.ActualCost),
                                                OutQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == pl.WarehouseId).Sum(th => th.Quantity),
                                                OutAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == pl.WarehouseId).Sum(th => th.ActualCost),
											}),
						// sub dealers
						ByDealers = dc.Dealers.Where(d => d.ParentCode == dealer.DealerCode
											&& d.PartInfos.Where(dp => dp.PartCode == pi.PartCode).Count() > 0
											).Select(pl => new MonthlyReportItem
						{
							PlaceCode = pl.DealerCode,
							PlaceName = pl.DealerName,
							PlaceType = "D",
                            BeginQuantity = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfo.PartCode == pi.PartCode && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == null && iv.DealerCode == pl.DealerCode).Quantity,
                            Balance = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfo.PartCode == pi.PartCode && iv.Month == month && iv.Year == year && iv.WarehouseId == null && iv.DealerCode == pl.DealerCode).Quantity,
                            InQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.DealerCode == pl.DealerCode).Sum(th => th.Quantity),
                            InAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.DealerCode == pl.DealerCode).Sum(th => th.ActualCost),
                            OutQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.DealerCode == pl.DealerCode).Sum(th => th.Quantity),
                            OutAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.DealerCode == pl.DealerCode).Sum(th => th.ActualCost),
						}),
					}).ToList<MonthlyReportPart>();

				#endregion

				#region Accsessory
				res.AddRange(pisA.Select(pi => new MonthlyReportPart
				{
					PartCode = pi.PartCode,
					PartType = PartType.Accessory,
					EnglishName = pi.EnglishName,
					VietnamName = pi.VietnamName,
                    ByWarehouses = dc.Warehouses.Where(w => (w.Status != WarehouseStatus.Deleted || w.Status == null) && w.DealerCode == dealer.DealerCode && w.Type == WarehouseType.Part
											&& w.PartSafeties.Where(ps => ps.PartInfo.PartCode == pi.PartCode).Count() > 0
											).Select(pl => new MonthlyReportItem
											{
												PlaceCode = pl.Code,
												PlaceName = pl.Address,
												PlaceType = "W",
                                                BeginQuantity = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfo.PartCode == pi.PartCode && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == pl.WarehouseId).Quantity,
                                                Balance = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfo.PartCode == pi.PartCode && iv.Month == month && iv.Year == year && iv.WarehouseId == pl.WarehouseId).Quantity,
                                                InQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == pl.WarehouseId).Sum(th => th.Quantity),
                                                InAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == pl.WarehouseId).Sum(th => th.ActualCost),
                                                OutQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == pl.WarehouseId).Sum(th => th.Quantity),
                                                OutAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == pl.WarehouseId).Sum(th => th.ActualCost),
											}),
					ByDealers = dc.Dealers.Where(d => d.ParentCode == dealer.DealerCode
											&& d.PartInfos.Where(dp => dp.PartCode == pi.PartCode).Count() > 0
											).Select(pl => new MonthlyReportItem
											{
												PlaceCode = pl.DealerCode,
												PlaceName = pl.DealerName,
												PlaceType = "D",
                                                BeginQuantity = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfo.PartCode == pi.PartCode && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == null && iv.DealerCode == pl.DealerCode).Quantity,
                                                Balance = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfo.PartCode == pi.PartCode && iv.Month == month && iv.Year == year && iv.WarehouseId == null && iv.DealerCode == pl.DealerCode).Quantity,
                                                InQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.DealerCode == pl.DealerCode).Sum(th => th.Quantity),
                                                InAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.DealerCode == pl.DealerCode).Sum(th => th.ActualCost),
                                                OutQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.DealerCode == pl.DealerCode).Sum(th => th.Quantity),
                                                OutAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.DealerCode == pl.DealerCode).Sum(th => th.ActualCost),
											}),
				}).ToList<MonthlyReportPart>()

				);

				#endregion
			}
			else
			{
				#region Part
				res = pisP.Join(dc.Parts.Where(p => p.DatabaseCode == dbCode), pi => pi.PartCode, p => p.PartCode,
					(pi, p) => new MonthlyReportPart
					{
						PartCode = pi.PartCode,
						PartType = PartType.Part,
						EnglishName = p.EnglishName,
						VietnamName = p.VietnamName,
						BeginQuantity = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfo.PartCode == pi.PartCode && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == wh.WarehouseId).Quantity,
                        Balance = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfo.PartCode == pi.PartCode && iv.Month == month && iv.Year == year && iv.WarehouseId == wh.WarehouseId).Quantity,
                        InQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.Quantity),
                        InAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.ActualCost),
                        OutQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.Quantity),
                        OutAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.ActualCost),
					}).ToList<MonthlyReportPart>();

				#endregion

				#region Accsessory
				res.AddRange(pisA.Select(pi => new MonthlyReportPart
				{
					PartCode = pi.PartCode,
					PartType = PartType.Accessory,
					EnglishName = pi.EnglishName,
					VietnamName = pi.VietnamName,
                    BeginQuantity = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfo.PartCode == pi.PartCode && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == wh.WarehouseId).Quantity,
                    Balance = dc.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfo.PartCode == pi.PartCode && iv.Month == month && iv.Year == year && iv.WarehouseId == wh.WarehouseId).Quantity,
                    InQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.Quantity),
                    InAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.ActualCost),
                    OutQuantity = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.Quantity),
                    OutAmount = dc.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfo.PartCode == pi.PartCode && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.ActualCost),
				}).ToList<MonthlyReportPart>()

				);

				#endregion
			}

			// summary part info
			if (wh == null)
			{
				res.ForEach(item =>
				{
					item.BeginQuantity = item.ByWarehouses.Sum(i => i.BeginQuantity) + item.ByDealers.Sum(i => i.BeginQuantity);
					item.InAmount = item.ByWarehouses.Sum(i => i.InAmount) + item.ByDealers.Sum(i => i.InAmount);
					item.InQuantity = item.ByWarehouses.Sum(i => i.InQuantity) + item.ByDealers.Sum(i => i.InQuantity);
					item.OutAmount = item.ByWarehouses.Sum(i => i.OutAmount) + item.ByDealers.Sum(i => i.OutAmount);
					item.OutQuantity = item.ByWarehouses.Sum(i => i.OutQuantity) + item.ByDealers.Sum(i => i.OutQuantity);
					item.Balance = item.ByWarehouses.Sum(i => i.Balance) + item.ByDealers.Sum(i => i.Balance);
				});
			}
			return res;
		}
		public List<MonthlyReportObject> GetMonthlyReportObjects(Warehouse wh, Dealer dealer, int month, int year)
		{
			List<MonthlyReportObject> res = new List<MonthlyReportObject>();

			string dbCode;
			int bgMonth = month - 1, bgYear = year;
			if (bgMonth == 0) { bgMonth = 12; bgYear--; }
			// for transaction histories
			DateTime dtTHBegin = new DateTime(year, month, 1);
			DateTime dtTHEnd = dtTHBegin.AddMonths(1);

			if (wh == null)
			{
				dbCode = dealer.DatabaseCode;
                res.AddRange(dealer.ActiveWarehouses.Where(w => w.Type == WarehouseType.Part).Select(w => new MonthlyReportObject
				{
					PlaceCode = w.Code,
					PlaceName = w.Address,
					PlaceType = "W",
					PartList = dcObj.PartInfos.Where(pi => pi.PartSafeties.Where(ps => ps.WarehouseId == w.WarehouseId).Count() > 0 && pi.PartType == PartType.Part)
							.Join(dcObj.Parts.Where(p => p.DatabaseCode == dbCode), pi => pi.PartCode, p => p.PartCode,
							(pi, p) => new MonthlyReportPart
							//.Select(ps => new MonthlyReportPart
							{
								PartCode = pi.PartCode,
								PartType = PartType.Part,
								EnglishName = p.EnglishName,
								VietnamName = p.VietnamName,
                                BeginQuantity = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfoId == pi.PartInfoId && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == w.WarehouseId).Quantity,
                                Balance = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfoId == pi.PartInfoId && iv.Month == month && iv.Year == year && iv.WarehouseId == w.WarehouseId).Quantity,
                                InQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == w.WarehouseId).Sum(th => th.Quantity),
                                InAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == w.WarehouseId).Sum(th => th.ActualCost),
                                OutQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == w.WarehouseId).Sum(th => th.Quantity),
                                OutAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == w.WarehouseId).Sum(th => th.ActualCost),
							}),
					AccList = dcObj.PartInfos.Where(pi => pi.PartSafeties.Where(ps => ps.WarehouseId == w.WarehouseId).Count() > 0 && pi.PartType == PartType.Accessory)
							.Select(pi => new MonthlyReportPart
							{
								PartCode = pi.PartCode,
								PartType = PartType.Accessory,
								EnglishName = pi.Accessory.EnglishName,
								VietnamName = pi.Accessory.VietnamName,
                                BeginQuantity = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfoId == pi.PartInfoId && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == w.WarehouseId).Quantity,
                                Balance = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfoId == pi.PartInfoId && iv.Month == month && iv.Year == year && iv.WarehouseId == w.WarehouseId).Quantity,
                                InQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == w.WarehouseId).Sum(th => th.Quantity),
                                InAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == w.WarehouseId).Sum(th => th.ActualCost),
                                OutQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == w.WarehouseId).Sum(th => th.Quantity),
                                OutAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == w.WarehouseId).Sum(th => th.ActualCost),
							}),
				}).ToList());

				res.AddRange(dealer.Dealers.Select(d => new MonthlyReportObject
				{
					PlaceCode = d.DealerCode,
					PlaceName = d.DealerName,
					PlaceType = "D",
					PartList = dcObj.PartInfos.Where(pi => pi.DealerCode == d.DealerCode && pi.PartSafeties.Count > 0 && pi.PartType == PartType.Part)
								 .Join(dcObj.Parts.Where(p => p.DatabaseCode == dbCode), pi => pi.PartCode, p => p.PartCode,
									(pi, p) => new MonthlyReportPart
									 {
										 PartCode = pi.PartCode,
										 PartType = PartType.Part,
										 EnglishName = p.EnglishName,
										 VietnamName = p.VietnamName,
                                         BeginQuantity = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfoId == pi.PartInfoId && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == null && iv.DealerCode == d.DealerCode).Quantity,
                                         Balance = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfoId == pi.PartInfoId && iv.Month == month && iv.Year == year && iv.WarehouseId == null && iv.DealerCode == d.DealerCode).Quantity,
                                         InQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.DealerCode == d.DealerCode).Sum(th => th.Quantity),
                                         InAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.DealerCode == d.DealerCode).Sum(th => th.ActualCost),
                                         OutQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.DealerCode == d.DealerCode).Sum(th => th.Quantity),
                                         OutAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.DealerCode == d.DealerCode).Sum(th => th.ActualCost),
									 }),
					AccList = dcObj.PartInfos.Where(pi => pi.DealerCode == d.DealerCode && pi.PartSafeties.Count > 0 && pi.PartType == PartType.Accessory)
						   .Select(pi => new MonthlyReportPart
						   {
							   PartCode = pi.PartCode,
							   PartType = PartType.Accessory,
							   EnglishName = pi.Accessory.EnglishName,
							   VietnamName = pi.Accessory.VietnamName,
                               BeginQuantity = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfoId == pi.PartInfoId && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == null && iv.DealerCode == d.DealerCode).Quantity,
                               Balance = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfoId == pi.PartInfoId && iv.Month == month && iv.Year == year && iv.WarehouseId == null && iv.DealerCode == d.DealerCode).Quantity,
                               InQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.DealerCode == d.DealerCode).Sum(th => th.Quantity),
                               InAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.DealerCode == d.DealerCode).Sum(th => th.ActualCost),
                               OutQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.DealerCode == d.DealerCode).Sum(th => th.Quantity),
                               OutAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.DealerCode == d.DealerCode).Sum(th => th.ActualCost),
						   }),
				}).ToList());
			}
			else
			{
				dbCode = wh.Dealer.DatabaseCode;
				res.Add(
					new MonthlyReportObject
					{
						PlaceCode = wh.Code,
						PlaceName = wh.Address,
						PlaceType = "W",
						PartList = dcObj.PartInfos.Where(pi => pi.PartSafeties.Where(ps => ps.WarehouseId == wh.WarehouseId).Count() > 0 && pi.PartType == PartType.Part)
								.Join(dcObj.Parts.Where(p => p.DatabaseCode == dbCode), pi => pi.PartCode, p => p.PartCode,
								(pi, p) => new MonthlyReportPart
								//.Select(ps => new MonthlyReportPart
								{
									PartCode = pi.PartCode,
									PartType = PartType.Part,
									EnglishName = p.EnglishName,
									VietnamName = p.VietnamName,
                                    BeginQuantity = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfoId == pi.PartInfoId && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == wh.WarehouseId).Quantity,
                                    Balance = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Part && iv.PartInfoId == pi.PartInfoId && iv.Month == month && iv.Year == year && iv.WarehouseId == wh.WarehouseId).Quantity,
                                    InQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.Quantity),
                                    InAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.ActualCost),
                                    OutQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.Quantity),
                                    OutAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Part && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.ActualCost),
								}),
						AccList = dcObj.PartInfos.Where(pi => pi.PartSafeties.Where(ps => ps.WarehouseId == wh.WarehouseId).Count() > 0 && pi.PartType == PartType.Accessory)
								.Select(pi => new MonthlyReportPart
								{
									PartCode = pi.PartCode,
									PartType = PartType.Accessory,
									EnglishName = pi.Accessory.EnglishName,
									VietnamName = pi.Accessory.VietnamName,
                                    BeginQuantity = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfoId == pi.PartInfoId && iv.Month == bgMonth && iv.Year == bgYear && iv.WarehouseId == wh.WarehouseId).Quantity,
                                    Balance = dcObj.Inventories.SingleOrDefault(iv => iv.PartInfo.PartType == PartType.Accessory && iv.PartInfoId == pi.PartInfoId && iv.Month == month && iv.Year == year && iv.WarehouseId == wh.WarehouseId).Quantity,
                                    InQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.Quantity),
                                    InAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity > 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.ActualCost),
                                    OutQuantity = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.Quantity),
                                    OutAmount = dcObj.TransactionHistories.Where(th => th.PartInfo.PartType == PartType.Accessory && th.PartInfoId == pi.PartInfoId && th.TransactionDate >= dtTHBegin && th.TransactionDate < dtTHEnd && th.Quantity < 0 && th.WarehouseId == wh.WarehouseId).Sum(th => th.ActualCost),
								}),
					}
				);
			}

			res.ForEach(obj =>
				{
					obj.BeginQuantity = obj.PartList.Sum(p => p.BeginQuantity) + obj.AccList.Sum(a => a.BeginQuantity);
					obj.InQuantity = obj.PartList.Sum(p => p.InQuantity) + obj.AccList.Sum(a => a.InQuantity);
					obj.InAmount = obj.PartList.Sum(p => p.InAmount) + obj.AccList.Sum(a => a.InAmount);
					obj.OutQuantity = obj.PartList.Sum(p => p.OutQuantity) + obj.AccList.Sum(a => a.OutQuantity);
					obj.OutAmount = obj.PartList.Sum(p => p.OutAmount) + obj.AccList.Sum(a => a.OutAmount);
					obj.Balance = obj.PartList.Sum(p => p.Balance) + obj.AccList.Sum(a => a.Balance);
				});

			return res;
		}

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			if (dc != null) dc.Dispose(); dc = null;
			if (dcObj != null) dcObj.Dispose(); dcObj = null;
		}

		#endregion
	}
}
