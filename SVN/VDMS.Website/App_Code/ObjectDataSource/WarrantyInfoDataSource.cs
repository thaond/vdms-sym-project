using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Service;

namespace VDMS.I.ObjectDataSource
{
	public class WarrantyInfoDataSource
	{
		private int count = 0;

		public int SelectCount(int maximumRows, int startRowIndex, string EngineNo, string EngineType, string CusID, string CusName)
		{
			return count;
		}

		public IList<Warrantyinfo> Select(int maximumRows, int startRowIndex, string EngineNo, string EngineType
			, string CusID, string CusName)
		{
			IDao<Warrantyinfo, string> dao = DaoFactory.GetDao<Warrantyinfo, string>();
			List<ICriterion> crit = new List<ICriterion>();
			if (!string.IsNullOrEmpty(EngineNo))
				crit.Add(Expression.InsensitiveLike("Id", EngineNo, MatchMode.Anywhere));
			if (!String.IsNullOrEmpty(EngineType))
				crit.Add(Expression.InsensitiveLike("Itemcode", EngineType, MatchMode.Anywhere));

            if (UserHelper.IsDealer)
                crit.Add(Expression.Eq("CreateByDealer", UserHelper.DealerCode));

			if ((!string.IsNullOrEmpty(CusID)) || (!string.IsNullOrEmpty(CusName)))
			{
				IDao<Customer, long> daoCus = DaoFactory.GetDao<Customer, long>();
				List<ICriterion> critCus = new List<ICriterion>();
				if (!String.IsNullOrEmpty(CusID))
					critCus.Add(Expression.InsensitiveLike("Identifynumber", CusID.Trim(), MatchMode.Anywhere));
				if (!String.IsNullOrEmpty(CusName))
					critCus.Add(Expression.InsensitiveLike("Fullname", CusName.Trim(), MatchMode.Anywhere));
				daoCus.SetCriteria(critCus.ToArray());
				List<Customer> lstCus = daoCus.GetAll();
				crit.Add(Expression.In("Customer", lstCus));
			}

			dao.SetCriteria(crit.ToArray());
			dao.SetOrder(new Order[] { Order.Desc("Id") });
			List<Warrantyinfo> list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
			count = dao.ItemCount;
			HttpContext.Current.Items["rowCount"] = count;
			return list;
		}

		public void Update(string Purchasedate, string Kmcount, string Id)
		{
			int KmC = 0;
			int.TryParse(Kmcount, out KmC);

			DateTime PD;
			DateTime.TryParse(Purchasedate, new CultureInfo(UserHelper.Language), DateTimeStyles.AllowWhiteSpaces, out PD);

			ServiceTools.SaveWarrantyInfo(Id, KmC, PD, null, null, null, null, 0);
		}
	}
}
