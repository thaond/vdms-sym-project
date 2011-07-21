using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using VDMS.Common.Utils;
using VDMS.II.Entity;
using VDMS.II.Linq;
namespace VDMS.II.PartManagement
{
	public class BindCardPart
	{
		public string PartCode { get; set; }
		public string EnglishName { get; set; }
		public string VietnamName { get; set; }
		public Inventory Begin { get; set; }
		public string ActDate { get; set; }
		public long PartInfoId { get; set; }
		public IEnumerable<BindCardItem> Items { get; set; }
	}
	public class BindCardItem
	{
		public int No { get; set; }
		public int Quantity { get; set; }
		public int Amount { get; set; }
		public long PartInfoId { get; set; }
		public DateTime ActDate { get; set; }
		public string VoucherNo { get; set; }
		public string PartCode { get; set; }
		public string ActDateString { get; set; }
		public string BeginQuantity { get; set; }
		public string InQuantity { get; set; }
		public string OutQuantity { get; set; }
		public string InAmount { get; set; }
		public string OutAmount { get; set; }
		public string Balance { get; set; }
		public string TransactionCode { get; set; }
		public string TransactionComment { get; set; }
		public string FromWH { get; set; }
		public string ToWH { get; set; }
		public long WarehouseId { get; set; }
		public long? ToWarehouseId { get; set; }

	}

	//public class BindCard

	public class BinCardDAO
	{

		public static PartDataContext PartDC
		{
			get { return DCFactory.GetDataContext<PartDataContext>(); }
		}

		#region bindCard v1

		int bindCardPartCount;
		public int CountBindCardPart(string partType, string partCode, string fromDate, string toDate, string dealerCode, long? warehouseId)
		{
			return bindCardPartCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public object FindBindCardPart(string partType, string partCode, string fromDate, string toDate, string dealerCode, long? warehouseId, int maximumRows, int startRowIndex)
		{
			//if (warehouseId == null) return null;
			if (string.IsNullOrEmpty(partCode)) partCode = "";
			else partCode = partCode.Trim();

			DateTime dtFrom = DataFormat.DateFromString(fromDate);
			DateTime dtTo = DataFormat.DateFromString(toDate);
			if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;

			// base query
			var query = PartDC.PartInfos.Where(pi => pi.PartCode.Contains(partCode) && pi.PartType == partType);

			// part used by selected warehouse and has transactions
			if (warehouseId == null)
			{
				query = query.Where(pi => pi.PartSafeties.Where(ps => ps.Warehouse.DealerCode == dealerCode).Count() > 0)
					// remove empty items
					//.Where(pi => pi.TransactionHistories.Where(th => th.DealerCode == dealerCode && th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo && th.Quantity != 0).Count() > 0)
							 ;
			}
			else
			{
				query = query.Where(pi => pi.PartSafeties.Where(ps => ps.WarehouseId == warehouseId).Count() > 0)
					// remove empty items
					//.Where(pi => pi.TransactionHistories.Where(th => th.WarehouseId == warehouseId && th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo && th.Quantity != 0).Count() > 0)
							 ;
			}

			//var q = query.ToList();
			int beginMonth = dtFrom.Month - 1, beginYear = dtFrom.Year;
			if (beginMonth == 0)
			{
				beginMonth = 12; beginYear--;
			}

			bindCardPartCount = query.Count();

			if (warehouseId == null)
			{
				if ((startRowIndex >= 0) && (maximumRows >= 0)) query = query.Skip(startRowIndex).Take(maximumRows);
				var res = query.Select(p => new BindCardPart
								  {
									  PartInfoId = p.PartInfoId,
									  PartCode = p.PartCode,
									  Begin = p.Inventories.SingleOrDefault(piv => piv.Month == beginMonth && piv.Year == beginYear && piv.WarehouseId == null && piv.DealerCode == dealerCode),
									  Items = p.TransactionHistories.Where(th => th.DealerCode == dealerCode)
															 .Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo && th.Quantity != 0)
															 .OrderBy(th => th.CreatedDate)
															 .Select(th => new BindCardItem
																	  {
																		  FromWH = th.Warehouse.Address,
																		  ToWH = th.Warehouse1.Address,
																		  Amount = th.ActualCost,
																		  Quantity = th.Quantity,
																		  VoucherNo = th.InvoiceNumber,
																		  ActDate = th.TransactionDate,
																		  TransactionCode = th.TransactionCode,
																		  TransactionComment = th.TransactionComment,
																	  })
								  }).ToList();
				return res;
			}
			else
			{
				if ((startRowIndex >= 0) && (maximumRows >= 0)) query = query.Skip(startRowIndex).Take(maximumRows);
				var res = query.Select(p => new BindCardPart
							 {
								 PartInfoId = p.PartInfoId,
								 PartCode = p.PartCode,
								 Begin = p.Inventories.SingleOrDefault(piv => piv.Month == beginMonth && piv.Year == beginYear && piv.WarehouseId == warehouseId),
								 Items = p.TransactionHistories.Where(th => th.WarehouseId == warehouseId)
														.Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo && th.Quantity != 0)
														.OrderBy(th => th.CreatedDate)
														.Select(th => new BindCardItem
														{
															FromWH = th.Warehouse.Address,
															ToWH = th.Warehouse1.Address,
															Amount = th.ActualCost,
															Quantity = th.Quantity,
															VoucherNo = th.InvoiceNumber,
															ActDate = th.TransactionDate,
															TransactionCode = th.TransactionCode,
															TransactionComment = th.TransactionComment,
														})
							 });
				return res;
			}

		}



