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

namespace VDMS.II.WebControls
{
    public class TransactionList : DropDownList
    {
        public bool ShowSelectAllItem { get; set; } 

       

        public override void DataBind()
        {

            base.DataBind();
        }
    }
}