using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Resources;
using VDMS.I.Vehicle;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.BasicData
{
	public class WarehouseLockingInfo
	{
		public long WarehouseId { get; set; }
		public string DealerCode { get; set; }
		public string Address { get; set; }
		public string Code { get; set; }
		public int LastMonth { get; set; }
		public int LastYear { get; set; }
	}

	public class WarehouseDAO
	{
		public WarehouseDAO()
		{
		}

		int _whCount = 0;
		public int CountWithCode(string DealerCode, string code, string type)
		{
			return _whCount;
		}
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IEnumerable<Warehouse> SearchWithCodeForReport(string DealerCode, string code, string type)
		{
			return SearchWithCodeForReport(DealerCode, code, type, -1, -1);
		}
		public IEnumerable<Warehouse> SearchWithCodeForReport(string DealerCode, string code, string type, int maximumRows, int startRowIndex)
		{
			if (string.IsNullOrEmpty(code))
			{
				var res = new List<Warehouse>();
				res.Add(new Warehouse() { Code = "", Address = Constants.All });
				_whCount = 1;
				return res;
			}
			return SearchWithCode(DealerCode, code, type, -1, -1);
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IEnumerable<Warehouse> SearchWithCode(string DealerCode, string code, string type)
		{
			return SearchWithCode(DealerCode, code, type, -1, -1);
		}
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IEnumerable<Warehouse> SearchWithCode(string DealerCode, string code, string type, int maximumRows, int startRowIndex)
		{
            var query = DC.ActiveWarehouses.Where(w => w.Type == type && w.DealerCode == DealerCode);
			if (!string.IsNullOrEmpty(code)) query = query.Where(w => w.Code == code);

			_whCount = query.Count();
			if ((startRowIndex >= 0) && (maximumRows > 0)) query = query.Skip(startRowIndex).Take(maximumRows);
			return query.ToList();
		}

		#region Get all

		public int GetCount(string DealerCode)
		{
			return GetCount(DealerCode, WarehouseType.Part);
		}
		public int GetCount(string DealerCode, string type)
		{
			PartDataContext db = DCFactory.GetDataContext<PartDataContext>();
            return db.ActiveWarehouses.Where(p => p.DealerCode == DealerCode && p.Type == type).Count();
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IEnumerable<Warehouse> FindAll(string DealerCode, int maximumRows, int startRowIndex)
		{
			return FindAll(DealerCode, WarehouseType.Part, maximumRows, startRowIndex);
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IEnumerable<Warehouse> FindAll(string DealerCode, string type, int maximumRows, int startRowIndex)
		{
			PartDataContext db = DCFactory.GetDataContext<PartDataContext>();
			//return db.Dealers.OrderBy(p => p.Code).Skip(startRowIndex).Take(maximumRows).AsEnumerable<Dealer>();
            return db.ActiveWarehouses.Where(p => p.DealerCode == DealerCode && p.Type == type)
						.OrderBy(p => p.Code)
						.Skip(startRowIndex)
						.Take(maximumRows).AsEnumerable<Warehouse>();
		}

		#endregion

		#region for close month

		int _countWithLock;
		public int CountWithLock(string DealerCode)
		{
			return _countWithLock;
		}
		//public int CountWithLock(string DealerCode, string type)
		//{
		//    return _countWithLock;
		//}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IList<WarehouseLockingInfo> FindWithLock1(string DealerCode, int maximumRows, int startRowIndex)
		{
			PartDataContext dc = DCFactory.GetDataContext<PartDataContext>();
			VDMS.I.Linq.VehicleDataContext dc1 = DCFactory.GetDataContext<VDMS.I.Linq.VehicleDataContext>();

            var query = from w in dc.ActiveWarehouses
						where w.DealerCode == DealerCode && w.Type == WarehouseType.Vehicle
						select new WarehouseLockingInfo
						{
							WarehouseId = w.WarehouseId,
							DealerCode = w.DealerCode,
							Address = w.Address,
							Code = w.Code,
							LastMonth = 0,
							LastYear = 0,
						};

			_countWithLock = query.Count();
			var res = query.Skip(startRowIndex).Take(maximumRows).ToList();

			res.ForEach(w =>
				{
					var lck = InventoryHelper.GetInventoryLock(w.DealerCode, w.Code);
					if (lck != null)
					{
						w.LastMonth = (int)lck.Month;
						w.LastYear = (int)lck.Year;
					}
				});
			return res;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindWithLock(string DealerCode, int maximumRows, int startRowIndex)
		{
			PartDataContext dc = DCFactory.GetDataContext<PartDataContext>();
            var query = from w in dc.ActiveWarehouses
						join i in dc.InventoryLocks on w.WarehouseId equals i.WarehouseId into wiJoin
						where w.DealerCode == DealerCode && w.Type == WarehouseType.Part
						from wi in wiJoin.DefaultIfEmpty()
						select new
						{
							WarehouseId = w.WarehouseId,
							DealerCode = w.DealerCode,
							Address = w.Address,
							Code = w.Code,
							LastMonth = wi.Month,
							LastYear = wi.Year,
						};

			_countWithLock = query.Count();
			return query.Skip(startRowIndex).Take(maximumRows);
		}


		[DataObjectMethod(DataObjectMethodType.Delete)]
		public void Delete(long WarehouseId)
		{
			Warehouse wh = WarehouseDAO.GetWarehouse(WarehouseId);
			if (wh != null)
			{
				DC.Warehouses.DeleteOnSubmit(wh);
				DC.SubmitChanges();
			}
		}

		[DataObjectMethod(DataObjectMethodType.Update)]
		public void Update(long WarehouseId, string Code, string Address)
		{
			WarehouseDAO.CreateOrUpdate(WarehouseId, Code, null, Address);
		}

		#endregion

		#region Static

		public static PartDataContext DC
		{
			get
			{
				return DCFactory.GetDataContext<PartDataContext>();
			}
		}

        public static IList<Warehouse> GetAllWarehouses(string dealerCode, string type)
        {
            return DC.Warehouses.Where(w => w.Type == type && w.DealerCode == dealerCode).ToList();
        }

		public static IList<Warehouse> GetWarehouses(string dealerCode, string type)
		{
            return DC.ActiveWarehouses.Where(w => w.Type == type && w.DealerCode == dealerCode).ToList();
		}

		public static bool IsWarehouseExist(string code, string dealerCode)
		{
			return WarehouseDAO.GetWarehouse(code, dealerCode) != null;
		}
		public static bool IsWarehouseExist(string code, string dealerCode, string type)
		{
			return WarehouseDAO.GetWarehouse(code, dealerCode, type) != null;
		}

		public static Warehouse GetWarehouse(long id)
		{
			return GetWarehouse(id, WarehouseType.Part);
		}
		public static Warehouse GetWarehouse(long id, string type)
		{
            Warehouse wh = DC.ActiveWarehouses.SingleOrDefault(c => c.WarehouseId == id && c.Type == type);
			return wh;
		}

		public static Warehouse GetWarehouse(string code, string dealerCode)
		{
			return GetWarehouse(code, dealerCode, WarehouseType.Part);
		}
		public static Warehouse GetWarehouse(string code, string dealerCode, string type)
		{
            return DC.ActiveWarehouses.SingleOrDefault(c => c.Code == code && c.DealerCode == dealerCode && c.Type == type);
		}
        public static Warehouse GetWarehouseAll(string code, string dealerCode, string type)
        {
            return DC.Warehouses.SingleOrDefault(c => c.Code == code && c.DealerCode == dealerCode && c.Type == type);
        }
		public static Warehouse CreateOrUpdate(long WarehouseId, string Code, string dealerCode, string Address, string type)
		{
			return WarehouseDAO.Create(WarehouseId, Code, dealerCode, Address, type, true);
		}
		public static Warehouse CreateOrUpdate(long WarehouseId, string Code, string dealerCode, string Address)
		{
			return WarehouseDAO.Create(WarehouseId, Code, dealerCode, Address, WarehouseType.Part, true);
		}

		public static Warehouse Create(long WarehouseId, string Code, string dealerCode, string Address, string type, bool overwrite)
		{
			Warehouse wh = WarehouseDAO.GetWarehouse(WarehouseId);
			if (wh == null) wh = WarehouseDAO.GetWarehouse(Code, dealerCode);

			if ((wh != null) && !overwrite)
			{
				throw new Exception(string.Format(Resources.Message.ItemAlreadyExist, Code));
			}

			if ((wh == null) && (dealerCode != null))
			{
				wh = new Warehouse() { DealerCode = dealerCode, };
				DC.Warehouses.InsertOnSubmit(wh);
			}

			if (wh != null)
			{
				wh.Code = Code;
				wh.Address = Address;
				wh.Type = type;
				DC.SubmitChanges();
			}

			return wh;
		}
		public static Warehouse Create(long WarehouseId, string Code, string dealerCode, string Address, bool overwrite)
		{
			return Create(WarehouseId, Code, dealerCode, Address, WarehouseType.Part, overwrite);
		}

		#endregion
	}
}
