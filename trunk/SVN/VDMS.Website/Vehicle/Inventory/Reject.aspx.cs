using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NHibernate.Expression;
using Resources;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Helper;
using VDMS.I.Vehicle;

public partial class Vehicle_Inventory_Reject : BasePage
{
    private const string VS_ItemInstanceID = "ItemInstanceID";
    private const string VS_ReturnItemID = "ReturnItemID";
    private Collection<ReturnItemErrorCode> errorCode = new Collection<ReturnItemErrorCode>();

    private void ShowError()
    {
        bllErrorMsg.Items.Clear();
        foreach (ReturnItemErrorCode error in errorCode)
        {
            switch (error)
            {
                case ReturnItemErrorCode.InventoryLocked:
                    DateTime lockdate = InventoryHelper.GetLockedDate(UserHelper.DealerCode, UserHelper.BranchCode);
                    bllErrorMsg.Items.Add(string.Format(Message.InventoryLocked, lockdate.Month + "/" + lockdate.Year));
                    break;
                case ReturnItemErrorCode.CancelRequestFailed: bllErrorMsg.Items.Add(Message.Reject_CancelRequestFailed); break;
                case ReturnItemErrorCode.ItemHasOutOfStock: bllErrorMsg.Items.Add(Message.Reject_ItemHasOutOfStock); break;

                case ReturnItemErrorCode.VerhicleNotExist: bllErrorMsg.Items.Add(Message.Reject_VerhicleNotExist); break;
                case ReturnItemErrorCode.ShippingDetailError: bllErrorMsg.Items.Add(Message.Reject_ShippingDetailError); break;
                case ReturnItemErrorCode.SaveStateFailed: bllErrorMsg.Items.Add(Message.SaveStateFailed); break;
                case ReturnItemErrorCode.CanNotSendToVMEP: bllErrorMsg.Items.Add(Message.Reject_CanNotSendToVMEP); break;
                case ReturnItemErrorCode.FinishReturnFailed: bllErrorMsg.Items.Add(Message.Reject_FinishReturnFailed); break;
                case ReturnItemErrorCode.ReleaseMoreThanImport: bllErrorMsg.Items.Add(Message.ReleaseMoreThanImport); break;
                case ReturnItemErrorCode.ReleaseMoreThanNow: bllErrorMsg.Items.Add(Message.ReleaseMoreThanNow); break;
            }
        }
        bllErrorMsg.Visible = bllErrorMsg.Items.Count > 0;
    }

    protected void AddError(ReturnItemErrorCode error)
    {
        if (errorCode.Contains(error)) return;
        errorCode.Add(error);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ShowError();
        //btnCancel.Visible = false;  // hiding feature
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        errorCode.Clear();
        //lblMsg.Text = "";
        if (!IsPostBack) { ShowDetailPanel(false); }
        //if (IsChangeLanguage && pnDetail.Visible) btnCheck_Click(sender, e);

        btnReject.OnClientClick = " return confirm('" + Question.Reject_RejectVerhicle + "');";
        btnSend.OnClientClick = " return confirm('" + Question.Reject_SendToVMEP + "');";
        if (btnReSend.Visible)
        {
            btnReSend.OnClientClick = " return confirm('" + Question.Reject_ReSendToVMEP + "');";
            btdelete.OnClientClick = " return confirm('" + Question.Reject_Delete + "');";
        }

        btnCancel.OnClientClick = " return confirm('" + Question.Reject_Cancel + "');";

    }

    private void ShowDetailPanel(bool status)
    {
        pnDetail.Visible = status;
    }

    private Iteminstance GetItemInfos(string engineNumber)
    {
        IDao<Iteminstance, long> dao = DaoFactory.GetDao<Iteminstance, long>();
        //if (UserHelper.IsAdmin)
        //    dao.SetCriteria(new ICriterion[] { 
        //    Expression.Eq("Enginenumber", engineNumber.ToUpper()), 
        //    Expression.Eq("Dealercode", UserHelper.DealerCode) 
        //});
        //else
        dao.SetCriteria(new ICriterion[] { 
            Expression.Eq("Enginenumber", engineNumber.ToUpper()), 
            Expression.Eq("Branchcode", UserHelper.BranchCode), 
            Expression.Eq("Dealercode", UserHelper.DealerCode) 
        });

        IList lst = dao.GetAll();
        if (lst.Count <= 0) return null;
        return (Iteminstance)lst[0];
    }

