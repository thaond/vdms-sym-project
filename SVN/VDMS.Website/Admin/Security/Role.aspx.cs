using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.Security;

public partial class Admin_Security_Role : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        InitErrMsgControl(msg);
        InitInfoMsgControl(msg);

        if (!Page.IsPostBack)
        {
            BindDealersList(UserHelper.DealerCode);
            BindRolesList(tvDealer.SelectedValue);
            btnDeleteRole.OnClientClick = DeleteConfirmQuestion;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        UpdateState();
    }

    void BindRolesList(string app)
    {
        XElement element = new XElement("Data");
        element.Add(CreateRoleXmlTree(0, app));
        xdsRoles.Data = element.ToString();
        xdsRoles.CacheKeyDependency = xdsRoles.Data;
        tvRoles.DataBind();
        BindRoleData();
    }

    void BindDealersList(string code)
    {
        int maxLevel = UserHelper.IsSysAdmin ? int.MaxValue : 1;
        XElement element = new XElement("Data");
        if (string.IsNullOrEmpty(code))
        {
            XElement vmep = new XElement("Dealer", new XElement("Name", "VMEP"), new XElement("Code", "/"));
            foreach (Dealer dealer in DealerDAO.GetTopDealers())
                vmep.Add(CreateDealerXmlTree(dealer.DealerCode, maxLevel, 0));

            element.Add(vmep);
        }
        else
        {
            element.Add(CreateDealerXmlTree(code, maxLevel, 0));
        }
        xmlDataSource.Data = element.ToString();
        xmlDataSource.CacheKeyDependency = xmlDataSource.Data;
        tvDealer.DataBind();
    }

    void BindRoleData()
    {
        if (tvRoles.SelectedNode != null)
        {
            string RoleName = tvRoles.SelectedNode.Text;
            cblUserInRole.DataSource = RoleDAO.GetUsersInRole(tvDealer.SelectedValue, RoleName);
            cblUserInRole.DataBind();
            txtCurrentRole.Text = RoleName;
            foreach (ListItem obj in cblUserInRole.Items) obj.Selected = true;
        }
    }

    static XElement CreateDealerXmlTree(string code, int maxLevel, int curLevel)
    {
        if (curLevel > maxLevel) return null;
        PartDataContext db = DCFactory.GetDataContext<PartDataContext>();
        var curr = db.Dealers.SingleOrDefault(p => p.DealerCode == code);
        if (curr == null) return null;

        var childs = db.Dealers.Where(p => p.ParentCode == curr.DealerCode).ToList();
        return new XElement("Dealer",
            new XElement("Name", curr.DealerName), new XElement("Code", curr.DealerCode),
              (childs.Count > 0) ?
                (from d in childs select CreateDealerXmlTree(d.DealerCode, maxLevel, curLevel + 1)).ToArray()
                : null
        );
    }

    static XElement CreateRoleXmlTree(decimal curr, string app)
    {

        Role role = null;
        role = (curr <= 0) ? RoleDAO.GetRootRoles(app) : role = RoleDAO.GetRole(curr, app);

        if (role == null) return null;

        return new XElement("Role",
            new XElement("Name", role.RoleName), new XElement("Code", role.RoleIndex),
            from d in RoleDAO.GetRoles(app).Where(p => (p.ParentRoleIndex == role.RoleIndex) && (p.ParentRoleIndex != p.RoleIndex))
            select CreateRoleXmlTree(d.RoleIndex, app)
        );
    }

    protected void UpdateState()
    {
        pnRoles.Visible = tvDealer.SelectedNode != null;
        phUserInRole.Visible = tvRoles.SelectedNode != null;
        pnCreateRole.Visible = tvRoles.SelectedNode != null;

        btnDeleteRole.Enabled = (tvRoles.SelectedNode != null) && (tvRoles.SelectedNode.ChildNodes.Count == 0) && (cblUserInRole.Items.Count == 0);
        btnSetPermission.Enabled = tvDealer.SelectedNode != null;
        btnSetPermission.OnClientClick = string.Format("window.open('/admin/security/Permission.aspx?d={0}&r={1}', 'setPermission{0}'); return false;", tvDealer.SelectedValue, tvRoles.SelectedValue);
        cmdUpdate.Enabled = cblUserInRole.Items.Count > 0;
        btnCreateAdmin.Visible = tvRoles.Nodes.Count <= 0;
        btnCreateAdmin.Enabled = tvDealer.SelectedNode != null;
        btnDeleteRole.Visible = tvRoles.Nodes.Count > 0;
    }

    protected void cvRole_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (tvDealer.SelectedNode == null) || !RoleDAO.RoleExists(tvDealer.SelectedValue, args.Value);
    }

    protected void cmdCreateRole_Click(object sender, EventArgs e)
    {
        try
        {
            long pId = long.Parse(tvRoles.SelectedNode.Value);
            if (!Page.IsValid || string.IsNullOrEmpty(tvDealer.SelectedValue) || (pId <= 0)) return;

            string roleName = txtRole.Text.Trim();
            if (RoleDAO.CreateRole(tvDealer.SelectedValue, roleName, pId) != null)
            {
                BindRolesList(tvDealer.SelectedValue);
                txtRole.Text = string.Empty;
                //AddInfoMsg(Resources.Message.ActionSucessful);
            }
            else AddErrorMsg(Resources.Message.GeneralError);
        }
        catch (Exception ex) { AddErrorMsg(ex.Message); }
    }

    protected void btnDeleteRole_Click(object sender, EventArgs e)
    {
        try
        {
            RoleDAO.DeleteRole(tvDealer.SelectedValue, tvRoles.SelectedNode.Text);
            BindRolesList(tvDealer.SelectedValue);
        }
        catch
        {
            AddErrorMsg(Resources.Message.Security_ErrorDeleteRole);
        }
    }

    // update users in role
    protected void cmdUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if ((tvRoles.SelectedNode != null) && (tvDealer.SelectedNode != null))
            {
                foreach (ListItem obj in cblUserInRole.Items)
                {
                    if (!obj.Selected)
                    {
                        RoleDAO.RemoveUserFromRole(tvDealer.SelectedValue, obj.Text, txtCurrentRole.Text);
                    }
                }
                BindRolesList(tvDealer.SelectedValue);
            }
        }
        catch (Exception ex) { AddErrorMsg(ex.Message); }
    }

    protected void tvRoles_SelectedNodeChanged(object sender, EventArgs e)
    {
        BindRoleData();
    }

    protected void tvRoles_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
        e.Node.Value = (e.Node.DataItem as SiteMapNode).Url;
        e.Node.NavigateUrl = string.Empty;
        //e.Node.se
    }

    protected void tvDealer_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
        //e.Node.Value = (e.Node.DataItem as XElement).Url;
        e.Node.NavigateUrl = string.Empty;
    }

    protected void tvDealer_SelectedNodeChanged(object sender, EventArgs e)
    {
        txtdealercode.Text = tvDealer.SelectedValue;
        BindRolesList(txtdealercode.Text);
    }

    protected void tvRoles_DataBound(object sender, EventArgs e)
    {
        //UpdateState();
    }

    protected void btnCreateAdmin_Click(object sender, EventArgs e)
    {
        try
        {
            if (RoleDAO.CreateAdminRole(tvDealer.SelectedValue) == null)
            {
                AddErrorMsg(Resources.Message.GeneralError);
            }
            else
            {
                BindRolesList(tvDealer.SelectedValue);
                txtRole.Text = string.Empty;
                AddInfoMsg(Resources.Message.ActionSucessful);
            }
        }
        catch (Exception ex) { AddErrorMsg(ex.Message); }
    }
    protected void btDealerCode_Click(object sender, EventArgs e)
    {
        BindRolesList(txtdealercode.Text);
    }
}
