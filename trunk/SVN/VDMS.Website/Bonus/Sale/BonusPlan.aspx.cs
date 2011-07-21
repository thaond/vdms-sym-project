using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.BonusSystem;
using VDMS.Common.Utils;
using VDMS.II.BasicData;
using Resources;
using VDMS.II.Entity;
using VDMS.II.WebControls;
using VDMS.Bonus.Entity;
using VDMS.Helper;

public partial class Bonus_Sale_BonusPlan : BasePage
{
    public long PlanId
    {
        get { return string.IsNullOrEmpty(Request.QueryString["id"]) ? 0 : long.Parse(Request.QueryString["id"]); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitErrMsgControl(error);
        if (!IsPostBack)
        {
            LoadPlan();
        }
    }

    protected string EvalBSourceName(long bsId)
    {
        var o = BonusPlans.DC.BonusSources.SingleOrDefault(p => p.BonusSourceId == bsId);
        return o == null ? "" : o.BonusSourceName;
    }

    private bool CheckError()
    {
        bool ok = true;
        BonusPlans.CleanEditingItems(PlanId);
        var items = BonusPlans.GetEditingItems(PlanId);
        foreach (var item in items)
        {
            var d = DealerDAO.GetDealerByCode(item.DealerCode);
            if (!string.IsNullOrEmpty(item.DealerCode) && (d == null
                // sale man chi quan ly vung cua minh
                    || (d.AreaCode != UserHelper.AreaCode && UserHelper.Profile.Position == PositionType.Employee && !UserHelper.IsSysAdmin)
                // con lai admin - ADM: theo DB
                    || d.DatabaseCode != UserHelper.DatabaseCode
                  ))
            {
                AddErrorMsg(string.Format(Errors.NotExistDealerCode, item.DealerCode));
                ok = false;
            }
            if (item.BonusDate == null || DataFormat.DateFromString(txtPlanMonth.Text) == DateTime.MinValue)
            {
                AddErrorMsg(Errors.LackOfDate);
                ok = false;
            }
        }
        return ok;
    }
    private void UpdateRow(GridViewRow row)
    {
        var d = BonusPlans.GetEditingItems(PlanId).SingleOrDefault(p => p.Id == (Guid)gvItems.DataKeys[row.RowIndex].Value);
        if (d != null)
        {
            string dc = (row.FindControl("txtDealerCode") as TextBox).Text.Trim().ToUpper();
            string am = (row.FindControl("txtAmount") as TextBox).Text;
            string ds = (row.FindControl("txtDesc") as TextBox).Text.Trim();
            string bd = (row.FindControl("txtBonusDate") as TextBox).Text;
            string bs = (row.FindControl("dlBSource") as DropDownList).SelectedValue;
            string st = (row.FindControl("dlStatus") as DropDownList).SelectedValue;

            d.DealerCode = dc;
            d.Amount = long.Parse(am);
            d.Description = ds;
            d.Status = st;
            //d.PlanType = "V";
            d.BonusSourceId = string.IsNullOrEmpty(bs) || bs == "0" ? null : (long?)long.Parse(bs);
            d.BonusDate = DataFormat.DateFromString(bd);
        }
    }

    private void LoadPlan()
    {
        ClearForm();
        var plan = BonusPlans.LoadPlan(PlanId);
        if (plan != null)
        {
            txtDesc.Text = plan.Description;
            txtPlanMonth.Text = plan.FromDate.ToString("MM/yyyy");
            txtPlanName.Text = plan.BonusPlanName;
            if (plan.Status == BonusPlanStatus.Confirmed) ReadOnlyMode(); else ViewMode();
        }
        else
        {
            //gvItems.ShowEmptyFooter = PlanId == 0;
            EditMode();
        }
        gvItems.DataBind();
    }
    private void ClearForm()
    {
        txtDesc.Text =
        txtPlanMonth.Text =
        txtPlanName.Text = "";
    }

    private void ReadOnlyMode()
    {
        btnEnd.Visible = false;
        btnSave.Visible = false;
        btnEdit.Visible = false;
        phEdit.Visible = false;
        gvViewItems.Visible = true;
        btnConfirm.Visible = false;
    }
    private void ViewMode()
    {
        btnEnd.Visible = false;
        btnSave.Visible = false;
        btnEdit.Visible = true;
        phEdit.Visible = false;
        gvViewItems.Visible = true;
        btnConfirm.Visible = BonusPlanStatus.ConfirmReadyStatus().Contains(BonusPlans.GetEditingPlan(PlanId).Status);
    }
    private void EditMode()
    {
        btnEnd.Visible = true;
        btnSave.Visible = false;
        btnEdit.Visible = false;
        phEdit.Visible = true;
        gvViewItems.Visible = false;
        btnConfirm.Visible = false;
    }
    private void FinishMode()
    {
        btnEnd.Visible = false;
        btnSave.Visible = true;
        btnEdit.Visible = true;
        phEdit.Visible = false;
        gvViewItems.Visible = true;
        btnConfirm.Visible = false;
    }

    public bool IsEditable(object status)
    {
        return !IsViewOnly(status);
    }
    public bool IsViewOnly(object status)
    {
        return status != null && ((string)status == BonusPlanStatus.Confirmed);
    }

    protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Insert":
                if (gvItems.FootRow != null)
                {
                    string dc = (gvItems.FootRow.FindControl("txtNewDealerCode") as TextBox).Text.Trim().ToUpper();
                    string am = (gvItems.FootRow.FindControl("txtNewAmount") as TextBox).Text;
                    string ds = (gvItems.FootRow.FindControl("txtNewDesc") as TextBox).Text.Trim();
                    string bd = (gvItems.FootRow.FindControl("txtNewBonusDate") as TextBox).Text;
                    string bs = (gvItems.FootRow.FindControl("dlNewBSource") as DropDownList).SelectedValue;
                    string st = (gvItems.FootRow.FindControl("dlNewStatus") as DropDownList).SelectedValue;
                    BonusPlans.AddEditingItem(PlanId, long.Parse(bs), DataFormat.DateFromString(bd), dc, st, BonusType.Vehicle, long.Parse(am), ds);
                }
                gvItems.DataBind();
                break;
        }
    }
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        //odsItems.Insert();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var p = BonusPlans.SaveEditingPlan(PlanId, txtPlanName.Text.Trim(), BonusType.Vehicle, BonusPlanStatus.Normal, txtDesc.Text.Trim(), DataFormat.DateFromString(txtPlanMonth.Text), DateTime.MinValue);
        Response.Redirect(string.Format("~/Bonus/Sale/BonusPlan.aspx?id={0}", p == null ? 0 : p.BonusPlanHeaderId));
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        EditMode();
        gvItems.DataBind();
    }
    protected void btnEnd_Click(object sender, EventArgs e)
    {
        if (CheckError())
        {
            FinishMode();
            gvViewItems.DataBind();
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        BonusPlans.CleanSessionPlan(PlanId);
        Response.Redirect("~/Bonus/Sale/BonusPlanList.aspx");
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        BonusPlans.ConfirmPlan(PlanId);
        BonusPlans.DC.SubmitChanges();
        ReadOnlyMode();
        Response.Redirect(Request.Url.ToString());
    }

    protected void lnkAddRows_Click(object sender, EventArgs e)
    {
        BonusPlans.AddEditingItem(int.Parse(txtItems.Text), PlanId, BonusType.Vehicle);
        gvItems.DataBind();
    }
    protected void ItemChanged(object sender, EventArgs e)
    {
        UpdateRow((GridViewRow)((TextBox)sender).NamingContainer);
    }
    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var i = BonusPlans.GetEditingItems(PlanId)[e.Row.RowIndex];
            var sr = e.Row.FindControl("dlBSource") as BonusSourceList;
            sr.SelectedValue = i.BonusSourceId.ToString();
            e.Row.Enabled = IsEditable(i.Status);

            //var st = e.Row.FindControl("dlStatus") as BonusSourceList;
            //st.SelectedValue = i.Status;
        }
    }
    protected void gvViewItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].CssClass = BonusPlans.IsValidBonusDate((BonusPlanDetail)e.Row.DataItem) ? "" : "errorText";
        }
    }

    protected void btnBUpload_Click(object sender, EventArgs e)
    {
        BonusPlans.CleanEditingItems();
        if (fileUpload.HasFile)
        {
            BonusPlans.ImportExcel(fileUpload.PostedFile.InputStream, VDMS.VDMSSetting.CurrentSetting.BonusSetting);
        }
        gvImport.DataSource = BonusPlans.GetAllEditingItems();
        gvImport.DataBind();

    }

    protected void btnBImport_Click(object sender, EventArgs e)
    {
        if (BonusPlans.GetAllEditingItems().Count > 0)
        {
            BonusPlans.SaveImportingBonus();
            BonusPlans.CleanEditingItems();
        }
    }
}
