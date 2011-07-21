using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace VDMS.II.Report
{
	public class PartMonthlyReportFile
	{
		public string FilePathName { get; set; }
		public string FilePathName2 { get; set; }
		public int Moth { get; set; }
		public int Year { get; set; }
	}

	public class PartMonthlyReportFileDAO
	{
		int reportFileCount;
		public int CountReportFiles(string wCode, string dealerCode)
		{
			return reportFileCount;
		}
		public List<PartMonthlyReportFile> GetReportFiles(string wCode, string dealerCode, int maximumRows, int startRowIndex)
		{
			List<PartMonthlyReportFile> byPart = GetReportFiles(PartMonthlyReport.GetMonthlyReportFileSearch(wCode, dealerCode), wCode, dealerCode, maximumRows, startRowIndex);
			List<PartMonthlyReportFile> byComp = GetReportFiles(PartMonthlyReport.GetMonthlyReportObjFileSearch(wCode, dealerCode), wCode, dealerCode, maximumRows, startRowIndex);

			byComp.ForEach(f =>
			{
				PartMonthlyReportFile item = byPart.SingleOrDefault(fp => fp.Moth == f.Moth && fp.Year == f.Year);
				if (item == null)
				{
					byPart.Add(new PartMonthlyReportFile { Year = f.Year, Moth = f.Moth, FilePathName2 = f.FilePathName });
				}
				else
				{
					item.FilePathName2 = f.FilePathName;
				}
			});

			return byPart;
		}

		public List<PartMonthlyReportFile> GetReportFiles(string searchStr, string wCode, string dealerCode, int maximumRows, int startRowIndex)
		{
			List<PartMonthlyReportFile> res = new List<PartMonthlyReportFile>();
			string path = Path.Combine(HttpRuntime.AppDomainAppPath, PartMonthlyReport.ReportPath);

			string[] files = Directory.GetFiles(path, searchStr);
			foreach (string f in files)
			{
				try
				{
					res.Add(new PartMonthlyReportFile { FilePathName = "Monthly/" + f.Substring(f.LastIndexOf('\\') + 1), Moth = int.Parse(f.Substring(f.Length - 6, 2)), Year = int.Parse(f.Substring(f.Length - 11, 4)) });
				}
				catch { }
			}
			reportFileCount = res.Count;
			return res.OrderBy(f => f.Year).OrderBy(f => f.Moth).Skip(startRowIndex).Take(maximumRows).ToList();
		}
	}
}