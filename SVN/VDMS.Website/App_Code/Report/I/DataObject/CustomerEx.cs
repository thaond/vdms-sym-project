using System;
using VDMS.Core.Domain;

/// <summary>
/// Summary description for CustomerEx
/// </summary>
namespace VDMS.I.Report.DataObject
{
	[Serializable]
	public class CustomerEx : Exchangepartdetail
	{
		public string F1 { get; set; }
		public string A1 { get; set; }
		public string F2 { get; set; }
		public string A2 { get; set; }

		public CustomerEx()
		{
		}

		public CustomerEx(string f1, string a1, string f2, string a2)
		{
			F1 = f1;
			A1 = a1;
			F2 = f2;
			A2 = a2;
		}
	}

}