using System;
using System.Collections.Generic;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.PartManagement
{
	public class TransactionDAO
	{
		int _TransactionsCount;
		public int CountTransactions(string dealerCode, long? wId, string transCode, string fromDate, string toDate, string invNo)
		{
			return _TransactionsCount;
		}

		public List<TransactionHistory> FindTransactions(string dealerCode, long wId, string transCode, string fromDate, string toDate, string invNo, int maximumRows, int startRowIndex)
		{
			DateTime dtFrom = DataFormat.DateFromString(fromDate), dtTo = DataFormat.DateFromString(toDate);
			if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;
			if (invNo != null) invNo = invNo.Trim();

			var dc = DCFactory.GetDataContext<PartDataContext>();
			var query = dc.TransactionHistories.Where(th => th.TransactionDate >= dtFrom && th.TransactionDate <= dtTo);
			if (!string.IsNullOrEmpty(transCode))
			{
				query = query.Where(th => transCode.Split(',').Contains(th.TransactionCode));
			}
			if (!string.IsNullOrEmpty(invNo)) query = query.Where(t => t.InvoiceNumber.Contains(invNo));

			if (wId != 0)
			{
				query = query.Where(th => th.WarehouseId == (long)wId);
			}
			else
			{
				query = query.Where(th => th.DealerCode == dealerCode);
			}
			query = query.OrderBy(th => th.InvoiceNumber).OrderBy(t => t.DealerCode);
			_TransactionsCount = query.Count();

			query.Skip(startRowIndex).Take(maximumRows);
			return query.ToList();
		}

		public static int Summarization(long partInfoId, int month, int year)
		{
			DateTime dtFrom = new DateTime(year, month, 1);
			DateTime dtTo = dtFrom.AddMonths(1);

			return TransactionDAO.Summarization(partInfoId, dtFrom, dtTo);
		}

		public static int Summarization(long partInfoId, DateTime dtFrom, DateTime dtTo)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return db.TransactionHistories.Where(th => th.PartInfoId == partInfoId && th.TransactionDate >= dtFrom && th.TransactionDate < dtTo)
										  .Sum(th => th.Quantity);
		}

		public static IQueryable<TransactionHistory> GetSummaryQuery(long partInfoId, long? warehouseId, string dealerCode, DateTime dtFrom, DateTime dtTo)
		{
			if (dealerCode == null) dealerCode = "";

			var db = DCFactory.GetDataContext<PartDataContext>();
			var query = db.TransactionHistories.Where(th => th.PartInfoId == partInfoId && th.TransactionDate >= dtFrom && th.TransactionDate < dtTo);
			if (warehouseId != null) query = query.Where(th => th.WarehouseId == warehouseId);
			else query = query.Where(th => th.DealerCode == dealerCode);

			return query;
		}

		public static int CountIn(long partInfoId, long? warehouseId, string dealerCode, DateTime dtFrom, DateTime dtTo)
		{
			var query = TransactionDAO.GetSummaryQuery(partInfoId, warehouseId, dealerCode, dtFrom, dtTo);
			return query.Where(th => th.Quantity > 0)
								.Sum(th => th.Quantity);
		}

		public static int CountOut(long partInfoId, long? warehouseId, string dealerCode, DateTime dtFrom, DateTime dtTo)
		{
			var query = TransactionDAO.GetSummaryQuery(partInfoId, warehouseId, dealerCode, dtFrom, dtTo);
			return Math.Abs(query.Where(th => th.Quantity < 0)
								.Sum(th => th.Quantity));
		}

		public static int SumInAmount(long partInfoId, long? warehouseId, string dealerCode, DateTime dtFrom, DateTime dtTo)
		{
			var query = TransactionDAO.GetSummaryQuery(partInfoId, warehouseId, dealerCode, dtFrom, dtTo);
			return query.Where(th => th.Quantity > 0)
								.Sum(th => th.ActualCost);
		}

		public static int SumOutAmount(long partInfoId, long? warehouseId, string dealerCode, DateTime dtFrom, DateTime dtTo)
		{
			var query = TransactionDAO.GetSummaryQuery(partInfoId, warehouseId, dealerCode, dtFrom, dtTo);
			return query.Where(th => th.Quantity < 0)
								.Sum(th => th.ActualCost);
		}

		public static TransactionHistory CreateTransaction(PartInfo partInfo, string dealerCode, long actWarehouse, long? toWarehouse, DateTime transDate, string transCode, int cost, int qty, string comment, string invNo, long? vendorId)
		{
			//var db = new PartDataContext(); // must create another data context
			var db = DCFactory.GetDataContext<PartDataContext>();
			var trans = new TransactionHistory
			{
				ActualCost = cost,
				CreatedBy = UserHelper.Username,
				CreatedDate = DateTime.Now,
				DealerCode = dealerCode,
				InvoiceNumber = invNo,
				PartInfoId = partInfo.PartInfoId,
				Quantity = qty,
				TransactionCode = transCode,
				TransactionComment = comment,
				TransactionDate = transDate,
				SecondaryWarehouseId = toWarehouse,
				VendorId = vendorId,
				WarehouseId = actWarehouse,
			};

			db.TransactionHistories.InsertOnSubmit(trans);
			return trans;
		}
	}
}