    private Shippingdetail GetItemShippingDetail(string engineNumber)
    {
        IDao<Shippingdetail, long> dao = DaoFactory.GetDao<Shippingdetail, long>();
        dao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", engineNumber.ToUpper()), Expression.Eq("Branchcode", UserHelper.BranchCode) });
        IList lst = dao.GetAll();
        if (lst.Count <= 0) return null;
        return (Shippingdetail)lst[0];
    }

    private Returnitem GetReturnInfo(long iisId, string enginenumber)
    {
        IDao<Returnitem, long> dao = DaoFactory.GetDao<Returnitem, long>();
        dao.SetCriteria(new ICriterion[] { Expression.Eq("Iteminstance.Id", iisId), Expression.Eq("Branchcode", UserHelper.BranchCode), Expression.Eq("Dealercode", UserHelper.BranchCode) });
        //dao.SetCriteria(new ICriterion[] { Expression.Eq("Iteminstance.Id", iisId) });
        IList lst = dao.GetAll();
        if (lst.Count <= 0) return null;
        return (Returnitem)lst[0];
    }

    private bool SaveReturnPropose(long iisID, string reason, int returnid)
    {
        IDao<Returnitem, long> dao = DaoFactory.GetDao<Returnitem, long>();


        Returnitem ri = new Returnitem();

        #region Fix
        //ri.Iteminstanceid = iisID; // nmchi
        IDao<Iteminstance, long> daoItemIns = DaoFactory.GetDao<Iteminstance, long>();
        ri.Iteminstance = daoItemIns.GetById(iisID, false); // Fix: dungnt 04/12/2007 (Do gen lai Returnitem)
        #endregion
        if (returnid > 0)
            ri.Id = returnid;
        ri.Returnreason = reason;
        ri.Returnnumber = "";
        ri.Vmepcomment = "";
        ri.Status = (int)ReturnItemStatus.Proposed;
        ri.Releasedate = Convert.ToDateTime(txtReleasedate.Text);
        ri.Confirmdate = DateTime.Now;
        ri.Dealercode = UserHelper.DealerCode;
        ri.Branchcode = UserHelper.BranchCode;
        try
        {
            dao.SaveOrUpdate(ri);
            return true;
        }
        catch { return false; }
    }

