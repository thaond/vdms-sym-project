using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using VDMS.Data.TipTop;
using VDMS.Helper;
using System.Collections.Generic;

/// <summary>
/// Summary description for IFShipping
/// </summary>
public class IFShipping
{
    static void LogMessage(string s)
    {
        FileHelper.WriteLineToFileText("GetShippingData.log", string.Concat(DateTime.Now, ": ", s), true);
    }

    public static void RefreshShipping(string issueNo, string dCode)
    {
        LogMessage(string.Format("Start Update shipping by {0} for [{1} | {2}] --------", UserHelper.Username, issueNo, dCode));
        RefreshShipping(Shipping.GetShippingHeader2(issueNo, dCode), string.Empty, 0);
    }

    public static void RefreshShipping(string TTorderNo, long oid)
    {
        var sh = Shipping.GetShippingHeaders(TTorderNo, "%");
        var db = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.I.Linq.VehicleDataContext>();
        var oh = db.OrderHeaders.SingleOrDefault(t => t.OrderHeaderId == oid);
        if (oh != null)
        {
            LogMessage(string.Format("Start Update shipping by {0} for [{1} | {2}] --------", UserHelper.Username, oid, TTorderNo));
            RefreshShipping(sh, TTorderNo, oid);
        }
        else
        {
            if (oh == null) LogMessage(string.Format("Cannot find VDMS order: {0}", oid));
        }
    }

    public static void RefreshShipping(DataTable sh, string TTorderNo, long oid)
    {
        if ((sh != null) && (sh.Rows.Count > 0))
        {
            var db = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.I.Linq.VehicleDataContext>();
            // delete old data
            List<string> li = new List<string>();
            foreach (DataRow row in sh.Rows)
            {
                li.Add((string)row["ShippingNumber"]);
            }
            if (li.Count > 0)
            {
                //db.IShippingHeaders.DeleteAllOnSubmit(db.IShippingHeaders.Where(t => li.Contains(t.IssueNumber) && t.DealerCode == oh.DealerCode && t.BranchCode == oh.SecondaryShippingCode));
                db.IShippingHeaders.DeleteAllOnSubmit(db.IShippingHeaders.Where(t => li.Contains(t.IssueNumber)));
                db.IShippingDetails.DeleteAllOnSubmit(db.IShippingDetails.Where(t => li.Contains(t.IssueNumber)));
                db.SubmitChanges();
            }

            // insert new data
            foreach (DataRow row in sh.Rows)
            {
                var sd = Shipping.GetShippingDetail((string)row["ShippingNumber"]);

                LogMessage(string.Format("Update issue: {0} with {1} items", row["ShippingNumber"], sd.Tables[0].Rows.Count));

                var h = new VDMS.I.Entity.IShippingHeader();
                h.IssueNumber = (string)row["ShippingNumber"];
                h.ShippingDate = (DateTime)row["ShippingDate"];
                h.DealerCode = (string)row["DealerCode"];
                h.BranchCode = (string)row["BranchCode"];   //oh.SecondaryShippingCode;     
                h.TotalQuantity = (sd != null) ? (int?)sd.Tables[0].Rows.Count : null;

                db.IShippingHeaders.InsertOnSubmit(h);

                if (h.TotalQuantity.HasValue && h.TotalQuantity > 0)
                {
                    List<VDMS.I.Entity.IShippingDetail> ld = new List<VDMS.I.Entity.IShippingDetail>();
                    foreach (DataRow drow in sd.Tables[0].Rows)
                    {
                        var d = new VDMS.I.Entity.IShippingDetail();
                        d.IssueNumber = h.IssueNumber;
                        d.ColorCode = (string)drow["ColorCode"];
                        d.ColorName = (string)drow["ColorName"];
                        d.EngineNumber = (string)drow["EngineNumber"];
                        //d.Exception = (string)drow[""];
                        d.ItemCode = (string)drow["ItemCode"];
                        d.ItemName = (string)drow["ItemName"];
                        d.Model = (string)drow["Model"];
                        d.OutStockDate = (DateTime)drow["OutStockDate"];
                        d.Price = (long)(decimal)drow["Price"];
                        //d.Status= (string)drow[""];
                        d.TipTopOrderNumber = (string)drow["TipTopOrderNumber"];
                        var o = db.OrderHeaders.SingleOrDefault(t => t.OrderNumber == d.TipTopOrderNumber && t.Status == 2);
                        d.VDMSOrderId = o == null ? (TTorderNo == d.TipTopOrderNumber ? oid : 0) : o.OrderHeaderId;
                        d.BranchCode = o == null ? null : o.SecondaryShippingCode;
                        ld.Add(d);
                    }
                    db.IShippingDetails.InsertAllOnSubmit(ld);
                }
            }
            db.SubmitChanges();
        }
        else
        {
            if ((sh == null) || (sh.Rows.Count <= 0)) LogMessage(string.Format("Cannot find any issues for TipTop order: {0}", TTorderNo));
        }
        LogMessage("Finished ----------------");
        LogMessage(" ");
    }
}
