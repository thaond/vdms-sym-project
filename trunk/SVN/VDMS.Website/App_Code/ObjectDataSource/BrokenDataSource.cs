using System;
using System.Collections.Generic;
using System.Web;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;

namespace VDMS.I.ObjectDataSource
{
	public enum BrokenErrorCode
	{
		OK,
		BrokenInUse,
		BrokenCodeExist,
	}

	public class BrokenDatasource
	{
		private int count = 0;
		private IDao<Broken, long> dao;

		// return maximum record
		public int SelectCount(string fromCode, string toCode)
		{
			return count;
		}
		public int SelectCount(int maximumRows, int startRowIndex, string fromCode)
		{
			return count;
		}
		public bool Insert(string BrokenCode, string Brokenname, string editby)
		{
			dao.SetCriteria(new ICriterion[] { Expression.Eq("Brokencode", BrokenCode.Trim()) });
			List<Broken> lst = dao.GetAll();
			if (lst.Count > 0) return false;
			try
			{
				Broken item = new Broken(Brokenname.Trim(), DateTime.Now, editby.Trim(), BrokenCode.Trim());
				dao.Save(item);
				return true;
			}
			catch { return false; }
		}
		public IList<Broken> Select(int maximumRows, int startRowIndex, string fromCode, string toCode)
		{
			List<Broken> list = null;
			string sql = "";
			if ((fromCode != null) && (fromCode.Trim() != "")) sql = "TO_NUMBER(BrokenCode) >= " + fromCode;
			if ((fromCode != null) && (toCode != null) && (fromCode.Trim() != "") && (toCode.Trim() != "")) sql += " and ";
			if ((toCode != null) && (toCode.Trim() != "")) sql += "TO_NUMBER(BrokenCode) <= " + toCode;
			try
			{
				dao.SetCriteria(new ICriterion[] { Expression.Sql(sql) });
				dao.SetOrder(new Order[] { Order.Asc("Brokencode") });
				//list = dao.GetAll();
				list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
				count = dao.ItemCount;

				// if user change pageindex after change "select conditions" without press "check" button
				// so we select with pageindex=0
				if (startRowIndex >= count)
				{
					list = dao.GetPaged(0, maximumRows);
					count = dao.ItemCount;
				}
				HttpContext.Current.Items["brokenRowCount"] = count;
			}
			catch { };
			return list;
		}

		public IList<Broken> Select(int maximumRows, int startRowIndex, string fromCode)
		{
			List<Broken> list = null;

			string code = string.IsNullOrEmpty(fromCode) ? "%" : fromCode.Trim() + "%";
			try
			{
				dao.SetCriteria(new ICriterion[] { Expression.Like("Brokencode", code) });
				dao.SetOrder(new Order[] { Order.Asc("Brokencode") });
				//list = dao.GetAll();
				list = dao.GetPaged(startRowIndex / maximumRows, maximumRows);
				count = dao.ItemCount;

				// if user change pageindex after change "select conditions" without press "check" button
				// so we select with pageindex=0
				if (startRowIndex >= count)
				{
					list = dao.GetPaged(0, maximumRows);
					count = dao.ItemCount;
				}
				HttpContext.Current.Items["listgvSelectBrokenRowCount"] = count;
			}
			catch { };
			return list;
		}

		public IList<Broken> Select()
		{
			List<Broken> list = null;
			try
			{
				dao.SetOrder(new Order[] { Order.Asc("Id") });
				list = dao.GetAll();
				count = dao.ItemCount;
			}
			catch { };
			return list;
		}
		public void Delete(long Id)
		{
			Broken item = new Broken();
			item.Id = Id;

			dao.Delete(item);
		}
		public static Broken GetByCode(string brokenCode)
		{
			IDao<Broken, long> bDao = DaoFactory.GetDao<Broken, long>();
			bDao.SetCriteria(new ICriterion[] { Expression.Eq("Brokencode", brokenCode.Trim()) });
			List<Broken> list = bDao.GetAll();
			if (list.Count < 1) return null;
			return list[0];
		}
		public static bool Delete(string brokenCode)
		{
			IDao<Broken, long> bDao = DaoFactory.GetDao<Broken, long>();
			bDao.SetCriteria(new ICriterion[] { Expression.Eq("Brokencode", brokenCode.Trim()) });
			List<Broken> list = bDao.GetAll();
			if (list.Count != 1) return false;
			try
			{
				using (TransactionBlock trans = new TransactionBlock())
				{
					Broken item = (Broken)list[0];
					bDao.Delete(item);
					trans.IsValid = true;
				}
			}
			catch { return false; }
			return true;
		}
		public bool Update(long Id, string Brokenname, string Lastupdate, string BrokenCode, string Editby)
		{
			// check for new Code exist?
			//dao.SetCriteria(new ICriterion[] { Expression.Eq("Brokencode", BrokenCode.Trim()) });
			//List<Broken> lst = dao.GetAll();
			//if (lst.Count > 0) return false;

			// get current Item and update
			Broken item = dao.GetById(Id, false); //true -> false
			if (item == null) return false;
			item.Brokencode = BrokenCode.Trim();
			item.Editby = Editby.Trim();
			item.Lastupdate = DateTime.Now;
			item.Brokenname = Brokenname.Trim();
			try
			{
				return (dao.SaveOrUpdate(item) != null);
			}
			catch { return false; }
		}
		public BrokenDatasource()
		{
			dao = DaoFactory.GetDao<Broken, long>();
		}
	}
}