		#endregion

		#region BindCard v2

		int bindCardWHCount;
		public int CountBindCardWH(string dealerCode, long wid)
		{
			return bindCardWHCount;
		}
		/// <summary>
		/// Get warehouses list for bindCard report
		/// </summary>
		/// <param name="dealerCode"></param>
		/// <param name="wid"></param>
		/// <param name="maximumRows"></param>
		/// <param name="startRowIndex"></param>
		/// <returns></returns>
		public IQueryable<Warehouse> FindBindCardWH(string dealerCode, long wid, int maximumRows, int startRowIndex)
		{
            var query = PartDC.ActiveWarehouses.Where(w => w.DealerCode == dealerCode && w.Type == WarehouseType.Part);
			if (wid > 0) query = query.Where(w => w.WarehouseId == wid);
			bindCardWHCount = query.Count();
			if ((startRowIndex >= 0) && (maximumRows > 0)) query = query.Skip(startRowIndex).Take(maximumRows);
			return query;
		}

		int bindCardActCount;
		public int CountBindCardAct(string partType, string partCode, string fromDate, string toDate, string dealerCode, long? warehouseId)
		{
			return bindCardActCount;
		}

		/// <summary>
		/// Get binCard actions for parts
		/// </summary>
		/// <param name="partType"></param>
		/// <param name="partCode"></param>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <param name="dealerCode"></param>
		/// <param name="warehouseId"></param>
		/// <param name="maximumRows"></param>
		/// <param name="startRowIndex"></param>
		/// <returns></returns>
		public IQueryable<BindCardItem> FindBindcardAct(string partType, string partCode, string fromDate, string toDate, string dealerCode, long warehouseId, int maximumRows, int startRowIndex)
		{
			if (string.IsNullOrEmpty(partCode)) partCode = "";
			else partCode = partCode.Trim();

			DateTime dtFrom = DataFormat.DateFromString(fromDate);
			DateTime dtTo = DataFormat.DateFromString(toDate);
			if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;

			// get parts list
			var parts = PartDC.PartInfos.Where(p => p.DealerCode == dealerCode && p.PartType == partType && p.PartCode.Contains(partCode));
			parts = parts.Where(p => p.PartSafeties.Where(ps => ps.WarehouseId == warehouseId).Count() > 0);

			// get transactions 
			var trans = PartDC.TransactionHistories.Where(t => t.TransactionDate >= dtFrom && t.TransactionDate <= dtTo && t.DealerCode == dealerCode);
			if (warehouseId != null) trans = trans.Where(t => t.WarehouseId == warehouseId);

			// join parts with transactions and build bincard data
			var res = from p in parts
					  join t in trans on p.PartInfoId equals t.PartInfoId into pt_join
					  from pt in pt_join.DefaultIfEmpty()
					  orderby p.PartCode, pt.TransactionDate, pt.CreatedDate
					  select new BindCardItem
					  {
						  ActDate = pt.TransactionDate,
						  Amount = pt.ActualCost,
						  Quantity = pt.Quantity,
						  PartCode = p.PartCode,
						  PartInfoId = p.PartInfoId,
						  TransactionCode = pt.TransactionCode,
						  TransactionComment = pt.TransactionComment,
						  VoucherNo = pt.InvoiceNumber,
						  WarehouseId = warehouseId,
						  ToWarehouseId = pt.SecondaryWarehouseId,
						  //BeginQuantity

					  };
			bindCardActCount = res.Count();

			if ((startRowIndex >= 0) && (maximumRows > 0)) res = res.Skip(startRowIndex).Take(maximumRows);

			return res;
		}

		#endregion
	}
}