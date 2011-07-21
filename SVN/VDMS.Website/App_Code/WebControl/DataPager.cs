using System;
using System.Web.UI.WebControls;
using Resources;
using System.Web;
using VDMS.Helper;
using System.Web.UI;

namespace VDMS.II.WebControls
{
    public class DataPager : System.Web.UI.WebControls.DataPager
    {
        public string DealerCode { get; set; }
        public bool DisablePaging { get; set; }
        public string InfoCssClass { get; set; }
        public string WrapCssClass { get; set; }
        /// <summary>
        /// ValidationGroup when changing page
        /// </summary>
        public string PageValidationGroup { get; set; }

        public bool IsTesting()
        {
            return false;
            //string url = HttpContext.Current.Request.Url.ToString().ToLower();
            //return (url.Contains("test")) || (url.Contains("local")) && (url.Contains("business.aspx"));
        }
        void LogMessage(string s)
        {
            FileHelper.WriteLineToFileText("SelectOrder.log", string.Concat(DateTime.Now, ": ", s), true);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Fields.Count == 0)
            {
                this.Fields.Add(new NumericPagerField() { ButtonCount = 10 });
            }
        }

        protected override void OnTotalRowCountAvailable(object sender, PageEventArgs e)
        {
            base.OnTotalRowCountAvailable(sender, e);
        }
        void SetValidationGroup(Control c, string vg)
        {
            var b = c as IButtonControl;
            if (b != null && !(b is DataPagerFieldItem))
            {
                b.ValidationGroup = vg;
                b.CausesValidation = true;
            }
            foreach (Control cc in c.Controls)
            {
                SetValidationGroup(cc, vg);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            //if (TotalRowCount <= PageSize) Visible = false;
            if (!DisablePaging)
            {
                if (IsTesting()) LogMessage(string.Format("TotalRowCount = {0},PageSize = {1}", this.TotalRowCount, this.PageSize));
                if (!string.IsNullOrEmpty(PageValidationGroup))
                {
                    SetValidationGroup(this, PageValidationGroup);
                }
            }
            base.OnPreRender(e);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!DisablePaging)
            {
                string text = string.Format(Message.PagingInfo, (this.StartRowIndex / this.PageSize) + 1, (this.TotalRowCount / this.PageSize) + ((this.TotalRowCount % this.PageSize) > 0 ? 1 : 0), this.TotalRowCount);
                if (IsTesting()) LogMessage(string.Format("Page = {0}", text));

                writer.Write(string.Format("<table class=\"{0}\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%\">", WrapCssClass));
                writer.Write("  <tr>");
                writer.Write("      <td>");
                base.Render(writer);
                writer.Write("      </td>");
                writer.Write(string.Format("      <td class=\"{0}\">", this.InfoCssClass));
                writer.Write(text);
                writer.Write("      </td>");
                writer.Write("  </tr>");
                writer.Write("</table>");
            }
        }
    }
}