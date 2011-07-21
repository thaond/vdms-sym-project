using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using VDMS.II.Security;

public partial class Admin_Security_ManageUser : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    void InitCommand()
    {
        txtDealerCode.Text = ddlDealer.SelectedValue;
        cmdAddNew.OnClientClick = string.Format("javascript:location.href='EditUser.aspx?app={0}'; return false;", ddlDealer.SelectedValue);
    }

    protected void cmdQuery_Click(object sender, EventArgs e)
    {

        gvwUsers.DataBind();

    }

    protected void gvwUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string userName = gvwUsers.DataKeys[e.RowIndex].Value.ToString();
        ProfileDAO.Delete(userName, ddlDealer.SelectedValue);
        Membership.DeleteUser(userName);
    }

    protected void gvwUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            MembershipUser user = e.Row.DataItem as MembershipUser;
            if (user.UserName.ToLower() == "admin") e.Row.Cells[6].Text = string.Empty;
            else
            {
                LinkButton btn = e.Row.Cells[6].Controls[0] as LinkButton;
                btn.OnClientClick = DeleteConfirmQuestion;
            }
            if (!string.IsNullOrEmpty(ddlDealer.SelectedValue))
                ((HyperLink)e.Row.Cells[5].Controls[0]).NavigateUrl += "&app=" + ddlDealer.SelectedValue;
        }
    }

    protected void ddlDealer_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitCommand();
    }

    protected void ddlDealer_DataBound(object sender, EventArgs e)
    {
        InitCommand();
    }
}
