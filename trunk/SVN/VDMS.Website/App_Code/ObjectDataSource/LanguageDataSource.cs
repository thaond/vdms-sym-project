using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web;

namespace VDMS.BLL.ObjectDataSource
{
	public class LanguageDataSource
	{
		private int count = 0;

		public int SelectAllLangCount()
		{
			return count;
		}

		public IList<CultureInfo> SelectAllLang()
		{
			List<CultureInfo> list = new List<CultureInfo>();
			int langCount;
			int.TryParse(ConfigurationManager.AppSettings["LanguageCount"], out langCount);
			for (int i = 1; i <= langCount; i++)
			{
				try
				{
					string name = ConfigurationManager.AppSettings[string.Format("Lang{0}", i)];
					if (!string.IsNullOrEmpty(name)) list.Add(new CultureInfo(name));
				}
				catch { }
			}

			count = list.Count;
			HttpContext.Current.Items["LangRowCount"] = count;
			return list;
		}
	}
}