    private bool SaveReturnFinish(string engineNumber, string branchCode)
    {
        using (TransactionBlock trans = new TransactionBlock())
        {
            IDao<Iteminstance, long> iisDao = DaoFactory.GetDao<Iteminstance, long>();
            IDao<Returnitem, long> riDao = DaoFactory.GetDao<Returnitem, long>();
            IDao<TransHis, long> trhDao = DaoFactory.GetDao<TransHis, long>();
            IList lst;
            try
            {
                // iteminstance record
                iisDao.SetCriteria(new ICriterion[] { Expression.Eq("Enginenumber", engineNumber.ToUpper()) });
                lst = iisDao.GetAll();
                if (lst.Count <= 0) return false;
                Iteminstance iis = (Iteminstance)lst[0];

                // check for item instock?
                if (!ItemHepler.IsInstock(iis.Status))
                {
                    AddError(ReturnItemErrorCode.ItemHasOutOfStock);
                    return false;
                }

                // 
                iis.Status = (int)ItemStatus.Return;
                //iis.Releaseddate = DateTime.Now;


                // returnItem record
                riDao.SetCriteria(new ICriterion[] { Expression.Eq("Iteminstance.Id", iis.Id), Expression.Eq("Status", (int)ReturnItemStatus.Allowed), Expression.Eq("Dealercode", UserHelper.DealerCode), Expression.Eq("Branchcode", UserHelper.BranchCode) });
                lst = riDao.GetAll();
                if (lst.Count <= 0) return false;
                Returnitem ri = (Returnitem)lst[0];
                ri.Status = (int)ReturnItemStatus.Returned;
                ri.Confirmdate = DateTime.Now;
                if (ri.Vmepcomment == null) ri.Vmepcomment = "";
                if (ri.Returnnumber == null) ri.Returnnumber = "";

                if (InventoryHelper.IsInventoryLock(ri.Releasedate, UserHelper.DealerCode, UserHelper.BranchCode))
                {
                    //AddError(string.Format(ReturnItemErrorCode.InventoryLocked,lockdate.Month + "/" + lockdate.Year));
                    AddError(ReturnItemErrorCode.InventoryLocked);
                    return false;
                }

                iis.Releaseddate = ri.Releasedate;

                // tranHistory record
                TransHis trh = new TransHis();
                trh.Actualcost = 0;
                trh.Frombranch = branchCode;
                trh.Modifieddate = DateTime.Now;
                trh.Iteminstance = iis;
                trh.Transactiondate = ri.Releasedate;
                trh.Transactiontype = (int)ItemStatus.Return;
                trh.Modifiedby = UserHelper.Username;

                // daily tracking
                InventoryHelper.SaveInventoryDay(iis.Item.Id, ri.Releasedate, -1, (int)ItemStatus.Return, UserHelper.DealerCode, branchCode);

                // save data
                iisDao.SaveOrUpdate(iis);
                riDao.SaveOrUpdate(ri);
                trhDao.Save(trh);

                trans.IsValid = true;
                return true;
            }
            catch { trans.IsValid = false; return false; }
        }
    }
    private void DeleteRequest(long riId)
    {
        IDao<Returnitem, long> riDao = DaoFactory.GetDao<Returnitem, long>();
        // returnItem record
        riDao.SetCriteria(new ICriterion[] { Expression.Eq("Id", riId) });
        List<Returnitem> lst = riDao.GetAll();
        if (lst.Count <= 0) return;
        Returnitem ri = (Returnitem)lst[0];
        riDao.Delete(ri.Id);
    }
    private bool CancelRequest(long riId, string dealerCode)
    {
        IDao<Returnitem, long> riDao = DaoFactory.GetDao<Returnitem, long>();
        // returnItem record
        riDao.SetCriteria(new ICriterion[] { Expression.Eq("Id", riId) });
        List<Returnitem> lst = riDao.GetAll();
        if (lst.Count <= 0) return false;
        Returnitem ri = (Returnitem)lst[0];
        // check for item instock?
        if (!ItemHepler.IsInstock(ri.Iteminstance.Status))
        {
            AddError(ReturnItemErrorCode.ItemHasOutOfStock);
            return false;
        }
        // change status
        ri.Status = (int)ReturnItemStatus.DealerCanceled;
        if (ri.Vmepcomment == null) ri.Vmepcomment = "";
        if (ri.Returnnumber == null) ri.Returnnumber = "";

        riDao.SaveOrUpdate(ri);
        return true;
    }

