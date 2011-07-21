using System.Collections.Generic;
using System.Data;

/// <summary>
/// Summary description for AreaHelper
/// </summary>
namespace VDMS.Helper
{
	public class AreaHelper
	{
		public static string GetAreaNameByAreaCode(string AreaCode)
		{
			string res = AreaCode;
			if (UserHelper.DatabaseCode == "DNF" && DNFCodeToName.ContainsKey(AreaCode)) res = DNFCodeToName[AreaCode];
			if (UserHelper.DatabaseCode == "HTF" && HTFCodeToName.ContainsKey(AreaCode)) res = HTFCodeToName[AreaCode];
			return res;
		}
		static Dictionary<string, string> HTFCodeToName = new Dictionary<string, string>();
		static Dictionary<string, string> DNFCodeToName = new Dictionary<string, string>();
		static List<string> HTFArea = new List<string>();
		static List<string> DNFArea = new List<string>();

		static AreaHelper()
		{
			Init();
		}

		public static List<string> Area
		{
			get
			{
				if (UserHelper.DatabaseCode == "DNF") return DNFArea;
				return HTFArea;
			}
		}

		public static void Init()
		{
			lock (HTFArea)
			{
				HTFArea.Clear();
				DNFArea.Clear();
				HTFCodeToName.Clear();
				DNFCodeToName.Clear();

				DataSet ds = VDMS.Data.TipTop.Area.GetListArea("HTF");
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					HTFArea.Add((string)row[0]);
					HTFCodeToName.Add(row["AreaCode"] as string, row["AreaName"] as string);
				}

				ds = VDMS.Data.TipTop.Area.GetListArea("DNF");
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					DNFArea.Add((string)row[0]);
					DNFCodeToName.Add(row["AreaCode"] as string, row["AreaName"] as string);
				}
			}
		}
	}
}