using System;
using System.Collections.Generic;
using System.Text;

namespace VDMS.Common.Web.GridViewHepler
{
	/// <summary>
	/// Summary description for GridViewSummaryList
	/// </summary>
	public class GridViewSummaryList : List<GridViewSummary>
	{
		public GridViewSummary this[string name]
		{
			get { return this.FindSummaryByColumn(name); }
		}

		public GridViewSummary FindSummaryByColumn(string columnName)
		{
			foreach (GridViewSummary s in this)
			{
				if (s.Column.ToLower() == columnName.ToLower()) return s;
			}

			return null;
		}
	}
}
