using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.Report
{
	public class InshortPart
	{
		public string EnglishName { get; set; }
		public string VietnamName { get; set; }
		public string PartCode { get; set; }
		public int QuotationQuantity { get; set; }
		public int OrderQuantity { get; set; }
		public int ShortQuantity { get; set; }

		public InshortPart(OrderDetail od, Part p)
		{
			this.EnglishName = p.EnglishName;
			this.VietnamName = p.VietnamName;
			this.OrderQuantity = od.OrderQuantity;
			this.QuotationQuantity = od.QuotationQuantity;
			this.ShortQuantity = this.OrderQuantity - this.QuotationQuantity;
			this.PartCode = od.PartCode;
		}
	}

	public class InShortOrderDAO
	{

		int inShortOrderCount = 0;
		public int CountInShortOrder(string dealerCode, string orderNumber, string dateFrom, string dateTo)
		{
			return inShortOrderCount;
		}
		public int CountInShortOrder(string orderNumber, string dateFrom, string dateTo)
		{
			return inShortOrderCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IList<OrderHeader> FindInShortOrder(string dealerCode, string orderNumber, string dateFrom, string dateTo, int maximumRows, int startRowIndex)
		{
			DateTime dtFrom = DataFormat.DateFromString(dateFrom);
			DateTime dtTo = DataFormat.DateFromString(dateTo);
			if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;
			if (orderNumber == null) orderNumber = "";

			var db = DCFactory.GetDataContext<PartDataContext>();
			var query = db.OrderHeaders
				// base condition
				.Where(p => p.DealerCode == dealerCode)
				.Where(p => p.OrderDate >= dtFrom && p.OrderDate <= dtTo)
				.Where(p => p.TipTopNumber.Contains(orderNumber))
				// xac dinh order ko du
				.Where(p => p.OrderDetails.Where(od => od.QuotationQuantity < od.OrderQuantity).Count() > 0)
				;

			inShortOrderCount = query.Count();
            if ((startRowIndex >= 0) && (maximumRows >= 0))
                return query.Skip(startRowIndex).Take(maximumRows).ToList();
            else 
                return query.ToList();
		}
	
        [DataObjectMethod(DataObjectMethodType.Select)]
		public IList<OrderHeader> FindInShortOrder(string orderNumber, string dateFrom, string dateTo)
		{
			return FindInShortOrder(UserHelper.DealerCode, orderNumber, dateFrom, dateTo, -1, -1);
		}
	
        [DataObjectMethod(DataObjectMethodType.Select)]
		public IList<OrderHeader> FindInShortOrder(string orderNumber, string dateFrom, string dateTo, int maximumRows, int startRowIndex)
		{
			return FindInShortOrder(UserHelper.DealerCode, orderNumber, dateFrom, dateTo, maximumRows, startRowIndex);
		}


		public static IEnumerable<InshortPart> GetInshortPartList(EntitySet<OrderDetail> orderDetails)
		{
			if ((orderDetails != null) && (orderDetails.Count > 0))
			{
				string dbCode = orderDetails[0].OrderHeader.Dealer.DatabaseCode;
				var dc = DCFactory.GetDataContext<PartDataContext>();
				var res = orderDetails.Where(od => od.QuotationQuantity < od.OrderQuantity)
							.Join(dc.Parts.Where(p => p.DatabaseCode == dbCode), od => od.PartCode, p => p.PartCode,
								  (od, p) => new InshortPart(od, p));
				return res;
			}
			else return null;
		}
	}
}