    private void ClearForm()
    {
        lblModel.Text = "";
        LblColor.Text = "";
        lblCurStatus.Text = "";
        lblCurStore.Text = "";
        lblDate.Text = "";
        lblVoucher.Text = "";
        lblReturnStatus.Text = "";
        lblReturnNumber.Text = "";
        lblReturnNote.Text = "";
        txtReason.Text = "";
        ViewState[VS_ReturnItemID] = "";
        ViewState[VS_ItemInstanceID] = "";
    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        ShowDetailPanel(false);
        hdMachineNo.Value = txtMachineNo.Text.Trim();
        ClearForm();

        Iteminstance item = GetItemInfos(hdMachineNo.Value);
        Shippingdetail ISD = GetItemShippingDetail(hdMachineNo.Value);
        if (item == null) { AddError(ReturnItemErrorCode.VerhicleNotExist); return; }
        if (ISD == null) { AddError(ReturnItemErrorCode.ShippingDetailError); return; }

        // save info to review later
        ViewState[VS_ItemInstanceID] = item.Id;

        // vehicle info
        int sIndex = item.Color.IndexOf("("), eIndex = item.Color.IndexOf(")");
        sIndex = (sIndex >= 0) ? sIndex + 1 : 0;
        eIndex = (eIndex >= 0) ? eIndex : item.Color.Length;

        LblColor.Text = item.Color.Substring(sIndex, eIndex - sIndex);
        lblVoucher.Text = ISD.Voucherstatus ? Constants.Yes : Constants.No;
        lblCurStatus.Text = ItemHepler.GetNativeItemStatusName(item.Status);
        lblCurStore.Text = item.Branchcode;
        lblModel.Text = item.Itemtype;
        lblDate.Text = (item.Madedate == DateTime.MinValue) ? "" : item.Madedate.ToShortDateString();
        lbimportdate.Text = item.Importeddate.ToShortDateString();

        // return info
        Returnitem ri = GetReturnInfo(item.Id, item.Enginenumber);
        if (ri != null)
        {
            lblReturnStatus.Text = ItemHepler.GetVMEPReturnStatusName(ri.Status);
            if (ri.Status != (int)ReturnItemStatus.Proposed)
            {  
                lblReturnNumber.Text = ri.Returnnumber;
                lblReturnNote.Text = ri.Vmepcomment;
                ViewState[VS_ReturnItemID] = ri.Id;
                txtReleasedate.Enabled = false;
                ibReleasedate.Enabled = false;
                txtReason.Enabled = false;
            }
            if (ri.Status == (int)ReturnItemStatus.NotAllow)
            {
                txtReleasedate.Enabled = true;
                ibReleasedate.Enabled = true;
                txtReason.Enabled = true;
            }
            txtReason.Text = ri.Returnreason;
            txtReleasedate.Text = ri.Releasedate.ToShortDateString();
            riID.Value = ri.Id.ToString();
        }

        bool vhInStock = ItemHepler.IsInstock(item.Status);

        txtReason.ReadOnly = (ri != null);
        btnSend.Enabled = vhInStock;
        btnReject.Enabled = vhInStock;
        btnSend.Visible = (ri == null) || (ReturnItemStatus.DealerCanceled == (ReturnItemStatus)ri.Status);
        if (ri != null && ((ReturnItemStatus)ri.Status == ReturnItemStatus.Proposed || (ReturnItemStatus.NotAllow == (ReturnItemStatus)ri.Status)))
        {
            btnReSend.Visible = true;
            btdelete.Visible = true;
        }
        else
        {
            btnReSend.Visible = false;
            btdelete.Visible = false;
        }
        btnReject.Visible = (ri != null) && (ReturnItemStatus.Allowed == (ReturnItemStatus)ri.Status);
        btnCancel.Visible = (ri != null) && (ReturnItemStatus.Allowed == (ReturnItemStatus)ri.Status);
        ShowDetailPanel(true);
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (txtReason.Text.Trim() == "") return;
        long iisId;
        if (Convert.ToDateTime(txtReleasedate.Text) < Convert.ToDateTime(lbimportdate.Text))
        { AddError(ReturnItemErrorCode.ReleaseMoreThanImport); return; }
        if (Convert.ToDateTime(txtReleasedate.Text) >= DateTime.Now)
        {
            AddError(ReturnItemErrorCode.ReleaseMoreThanNow); return;
        }
        if (!long.TryParse(ViewState[VS_ItemInstanceID].ToString(), out iisId))
        { AddError(ReturnItemErrorCode.SaveStateFailed); return; }
        if (InventoryHelper.IsInventoryLock(Convert.ToDateTime(txtReleasedate.Text), UserHelper.DealerCode, UserHelper.BranchCode))
        {
            AddError(ReturnItemErrorCode.InventoryLocked);
            return;
        }
        if (!string.IsNullOrEmpty(riID.Value))
        {
            if (!SaveReturnPropose(iisId, txtReason.Text.Trim(), int.Parse(riID.Value)))
            { AddError(ReturnItemErrorCode.CanNotSendToVMEP); return; }
        }
        else
        {
            if (!SaveReturnPropose(iisId, txtReason.Text.Trim(), 0))
            { AddError(ReturnItemErrorCode.CanNotSendToVMEP); return; }
        }

        ShowDetailPanel(false);
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        long riId;
        if (!long.TryParse(ViewState[VS_ReturnItemID].ToString(), out riId))
        {
            AddError(ReturnItemErrorCode.SaveStateFailed); return;
        }

        if (InventoryHelper.IsInventoryLock(Convert.ToDateTime(txtReleasedate.Text), UserHelper.DealerCode, UserHelper.BranchCode))
        {
            AddError(ReturnItemErrorCode.InventoryLocked);
            return;
        }

        if (!SaveReturnFinish(hdMachineNo.Value, UserHelper.DealerCode))
        {
            AddError(ReturnItemErrorCode.FinishReturnFailed); return;
        }

        ShowDetailPanel(false);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        long riId;
        if (!long.TryParse(ViewState[VS_ReturnItemID].ToString(), out riId))
        {
            AddError(ReturnItemErrorCode.SaveStateFailed); return;
        }
        if (!CancelRequest(riId, UserHelper.DealerCode))
        {
            AddError(ReturnItemErrorCode.CancelRequestFailed); return;
        }
        ShowDetailPanel(false);
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {

        DeleteRequest(int.Parse(riID.Value));
        ShowDetailPanel(false);
    }
}
