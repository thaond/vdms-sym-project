using System;
using System.Linq;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using VDMS.Common.Utils;
using VDMS.Data.DAL2;
using VDMS.Data.TipTop;
using VDMS.Helper;

namespace VDMS.I.Report
{
	public class ReportHelper
	{
		public static DataSet BuildReportDaily(DateTime FromDate, DateTime ToDate, string DealerCode)
		{
			DataSet result = null;
            foreach (var w in VDMS.II.BasicData.DealerDAO.GetDealerByCode(DealerCode).ActiveWarehouses.Where(wh=>wh.Type == VDMS.II.Entity.WarehouseType.Vehicle))
			{
				DataSet ds = InventoryDao.ReportDaily(FromDate, ToDate, DealerCode, w.Code, w.Address);
				if (result == null) result = ds;
				else
				{
					foreach (DataRow row1 in ds.Tables[0].Rows)
						result.Tables[0].ImportRow(row1);
				}
			}
			return result;
		}

		public static ReportDocument BuildReportReceiptDetail(string CrystalReportURL, DateTime fromdate, DateTime todate, string DealerCode, string BranchCode, string BranchName)
		{
			ReportDocument rdReceiptDetail = ReportFactory.GetReport();
			rdReceiptDetail.Load(CrystalReportURL);
			rdReceiptDetail.SetDataSource(InventoryDao.ReportSellingDailyDebtOnly(fromdate, todate, DealerCode, BranchCode, UserHelper.DatabaseCode));
			String TitleReport = Resources.Constants.Store + " ";
			TitleReport += BranchName + ". " + Resources.Constants.FromDate + " " + fromdate.ToShortDateString() + " " + Resources.Constants.ToDate + " " + todate.ToShortDateString();
			rdReceiptDetail.SetParameterValue("Title", TitleReport);
			return rdReceiptDetail;
		}

		public static ReportDocument BuildReportSellingDaily(string CrystalReportURL, DateTime fromdate, DateTime todate, string DealerCode, string BranchCode, string BranchName)
		{
			ReportDocument rdSellingDaily = ReportFactory.GetReport();
			rdSellingDaily.Load(CrystalReportURL);
			rdSellingDaily.SetDataSource(InventoryDao.ReportSellingDaily(fromdate, todate, DealerCode, BranchCode, UserHelper.DatabaseCode));
			String TitleReport = Resources.Constants.Store + " ";
			TitleReport += BranchName + ". " + Resources.Constants.FromDate + " " + fromdate.ToShortDateString() + " " + Resources.Constants.ToDate + " " + todate.ToShortDateString();
			rdSellingDaily.SetParameterValue("Title", TitleReport);
			return rdSellingDaily;
		}

		public static ReportDocument BuildReportDocumentForAgency(DataTable lstForAgency, string CrystalReportURL, DateTime fromdate, DateTime todate)
		{
			ReportDocument rdForAgency = ReportFactory.GetReport();
			rdForAgency.Load(CrystalReportURL);
			rdForAgency.SetDataSource(lstForAgency);
			// Set a value to the parameter.
			rdForAgency.SetParameterValue("FromDate", fromdate.ToShortDateString());
			rdForAgency.SetParameterValue("ToDate", todate.ToShortDateString());
			return rdForAgency;
		}
	}
}