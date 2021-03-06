using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using VDMS.Common.Utils;
using VDMS.Core.Domain;
using VDMS.I.ObjectDataSource;
using VDMS.I.Vehicle;

public partial class Sales_Inventory_History : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bllMessage.Items.Clear();
    }

    protected void btnTest_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        TransHisDataSource thDS = new TransHisDataSource();
        List<TransHis> listTransHis = new List<TransHis>();
        string EngineNo = txtEngineNo.Text.Trim().ToUpper();
        listTransHis = thDS.Select(EngineNo);
        if (listTransHis.Count > 0)
        {
            TransHis th = listTransHis[0];
            //listTransHis.RemoveAt(0);

            Invoice iv = ItemInstanceHelper.GetInvoiceByItemInstance(th.Iteminstance);
            Shippingdetail sh = ItemInstanceHelper.GetShippingdetailByItemInstance(th.Iteminstance.Id);

            lblType.Text = th.Iteminstance.Itemtype;
            lblColour.Text = th.Iteminstance.Color;
            lblAddress.Text = AddressDataSource.GetAddressByBranchCode(th.Iteminstance.Branchcode);
            lblImportDate.Text = th.Iteminstance.Importeddate.ToShortDateString();
            lblMoneyImport.Text = NumberFormatHelper.NumberToCurentMoneyFormatString((Decimal)th.Actualcost);
            if (th.Iteminstance.Madedate != DateTime.MinValue)
                lblExportDate.Text = th.Iteminstance.Madedate.ToShortDateString();
            else
                lblExportDate.Text = string.Empty;

            if (iv != null) // Đã được bán
            {
                lblAgencyNo.Text = iv.Dealercode; // Nếu lấy mã đại lí bán
                //lblAgencyNo.Text = th.Iteminstance.Branchcode; // Có phải là mã của chi nhánh bán không? Đúng ra phải lấy trong bản Invoice
                lblIdentityNo.Text = iv.Customer.Identifynumber;
                lblMoneySale.Text = NumberFormatHelper.NumberToCurentMoneyFormatString(iv.Sellitem.Pricebeforetax);
            }
            if (sh != null)
            {
                lblInvoice.Text = sh.Ordernumber;
                if (sh.Voucherstatus)
                    lblReceiptStatus.Text = Resources.ShippingDetail.Voucher;
                else
                    lblReceiptStatus.Text = Resources.ShippingDetail.NoneVoucher;
            }

            lblImportExportDate.Text = th.Transactiondate.ToShortDateString();
            lblInventoryStatus.Text = ItemStatusToStringName(th.Transactiontype);
            //lblInventoryStatus.Text = ItemStatusToStringName(th.Iteminstance.Status);
        }
        else
        {
            bllMessage.Items.Add(Resources.HistoryMessage.DataEmpty);

            lblType.Text = string.Empty;
            lblColour.Text = string.Empty;
            lblAddress.Text = string.Empty;
            lblImportDate.Text = string.Empty;
            lblMoneyImport.Text = string.Empty;
            lblExportDate.Text = string.Empty; ;
            lblReceiptStatus.Text = string.Empty;
            lblInvoice.Text = string.Empty;
            lblImportExportDate.Text = string.Empty;
            lblInventoryStatus.Text = string.Empty;
            lblMoneySale.Text = string.Empty;
            liAgencyNo.Text = string.Empty;
            lblIdentityNo.Text = string.Empty;
        }
        grdHistory.DataSource = listTransHis;
        grdHistory.DataBind();
    }
    protected void grdHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow) e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
    }

    #region Biz
    //EvalType(Eval("Transactiontype"))
    protected string EvalType(object oTranType)
    {
        string res = string.Empty;
        try
        {
            int TranType = int.Parse(oTranType.ToString());
            res = ItemStatusToStringName(TranType);
        }
        catch { }
        return res;
    }
    private string ItemStatusToStringName(int status)
    {
        string res = string.Empty;
        switch (status)
        {
            case (int)ItemStatus.Imported:
                res = Resources.ItemStatusString.Imported;
                break;
            case (int)ItemStatus.AdmitTemporarily:
                res = Resources.ItemStatusString.AdmitTemporarily;
                break;
            case (int)ItemStatus.Moved:
                res = Resources.ItemStatusString.Moved;
                break;
            case (int)ItemStatus.Sold:
                res = Resources.ItemStatusString.Sold;
                break;
            case (int)ItemStatus.Return:
                res = Resources.ItemStatusString.Return;
                break;
            case (int)ItemStatus.Redundant:
                res = Resources.ItemStatusString.Redundant;
                break;
            case (int)ItemStatus.Lacked:
                res = Resources.ItemStatusString.Lacked;
                break;
            case (int)ItemStatus.ReceivedFromMoving:
                res = Resources.ItemStatusString.ReceivedFromMoving;
                break;
        }
        return res;
    }
    #endregion
}
