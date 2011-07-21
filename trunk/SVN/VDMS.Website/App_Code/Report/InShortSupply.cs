using System;
using System.ComponentModel;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.II.Linq;

namespace VDMS.II.Report
{
	#region Object

	public class InShortSupplier
	{
		public string PartCode { get; set; }
		public string EnName { get; set; }
		public string VnName { get; set; }
		public decimal TotalOrder { get; set; }
		public decimal TotalQuotation { get; set; }
		public decimal TotalShort { get; set; }
		public decimal ShortRate
		{
			get
			{
				if (this.TotalOrder == 0) return 0;
				else return (this.TotalShort / this.TotalOrder);
			}
		}
		public string StringShortRate
		{
			get
			{
				return (ShortRate * 100).ToString("N2") + "%";
			}
		}
	}

	#endregion

	public class InShortSupplyDAO
	{

		int inShortSupplierCount = 0;
		public int CountInShortSupplier(string dealerCode, string dbCode, string dateFrom, string dateTo, string orderBy)
		{
			return inShortSupplierCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable<InShortSupplier> FindInShortSupplier(int maximumRows, int startRowIndex, string dealerCode, string dbCode, string dateFrom, string dateTo, string orderBy)
		{
			DateTime dtFrom = DataFormat.DateFromString(dateFrom);
			DateTime dtTo = DataFormat.DateFromString(dateTo);
			if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;
			if (dbCode == null) dbCode = "";
			if (dealerCode == null) dealerCode = "";

			var db = DCFactory.GetDataContext<PartDataContext>();

			// base condition
			var oHs = db.OrderHeaders.Where(oh => oh.Dealer.DatabaseCode.Contains(dbCode)
				&& oh.OrderDate >= dtFrom
				&& oh.OrderDate <= dtTo
				&& oh.DealerCode.Contains(dealerCode)
				);

			var query = db.OrderDetails
				//.Where(od => od.OrderQuantity > od.QuotationQuantity)
							   .Join(oHs, od => od.OrderHeaderId, oh => oh.OrderHeaderId, (od, oh) => od)
							   ;

			//var list = db.OrderDetails
			var list = query
					.GroupBy(p => p.PartCode)
					.Join(db.Parts.Where(p => p.DatabaseCode.Contains(dbCode)), g => g.Key, p => p.PartCode,
						(g, p) => new InShortSupplier()
						{
							PartCode = g.Key,
							EnName = p.EnglishName,
							VnName = p.VietnamName,
							TotalQuotation = g.Sum(od => od.QuotationQuantity),
							TotalOrder = g.Sum(od => od.OrderQuantity),
							TotalShort = g.Sum(od => od.OrderQuantity) - g.Sum(od => od.QuotationQuantity)
						}
					  ).Where(od => od.TotalOrder != od.TotalQuotation);
			;

			inShortSupplierCount = list.Count();
			switch (orderBy)
			{
				case "O": list = list.OrderByDescending(p => p.TotalOrder);
					break;
				case "S": list = list.OrderByDescending(p => p.TotalShort);
					break;
				case "R": list = list.OrderByDescending(p => p.TotalShort / (p.TotalOrder + 1));
					break;
			}
			return list.Skip(startRowIndex).Take(maximumRows);
		}

		public IQueryable FindInShortSupplier(string dealerCode, string dbCode, string dateFrom, string dateTo, string orderBy)
		{
			return FindInShortSupplier(int.MaxValue, 0, dealerCode, dbCode, dateFrom, dateTo, orderBy);
		}
	}
}