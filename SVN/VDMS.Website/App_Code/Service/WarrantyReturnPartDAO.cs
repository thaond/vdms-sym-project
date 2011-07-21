using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.Common.Utils;
using VDMS.II.Linq;

namespace VDMS.I.Service
{
    public class WarrantyReturnPart
    {
        public string PartCode { get; set; }
        public long Quantity { get; set; }
    }

    public class WarrantyReturnPartDAO
    {
        public WarrantyReturnPartDAO()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static IList<WarrantyReturnPart> FindReturnParts(string partCode, string fromDate, string toDate, string dealerCode, string status, string area)
        {
            DateTime dtFrom = DataFormat.DateFromString(fromDate);
            DateTime dtTo = DataFormat.DateFromString(toDate);
            if (dtTo == DateTime.MinValue) dtTo = DateTime.MaxValue;

            PartDataContext dc = DCFactory.GetDataContext<PartDataContext>();
            Random ran = new Random();
            return dc.PartInfos.Where(p => p.DealerCode.Contains(dealerCode))
                .Select(p => new WarrantyReturnPart()
                    {
                        PartCode = p.PartCode,
                        Quantity = ran.Next(200)
                    }).ToList();
        }
    }
}