using System;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.Provider;

public partial class login : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // show VMEP item in dealer list
            VDMSProvider.OrgCode = "\\";
        }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Request.Browser.IsMobileDevice)
        {
            pl1.Visible = false;
            this.Theme = "Mobile";
        }
        else
        {
            pl2.Visible = false;
        }

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(VDMSProvider.Language);
        if (Request.Browser.IsMobileDevice)
        {
            VDMSProvider.OrgCode = ((DropDownList)Login2.FindControl("ddl")).SelectedValue;
            if (!Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(VDMSProvider.Language))
                {
                    ((DropDownList)Login2.FindControl("Language")).DataBind();
                    ((DropDownList)Login2.FindControl("Language")).Items[1].Selected = true;
                }   
                else
                    ((DropDownList)Login2.FindControl("Language")).SelectedValue = VDMSProvider.Language;
                try
                {
                    ((DropDownList)Login2.FindControl("ddl")).SelectedValue = Response.Cookies["DealerCode"].Value;
                }
                catch
                {   
                }
            }
        }
        else
        {
            VDMSProvider.OrgCode = ((DropDownList)Login1.FindControl("ddl")).SelectedValue;
            if (!Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(VDMSProvider.Language))
                {
                    ((DropDownList)Login1.FindControl("Language")).DataBind();
                    ((DropDownList)Login1.FindControl("Language")).Items[1].Selected = true;
                }
                else
                    ((DropDownList)Login1.FindControl("Language")).SelectedValue = VDMSProvider.Language;
                try
                {   
                    ((DropDownList)Login1.FindControl("ddl")).SelectedValue = Response.Cookies["DealerCode"].Value;
                    ((DropDownList)Login1.FindControl("Language")).SelectedValue = VDMSProvider.Language;
                }
                catch
                {
                    //((DropDownList)Login1.FindControl("Language")).SelectedValue = "vi-VN";
                }
            }
        }

    }

    protected void Login1_LoggedIn(object sender, EventArgs e)
    {
        HttpContext.Current.Session["CurrentUser.Dealer"] = null;
        HttpContext.Current.Session["CurrentUser.Profile"] = null;

        // Thanh IT Dong Nai request
        if (Request.Browser.IsMobileDevice)
        {
            if (!VDMSProvider.IsDebugEnabled) Response.Redirect("~/Mobile/default.aspx");
        }
        else
        {
            if (!VDMSProvider.IsDebugEnabled) Response.Redirect("~/default.aspx");
        }


    }

    protected override void InitializeCulture()
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(VDMSProvider.Language);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(VDMSProvider.Language);
        base.InitializeCulture();
    }

    protected void Language_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.Browser.IsMobileDevice)
            VDMSProvider.Language = ((DropDownList)Login2.FindControl("Language")).SelectedValue;
        else
            VDMSProvider.Language = ((DropDownList)Login1.FindControl("Language")).SelectedValue;
        Response.Redirect(Page.Request.Url.AbsoluteUri);
    }

    protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        if (!Page.IsValid) return;
        if (Request.Browser.IsMobileDevice)
        {
            VDMSProvider.Language = ((DropDownList)Login2.FindControl("Language")).SelectedValue;
            var text = ((TextBox)Login2.FindControl("txtDealerCode")).Text.Trim();
            if (!string.IsNullOrEmpty(text)) VDMSProvider.OrgCode = text;
            else VDMSProvider.OrgCode = ((DropDownList)Login2.FindControl("ddl")).SelectedValue;
        }
        else
        {
            VDMSProvider.Language = ((DropDownList)Login1.FindControl("Language")).SelectedValue;
            var text = ((TextBox)Login1.FindControl("txtDealerCode")).Text.Trim();
            if (!string.IsNullOrEmpty(text)) VDMSProvider.OrgCode = text;
            else VDMSProvider.OrgCode = ((DropDownList)Login1.FindControl("ddl")).SelectedValue;
        }

        var cookie = new HttpCookie("DealerCode");
        cookie.Value = VDMSProvider.OrgCode;
        cookie.Expires = DateTime.Now.AddMonths(1);
        Response.Cookies.Add(cookie);
    }

    protected void cvDealerCode_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = DealerHelper.Dealers.Count(p => p.DealerCode.ToLower() == args.Value.ToLower()) == 1;
    }
    protected void UserName_TextChanged(object sender, EventArgs e)
    {
        ((TextBox)sender).Text = ((TextBox)sender).Text.Trim().ToUpper();
    }
}
