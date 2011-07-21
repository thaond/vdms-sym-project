using System.ComponentModel;
using System.Linq;
using VDMS.Helper;
using VDMS.II.Linq;
using System.Collections.Generic;

namespace VDMS.II.PartManagement.Order
{
    [DataObject(true)]
    public class PartOrder : PartData
    {
        public long OrderDetailId { get; set; }
        public int Amount { get { return Quantity * UnitPrice; } }
        public bool ChangedForPacking { get; set; }
    }

    public class PartOrderDAO : SessionPartDAO<PartOrder>
    {
        public static void Init()
        {
            key = "PartOrder_List";
        }
        public static void InitPrice()
        {
            Parts.ForEach(p => { p.PartInfoId = -1; p.PartType = "P"; });
        }
        public static void Merge()
        {
            var r = from m in Parts
                    group m by m.PartCode into g
                    select new PartOrder()
                    {
                        Quantity = g.Sum(m => m.Quantity),
                        PartCode = g.ElementAt(0).PartCode,
                        OrderDetailId = g.Max(m => m.OrderDetailId)
                    };
            var t = new List<PartOrder>();
            t.AddRange(r);
            Parts.Clear();
            Parts.AddRange(t);
        }

        public static void LoadFromDB(long OrderId)
        {
            var list = new List<PartOrder>();// Parts;

            //list.Clear();
            var db = DCFactory.GetDataContext<PartDataContext>();
            var query = from d in db.OrderDetails
                        join p in db.Parts on d.PartCode equals p.PartCode
                        where d.OrderHeaderId == OrderId && p.DatabaseCode == UserHelper.DatabaseCode
                        orderby d.LineNumber
                        select new { d, p };
            foreach (var item in query)
            {
                var tmp = new PartOrder
                {   
                    PartCode = item.d.PartCode,
                    Line = item.d.LineNumber,
                    Quantity = item.d.OrderQuantity,
                    UnitPrice = item.d.UnitPrice,
                    PartName = UserHelper.IsVietnamLanguage ? item.p.VietnamName : item.p.EnglishName,
                    OrderDetailId = item.d.OrderDetailId,
                    OriginalQty = item.d.OriginalQty,
                    Quo_Status = item.d.Quo_Status,
                    QuotationQuantity = item.d.QuotationQuantity,
                    Status =item.p.Status,
                    DelivaryQuantity = item.d.DelivaryQuantity
                };
                var op = Parts.SingleOrDefault(p => p.PartCode == item.d.PartCode);
                if (op != null) tmp.ChangedForPacking = op.ChangedForPacking;

                list.Add(tmp);
            }

            Parts.Clear();
            Parts.AddRange(list);
        }
    }
}