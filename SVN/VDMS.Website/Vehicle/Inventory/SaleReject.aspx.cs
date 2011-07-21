using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;
using VDMS.I.Vehicle;
using Resources;
using VDMS.Helper;

public partial class Vehicle_Inventory_SaleReject : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(DateTime.Now.ToString("ddMMyyyy"));
        bllMsg.Items.Clear();
    }

    protected bool EvalEnableConfirm(object oStatus)
    {
        bool res = true;
        try
        {
            int status = int.Parse(oStatus.ToString());
            switch (status)
            {
                case (int)ReturnItemStatus.Proposed:
                    res = true;
                    break;
                case (int)ReturnItemStatus.Allowed:
                    res = false;
                    break;
                case (int)ReturnItemStatus.NotAllow:
                    res = true;
                    break;
                case (int)ReturnItemStatus.Returned:
                    res = false;
                    break;
                case (int)ReturnItemStatus.DealerCanceled:
                    res = true;
                    break;
                default:
                    break;
            }
        }
        catch { }
        return res;
    }
    protected string EvalStatus(object status)
    {
        int st = int.Parse(status.ToString());
        string _str = string.Empty;
        switch (st)
        {
            case (int)ReturnItemStatus.Proposed:
                _str = "/Images/cmd/wait.png";
                break;
            case (int)ReturnItemStatus.Allowed:
                _str = "/Images/cmd/confirm.png";
                break;
            case (int)ReturnItemStatus.NotAllow:
                _str = "/Images/cmd/deny.png";
                break;
            case (int)ReturnItemStatus.Returned:
                _str = "/Images/cmd/bike.png";
                break;
            case (int)ReturnItemStatus.DealerCanceled:
                _str = "/Images/cmd/cancel.png";
                break;
        }
        return _str;
    }
    protected bool EvalEnableDelConfirm(object oStatus)
    {
        bool res = true;
        try
        {
            int status = int.Parse(oStatus.ToString());
            switch (status)
            {
                case (int)ReturnItemStatus.Proposed:
                    res = true;
                    break;
                case (int)ReturnItemStatus.Allowed:
                    res = true;
                    break;
                case (int)ReturnItemStatus.NotAllow:
                    res = false;
                    break;
                case (int)ReturnItemStatus.Returned:
                    res = false;
                    break;
                case (int)ReturnItemStatus.DealerCanceled:
                    res = true;
                    break;
                default:
                    break;
            }
        }
        catch { }
        return res;
    }

    protected bool EvalReadOnlyVMEPComment(object oStatus)
    {
        bool res = false;
        try
        {
            int status = int.Parse(oStatus.ToString());
            switch (status)
            {
                case (int)ReturnItemStatus.Proposed:
                    res = false;
                    break;
                case (int)ReturnItemStatus.Allowed:
                    res = false;
                    break;
                case (int)ReturnItemStatus.NotAllow:
                    res = false;
                    break;
                case (int)ReturnItemStatus.Returned:
                    res = true;
                    break;
                default:
                    break;
            }
        }
        catch { }
        return res;
    }

    protected bool EvalReadOnlyReturnNumber(object oStatus)
    {
        bool res = false;
        try
        {
            int status = int.Parse(oStatus.ToString());
            switch (status)
            {
                case (int)ReturnItemStatus.Proposed:
                    res = false;
                    break;
                case (int)ReturnItemStatus.Allowed:
                    res = false;
                    break;
                case (int)ReturnItemStatus.NotAllow:
                    res = false;
                    break;
                case (int)ReturnItemStatus.Returned:
                    res = true;
                    break;
                default:
                    break;
            }
        }
        catch { }
        return res;
    }

    //protected string EvalDate(object objDate)
    //{
    //    string res = string.Empty;
    //    try
    //    {
    //        DateTime dtDate = (DateTime)objDate;
    //        res = dtDate.ToShortDateString();
    //    }
    //    catch { }
    //    return res;
    //}
    protected void btnTest_Click(object sender, EventArgs e)
    {
    }
    //protected void grdSaleReject_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        Label lblReturnItemId = grdSaleReject.Rows[e.RowIndex].FindControl("lblReturnItemId") as Label;
    //        TextBox txtVMEPComment = grdSaleReject.Rows[e.RowIndex].FindControl("txtVMEPComment") as TextBox;
    //        TextBox txtReturnNumber = grdSaleReject.Rows[e.RowIndex].FindControl("txtReturnNumber") as TextBox;
    //        if (txtReturnNumber.Text.Trim() != string.Empty)
    //        {
    //            Returnitem rt = SaleRejectHelper.GetReturnitemById(long.Parse(lblReturnItemId.Text));
    //            // Dieu kien xac nhan o day
    //            rt.Status = (int)ReturnItemStatus.Allowed;
    //            rt.Vmepcomment = txtVMEPComment.Text.Trim();
    //            rt.Returnnumber = txtReturnNumber.Text.Trim();
    //            using (TransactionBlock tran = new TransactionBlock())
    //            {
    //                IDao<Returnitem, long> dao = DaoFactory.GetDao<Returnitem, long>();
    //                dao.SaveOrUpdate(rt);
    //                tran.IsValid = true;
    //            }

    //            BindData();
    //        }
    //        else
    //        {
    //            bllMsg.Items.Add(Resources.Constants.ReturnNumberInvalid);
    //        }
    //    }
    //    catch { }
    //    TextBox txtReturnNumber = grdSaleReject.Rows[e.RowIndex].FindControl("txtReturnNumber") as TextBox;
    //    if (string.IsNullOrEmpty(txtReturnNumber.Text.Trim())) bllMsg.Items.Add(Resources.Constants.ReturnNumberInvalid);
    //}

    //protected void grdSaleReject_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        Label lblReturnItemId = grdSaleReject.Rows[e.RowIndex].FindControl("lblReturnItemId") as Label;
    //        TextBox txtVMEPComment = grdSaleReject.Rows[e.RowIndex].FindControl("txtVMEPComment") as TextBox;
    //        TextBox txtReturnNumber = grdSaleReject.Rows[e.RowIndex].FindControl("txtReturnNumber") as TextBox;

    //        Returnitem rt = SaleRejectHelper.GetReturnitemById(long.Parse(lblReturnItemId.Text));
    //        // Dieu kien huy xac nhan o day
    //        rt.Status = (int)ReturnItemStatus.NotAllow;
    //        rt.Vmepcomment = txtVMEPComment.Text;
    //        rt.Returnnumber = string.Empty; // So phieu xac nhan xoa trang
    //        using (TransactionBlock tran = new TransactionBlock())
    //        {
    //            SaleRejectHelper.UpdateReturnitem(rt);
    //            tran.IsValid = true;
    //        }

    //        BindData();
    //    }
    //    catch { }
    //}

    protected void grdSaleReject_DataBound(object sender, EventArgs e)
    {
        try
        {
            //Literal litPageInfo = grdSaleReject.TopPagerRow.FindControl("litPageInfo") as Literal;
            //if (litPageInfo != null) litPageInfo.Text = string.Format(Resources.Message.PagingInfo, grdSaleReject.PageIndex + 1, grdSaleReject.PageCount, HttpContext.Current.Items["rowCount"]);
        }
        catch { }
    }

    protected void grdSaleReject_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = (grdSaleReject.PageSize * grdSaleReject.PageIndex + e.Row.RowIndex + 1).ToString();
            DataRowView row = e.Row.DataItem as DataRowView;
            Button btn10 = e.Row.Cells[10].FindControl("btconfirm") as Button;
            Button btn11 = e.Row.Cells[10].FindControl("btreject") as Button;
            btn10.Enabled = EvalEnableConfirm(row["Status"]);
            btn10.OnClientClick = " return confirm('" + Question.SaleReject_Confirm + "');";
            btn11.Enabled = EvalEnableDelConfirm(row["Status"]);
            btn11.OnClientClick = " return confirm('" + Question.SaleReject_Cancel + "');";
        }
    }

    protected void grdSaleReject_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            int RowIndex = int.Parse((string)e.CommandArgument);
            decimal Id = (decimal)grdSaleReject.DataKeys[RowIndex].Value;
            TextBox txtVMEPComment = grdSaleReject.Rows[RowIndex].FindControl("txtVMEPComment") as TextBox;
            TextBox txtReturnNumber = grdSaleReject.Rows[RowIndex].FindControl("txtReturnNumber") as TextBox;
            IDao<Returnitem, long> dao = DaoFactory.GetDao<Returnitem, long>();
            Returnitem rt = dao.GetById((long)Id, false);

            if (string.IsNullOrEmpty(txtReturnNumber.Text.Trim()) && e.CommandName == "Confirm")
            {
                bllMsg.Items.Add(Resources.Constants.ReturnNumberInvalid);
                return;
            }

            if (InventoryHelper.IsInventoryLock(rt.Releasedate,rt.Dealercode,rt.Branchcode) && e.CommandName == "Confirm")
            {
                DateTime lockdate = InventoryHelper.GetLockedDate(rt.Dealercode,rt.Branchcode);
                bllMsg.Items.Add(string.Format(Message.InventoryLocked, lockdate.Month + "/" + lockdate.Year));
                return;
            }
            switch (e.CommandName)
            {
                case "Reject":
                    rt.Status = (int)ReturnItemStatus.NotAllow;
                    rt.Vmepcomment = txtVMEPComment.Text.Trim();
                    rt.Returnnumber = string.Empty;
                    break;
                case "Confirm":
                    rt.Status = (int)ReturnItemStatus.Allowed;
                    rt.Vmepcomment = txtVMEPComment.Text.Trim();
                    rt.Returnnumber = txtReturnNumber.Text.Trim();
                    break;
            }
            dao.SaveOrUpdate(rt);
            ReLoad();
        }
    }
}
