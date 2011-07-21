using System;
using System.Web;

namespace VDMS.Core.Domain
{
    public partial class Item
    {
        public long GetUnitPrice(string dbCode)
        {
            if (dbCode.ToLower() == "dnf") return _dnfprice;
            return _htfprice;
        }

        public long Price
        {
            get
            {
                string AreaCode = (string)HttpContext.Current.Session["CurrentUser.DatabaseCode"];
                if (AreaCode.ToLower() == "dnf") return this._dnfprice;
                return this._htfprice;
            }
            set
            {
                string AreaCode = (string)HttpContext.Current.Session["CurrentUser.DatabaseCode"];
                if (AreaCode.ToLower() == "dnf") this._dnfprice = value;
                else this._htfprice = value;
            }
        }
    }
}
