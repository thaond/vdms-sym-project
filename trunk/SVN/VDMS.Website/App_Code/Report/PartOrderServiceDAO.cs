using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.Report
{
	public class PartServiceRate
	{
		public string DealerCode { get; set; }
		public int QuotationParts { get; set; }
		public int NotQuotationParts { get; set; }
		public int OrderedParts { get; set; }
		public double SupplyRate { get; set; }
		public int QuotationPieces { get; set; }
		public int OrderedPieces { get; set; }
		public double ServiceRate { get; set; }
		public IEnumerable<OrderDetail> OrderDetails { get; set; }

		public PartServiceRate()
		{
			//OrderedParts = 1;
			//OrderedPieces = 1;
		}
	}

	public class PartOrderServiceDAO
	{

		int partServiceRateCount = 0;
		public int CountInShortOrder(string dealerCode, string dbCode, string dateFrom, string dateTo)
		{
			return partServiceRateCount;
		}
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<PartServiceRate> CalcPartServiceRate(string dealerCode, string dbCode, string dateFrom, string dateTo)
        {
            return CalcPartServiceRate(dealerCode, dbCode, dateFrom, dateTo, -1, -1);
        }

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IList<PartServiceRate> CalcPartServiceRate1(string dealerCode, string dbCode, string dateFrom, string dateTo, int startRowIndex, int maximumRows)
		{
			// khong xai` WHERE roi GROUP roi SUM dc !!!!
			DateTime dtFrom = DataFormat.DateFromString(dateFrom);
			DateTime dtTo = DataFormat.DateFromString(dateTo);
			if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;

			//if (string.IsNullOrEmpty(dbCode)) throw new Exception("CalcPartServiceRate: Invalid database code!");
            if (dbCode == null) dbCode = "";

			var db = DCFactory.GetDataContext<PartDataContext>();
			List<PartServiceRate> res = new List<PartServiceRate>();

			if (!string.IsNullOrEmpty(dealerCode))
			{
				// get dealer list
				DealerDAO dDao = new DealerDAO();
				IList<Dealer> dealers = dDao.SearchDealerByCodeWithDB(dealerCode, dbCode, maximumRows, startRowIndex);
				partServiceRateCount = dDao.CountByCodeWithDB(dealerCode, dbCode);

				// calc result for each dealer
				foreach (Dealer dealer in dealers)
				{
					PartServiceRate item = new PartServiceRate();
					var query = db.OrderDetails.Where(od => od.OrderHeader.Dealer.DealerCode == dealer.DealerCode);

					item.DealerCode = dealer.DealerCode;
					item.OrderedParts = query.GroupBy(od => od.PartCode).Select(g => g.Key).Count();

					List<string> zeroQParts = query.Where(od => od.QuotationQuantity == 0)
												   .GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
					List<string> wereQParts = query.Where(od => od.QuotationQuantity > 0)
												   .GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
					item.NotQuotationParts = zeroQParts.Count - zeroQParts.RemoveAll(p => wereQParts.Contains(p));

					item.QuotationParts = item.OrderedParts - item.NotQuotationParts;
					item.OrderedPieces = query.Sum(od => od.OrderQuantity);
					item.QuotationPieces = query.Sum(od => od.QuotationQuantity);

					res.Add(item);
				}
			}
			else
			{
				PartServiceRate item = new PartServiceRate();
				var query = db.OrderDetails.Where(od => od.OrderHeader.Dealer.DatabaseCode.Contains(dbCode));

				item.DealerCode = Resources.Constants.All;
				item.OrderedParts = query.GroupBy(od => od.PartCode).Select(g => g.Key).Count();

				List<string> zeroQParts = query.Where(od => od.QuotationQuantity == 0)
											   .GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
				List<string> wereQParts = query.Where(od => od.QuotationQuantity > 0)
											   .GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
				item.NotQuotationParts = zeroQParts.Count - zeroQParts.RemoveAll(p => wereQParts.Contains(p));

				item.QuotationParts = item.OrderedParts - item.NotQuotationParts;
				item.OrderedPieces = query.Sum(od => od.OrderQuantity);
				item.QuotationPieces = query.Sum(od => od.QuotationQuantity);

				res.Add(item);
				partServiceRateCount = 1;
			}
			return res;
		}

        public IList<PartServiceRate> CalcPartServiceRate(string dealerCode, string dbCode, string dateFrom, string dateTo, int startRowIndex, int maximumRows)
        {
            DateTime dtFrom = DataFormat.DateFromString(dateFrom);
            DateTime dtTo = DataFormat.DateFromString(dateTo);

            //if (string.IsNullOrEmpty(dbCode)) throw new Exception("CalcPartServiceRate: Invalid database code!");
            if (dbCode == null) dbCode = "";

            var db = DCFactory.GetDataContext<PartDataContext>();
            List<PartServiceRate> res = new List<PartServiceRate>();

            if (!string.IsNullOrEmpty(dealerCode))
            {
                // get dealer list
                DealerDAO dDao = new DealerDAO();
                IList<Dealer> dealers = dDao.SearchDealerByCodeWithDB(dealerCode, dbCode, maximumRows, startRowIndex);
                partServiceRateCount = dDao.CountByCodeWithDB(dealerCode, dbCode);

                // calc result for each dealer
                foreach (Dealer dealer in dealers)
                {
                    PartServiceRate item = new PartServiceRate();
                    var queryO = db.OrderDetails.Where(od => od.OrderHeader.Dealer.DealerCode == dealer.DealerCode && od.OrderHeader.ReferenceRootId == null);
                    var queryS = db.OrderDetails.Where(od => od.OrderHeader.Dealer.DealerCode == dealer.DealerCode && od.OrderHeader.ReferenceRootId != null);

                    if (dtFrom > DateTime.MinValue)
                    {
                        queryS = queryO.Where(od => od.OrderHeader.OrderHeader2.OrderDate >= dtFrom);
                        queryO = queryO.Where(od => od.OrderHeader.OrderDate >= dtFrom);
                    }
                    if (dtTo > DateTime.MinValue)
                    {
                        queryS = queryO.Where(od => od.OrderHeader.OrderHeader2.OrderDate <= dtTo);
                        queryO = queryO.Where(od => od.OrderHeader.OrderDate <= dtTo);
                    }

                    item.DealerCode = dealer.DealerCode;
                    item.OrderedPieces = queryO.Sum(od => od.OrderQuantity);
                    item.QuotationPieces = queryO.Sum(od => od.QuotationQuantity) + queryS.Sum(od => od.QuotationQuantity);

                    var listO = queryO.Where(od => od.QuotationQuantity > 0).GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
                    var listS = queryS.Where(od => od.QuotationQuantity > 0).GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
                    listO.RemoveAll(p => listS.Contains(p));
                    item.QuotationParts = listO.Count + listS.Count;

                    listO = queryO.Where(od => od.OrderQuantity > 0).GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
                    listS = queryS.Where(od => od.OrderQuantity > 0).GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
                    listO.RemoveAll(p => listS.Contains(p));
                    item.OrderedParts = listO.Count + listS.Count;

                    item.NotQuotationParts = item.OrderedParts - item.QuotationParts;

                    res.Add(item);
                }
            }
            else
            {
                PartServiceRate item = new PartServiceRate();
                var queryO = db.OrderDetails.Where(od => od.OrderHeader.Dealer.DatabaseCode.Contains(dbCode) && od.OrderHeader.ReferenceRootId == null);
                var queryS = db.OrderDetails.Where(od => od.OrderHeader.Dealer.DatabaseCode.Contains(dbCode) && od.OrderHeader.ReferenceRootId != null);

                if (dtFrom > DateTime.MinValue)
                {
                    queryS = queryO.Where(od => od.OrderHeader.OrderHeader2.OrderDate >= dtFrom);
                    queryO = queryO.Where(od => od.OrderHeader.OrderDate >= dtFrom);
                }
                if (dtTo > DateTime.MinValue)
                {
                    queryS = queryO.Where(od => od.OrderHeader.OrderHeader2.OrderDate <= dtTo);
                    queryO = queryO.Where(od => od.OrderHeader.OrderDate <= dtTo);
                }

                item.DealerCode = Resources.Constants.All;

                item.OrderedPieces = queryO.Sum(od => od.OrderQuantity);
                item.QuotationPieces = queryO.Sum(od => od.QuotationQuantity) + queryS.Sum(od => od.QuotationQuantity);

                var listO = queryO.Where(od => od.QuotationQuantity > 0).GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
                var listS = queryS.Where(od => od.QuotationQuantity > 0).GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
                listO.RemoveAll(p => listS.Contains(p));
                item.QuotationParts = listO.Count + listS.Count;

                listO = queryO.Where(od => od.OrderQuantity > 0).GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
                listS = queryS.Where(od => od.OrderQuantity > 0).GroupBy(od => od.PartCode).Select(g => g.Key).ToList();
                listO.RemoveAll(p => listS.Contains(p));
                item.OrderedParts = listO.Count + listS.Count;
                
                item.NotQuotationParts = item.OrderedParts - item.QuotationParts;

                res.Add(item);
                partServiceRateCount = 1;
            }
            return res;
        }

	}
}