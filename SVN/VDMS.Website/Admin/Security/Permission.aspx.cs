using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.Security;
using VDMS.Provider;

public partial class Admin_Security_Permission : BasePage
{
    string QueryDealer
    {
        get
        {
            string dc = Request.QueryString["d"];
            return (string.IsNullOrEmpty(dc)) ? UserHelper.DealerCode : dc.Trim();
        }
    }

    string _queryRole;
    string QueryRole
    {
        get
        {
            if (string.IsNullOrEmpty(_queryRole))
            {
                string dc = Request.QueryString["r"];
                if (!string.IsNullOrEmpty(dc))
                {
                    decimal id;
                    decimal.TryParse(dc, out id);
                    //Role role = RoleDAO.GetRole(id, QueryDealer);
                    Role role = RoleDAO.GetRole(id);
                    _queryRole = (role == null) ? "Administrators" : role.RoleName;
                }
                else _queryRole = "Administrators";
            }
            return _queryRole;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitErrMsgControl(msg);
        InitInfoMsgControl(msg);
        if (!Page.IsPostBack)
        {
            txtTargetCode.Text = this.QueryDealer;
            txtTargetCode.ReadOnly = UserHelper.IsDealer;
            btnSavePath.Visible = UserHelper.IsSysAdmin;
            siteMapDS.SiteMapProvider = string.Concat(VDMSProvider.Language, "SecuritySiteMap");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtTargetCode.Text) || string.IsNullOrEmpty(ddlRoles.SelectedValue)) return;
            PermissionDAO.DeleteAllRolePath(txtTargetCode.Text, ddlRoles.SelectedValue);
            PermissionDAO.SaveRolePath(txtTargetCode.Text, ddlRoles.SelectedValue, tvRole.Nodes[0]);
            PermissionDAO.SaveChanged();
            AddInfoMsg(Resources.Message.ActionSucessful);
        }
        catch (Exception ex) { AddErrorMsg(ex.Message); }
    }

    protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        tvRole.DataBind();
    }

    protected void tvRole_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
        SiteMapNode node = e.Node.DataItem as SiteMapNode;
		if (Security.IsDeniedURL(node.Url, txtTargetCode.Text)) e.Node.ShowCheckBox = false;
        if (string.IsNullOrEmpty(node.Url))
        {
            e.Node.ShowCheckBox = false;
            e.Node.SelectAction = TreeNodeSelectAction.Expand;
            e.Node.Value = "";
        }
        else
        {
            e.Node.Checked = !Security.IsDeniedURL(node.Url, txtTargetCode.Text, ddlRoles.SelectedValue);
            e.Node.NavigateUrl = string.Format("TaskSetting.aspx?url={0}&app={1}&role={2}&&TB_iframe=true&height=250&width=510", node.Url, txtTargetCode.Text, ddlRoles.SelectedValue);
            //

            e.Node.Value = node.Url;
            e.Node.SelectAction = TreeNodeSelectAction.Select;
        }
    }

    // save path list to app_site_map
    protected void btnSavePath_Click(object sender, EventArgs e)
    {
        try
        {
            PermissionDAO.SaveSitePath(tvRole.Nodes[0]);
            PermissionDAO.SaveChanged();
            AddInfoMsg(Resources.Message.ActionSucessful);
        }
        catch (Exception ex) { AddErrorMsg(ex.Message); }
    }

    protected void ddlRoles_DataBound(object sender, EventArgs e)
    {
        ListItem item = ddlRoles.Items.FindByValue(this.QueryRole);
        if (item != null)
        {
            ddlRoles.SelectedValue = item.Value;
        }

        // parent can set permisson for admin only.
        if (QueryDealer != UserHelper.DealerCode)
        {
            ddlRoles.Enabled = false;
            if (ddlRoles.SelectedValue != "Administrators")
            {
                ddlRoles.Items.Clear();
            }
        }
    }
}

