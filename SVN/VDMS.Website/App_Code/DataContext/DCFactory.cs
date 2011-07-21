using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.II.Linq;

namespace VDMS.II.Linq
{
	public class DCFactory
	{
		static Dictionary<string, Devart.Data.Linq.DataContext> staticDC = new Dictionary<string, Devart.Data.Linq.DataContext>();

		public static T GetDataContext<T>() where T : Devart.Data.Linq.DataContext, new()
		{
			HttpContext context = HttpContext.Current;
			string key = typeof(T).ToString();

			if (context != null)
			{
				T dc = context.Items[key] as T;

				if (dc == null)
				{
					dc = new T();
					context.Items[key] = dc;
				}

				return dc;
			}
			else
			{
				if (!staticDC.ContainsKey(key))
				{
					T dc = new T();
					staticDC[key] = dc;
				}
				return (T)staticDC[key];
			}
		}

		public static void RemoveDataContext<T>() where T : Devart.Data.Linq.DataContext
		{
			HttpContext context = HttpContext.Current;
			string key = typeof(T).ToString();
			if (context != null) context.Items.Remove(key);
			else staticDC.Remove(key);
		}
	}
}