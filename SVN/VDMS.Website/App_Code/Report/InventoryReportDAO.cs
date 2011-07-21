using System.ComponentModel;
using System.Linq;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.Report
{
    public class InventoryReportDAO
    {
        public static PartDataContext PartDC
        {
            get
            {
                return DCFactory.GetDataContext<PartDataContext>();
            }
        }



        int _DealerCount = 0;
        public int CountInstockByPart(string partCode, string dealer, string type)
        {
            return _DealerCount;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public object SearchInstockByPart(string partCode, string dealer, string type)
        {
            return SearchInstockByPart(partCode, dealer, type, -1, -1);
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public object SearchInstockByPart(string partCode, string dealer, string type, int maximumRows, int startRowIndex)
        {
            if (partCode == null) partCode = ""; else partCode = partCode.Trim().ToUpper();
            if (type == null) type = "";

            var initQ = InventoryReportDAO.PartDC.PartSafeties.Where(ps => ps.PartInfo.PartCode == partCode && ps.PartInfo.PartType == type);
            if (!string.IsNullOrEmpty(dealer))
            {
                initQ = initQ.Where(ps => ps.PartInfo.DealerCode == dealer && ps.Warehouse.DealerCode == dealer);
            }

            var query = initQ.Select(p => new
            {
                DealerCode = p.PartInfo.DealerCode,
                DealerName = p.PartInfo.Dealer.DealerName,
                CurrentStock = p.CurrentStock,
            }).GroupBy(p => new { DealerCode = p.DealerCode, DealerName = p.DealerName }).Select(g => new
            {
                DealerCode = g.Key.DealerCode,
                DealerName = g.Key.DealerName,
                CurrentStock = g.Sum(p => p.CurrentStock),
            });
            _DealerCount = query.Count();

            if ((startRowIndex >= 0) && (maximumRows > 0)) query = query.Skip(startRowIndex).Take(maximumRows);

            return query;
        }


        int partsCount = 0;
        public int CountPartInstock(string partCode, string dealerCode, long? wId, string type)
        {
            return partsCount;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public object SearchPartInstock(string partCode, string dealerCode, long? wId, string type)
        {
            return SearchPartInstock(partCode, dealerCode, wId, type, -1, -1);
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public object SearchPartInstock(string partCode, string dealerCode, long? wId, string type, int maximumRows, int startRowIndex)
        {
#warning rewrite when posible
            if (partCode == null) partCode = ""; else partCode = partCode.Trim().ToUpper();
            if (type == null) type = "";
            string dbCode = UserHelper.DatabaseCode;

            var initQ = InventoryReportDAO.PartDC.PartSafeties.Where(ps => ps.PartInfo.PartCode.Contains(partCode) && ps.PartInfo.PartType == type);
            //var initQ = InventoryReportDAO.PartDC.PartSafeties.Where(ps => ps.PartInfo.PartCode == partCode && ps.PartInfo.PartType == type);

            if (wId != null)
            {
                Warehouse wh = WarehouseDAO.GetWarehouse((long)wId);
                if (wh == null) return null;
                initQ = initQ.Where(p => p.WarehouseId == wId);
                dbCode = wh.Dealer.DatabaseCode;
            }
            if (!string.IsNullOrEmpty(dealerCode))
            {
                Dealer dealer = DealerDAO.GetDealerByCode(dealerCode.ToString());
                if (dealer == null) return null;
                initQ = initQ.Where(p => p.Warehouse.DealerCode == dealerCode);
                dbCode = dealer.DatabaseCode;
            }

            var query = initQ.Select(ps => new
                             {
                                 PartCode = ps.PartInfo.PartCode,
                                 CurrentStock = ps.CurrentStock
                             })
                             .ToList()
                             .GroupBy(p => p.PartCode)
                             .Select(g => new
                             {
                                 PartCode = g.Key,
                                 CurrentStock = g.Sum(ps => ps.CurrentStock)
                             });

            if (type == "A")
            {
                var res = query.Join(InventoryReportDAO.PartDC.PartInfos, g => g.PartCode, pi => pi.PartCode,
                                                        (g, pi) => new
                                                        {
                                                            PartCode = pi.PartCode,
                                                            EnglishName = pi.Accessory.EnglishName,
                                                            VietnamName = pi.Accessory.VietnamName,
                                                            CurrentStock = g.CurrentStock
                                                        }).Distinct();

                partsCount = res.Count();
                if ((startRowIndex >= 0) && (maximumRows >= 0)) res = res.Skip(startRowIndex).Take(maximumRows);
                return res;
            }
            else
            {
                //res = query.Join(InventoryReportDAO.PartDC.Parts.Where(p => p.DatabaseCode == dbCode), ps => ps.PartCode, p => p.PartCode,
                //            (ps, p) => new
                //            {
                //                PartCode = p.PartCode,
                //                EnglishName = p.EnglishName,
                //                VietnamName = p.VietnamName,
                //                CurrentStock = ps.CurrentStock
                //            });
                var res = query.Select(p => new
                {
                    PartCode = p.PartCode,
                    EnglishName = PartDC.Parts.FirstOrDefault(i => i.PartCode == p.PartCode).EnglishName,
                    VietnamName = PartDC.Parts.FirstOrDefault(i => i.PartCode == p.PartCode).VietnamName,
                    CurrentStock = p.CurrentStock
                });

                partsCount = res.Count();
                if ((startRowIndex >= 0) && (maximumRows > 0)) res = res.Skip(startRowIndex).Take(maximumRows);
                
                return res;
            }
        }
    }
}