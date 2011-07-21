using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.I.Vehicle;
using VDMS.I.Entity;
using Resources;
using VDMS.Common.Utils;

public partial class Vehicle_Fin_ImportPayment : BasePage
{
    public string NewDealerCode
    {
        get
        {
            return txtDealer.Text.Trim().ToUpper();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitErrMsgControl(_error);
        InitInfoMsgControl(_msg);
        if (!IsPostBack)
        {
            btnSave.Enabled = false;
            txtDate.Text = DateTime.Now.ToShortDateString();
            rvDate.MinimumValue = new DateTime(2000, 1, 1).ToShortDateString();
            rvDate.MaximumValue = DateTime.Now.ToShortDateString();
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (file.HasFile)
        {
            try
            {
                PaymentManager.LoadExcelData(file.FileContent, ddlBank.SelectedValue);
                gvPayment.DataBind();
            }
            catch (Exception ex)
            {
                AddErrorMsg(ex.Message);
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    PaymentManager.SavePayments();
        //    btnSave.Enabled = false;
        //    AddInfoMsg(Message.ActionSucessful);
        //}
        //catch (Exception ex)
        //{
        //    AddErrorMsg(ex.Message);
        //}
        PaymentManager.SavePayments();
        btnSave.Enabled = false;
        AddInfoMsg(Message.ActionSucessful);
    }

    int errorCount = 0;
    protected void gvPayment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var error = Server.HtmlDecode(e.Row.Cells[e.Row.Cells.Count - 2].Text).Trim();
            string css = e.Row.RowIndex % 2 == 0 ? "even" : "odd";
            e.Row.CssClass += string.IsNullOrEmpty(error) ? css : css + " errorText";
            if (!string.IsNullOrEmpty(error)) errorCount++;
        }
    }
    protected void gvPayment_DataBound(object sender, EventArgs e)
    {
        if (errorCount > 0) AddErrorMsg(string.Format(Errors.ErrorsFound, errorCount));
        btnSave.Enabled = (errorCount == 0);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        PaymentManager.CleanSessionBank(ddlBank.SelectedValue);
        gvPayment.DataBind();
    }
    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        PaymentManager.CleanSession(Session.SessionID);
        gvPayment.DataBind();
    }

    protected void ClearNewForm()
    {
        txtOrderNum.Text = txtDealer.Text = txtAmount.Text = txtDesc.Text = txtTrans.Text = "";
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var am = txtAmount.Text.Trim();
            var d = VDMS.II.BasicData.DealerDAO.GetDealerByCode(NewDealerCode);
            if (d != null)
            {
                PaymentManager.ImportPayment(txtOrderNum.Text.Trim(),
                    ddlNewBank.SelectedValue, NewDealerCode, d.DealerName, txtTrans.Text.Trim().ToUpper(), txtDesc.Text.Trim(),
                    DataFormat.DateFromString(txtDate.Text), string.IsNullOrEmpty(am) ? 0 : long.Parse(am));
                ClearNewForm();
                gvPayment.DataBind();
            }
        }
    }

    protected void cvOrderNum_ServerValidate(object source, ServerValidateEventArgs args)
    {
        OrderHeader o;
        long oid;
        if (long.TryParse(txtOrderNum.Text.Trim(), out oid))
            o = OrderDAO.GetOrder(oid);
        else
            o = OrderDAO.GetOrder(txtOrderNum.Text.Trim());

        args.IsValid = o != null && o.DealerCode == NewDealerCode;
    }
    protected void cvDealer_ServerValidate(object source, ServerValidateEventArgs args)
    {
        var d = VDMS.II.BasicData.DealerDAO.GetDealerByCode(NewDealerCode);
        args.IsValid = d != null;
    }
}
