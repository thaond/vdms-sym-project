using System;

public partial class Service_verifyExchangeVoucher : BasePage
{
    private string pcvn;
    protected void Page_PreRender(object sender, EventArgs e)
    {
        pnBatchAct.Visible = !evReport.IsEditing;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        pcvn = Request.QueryString["pcvn"];
        evReport.OnVerify = !string.IsNullOrEmpty(pcvn);
        evReport.ShowEditButton = evReport.ShowVerifyButton = evReport.OnVerify;
        evReport.ProposalNumber = pcvn;
        evReport.ShowAllPageSummary = true;

        //evReport2.OnVerify = !string.IsNullOrEmpty(pcvn);
        //evReport2.ProposalNumber = pcvn;
        //evReport2.ShowAllPageSummary = true;
    }

    // batch actions
    protected void btnApproveAllRemain_Click(object sender, EventArgs e)
    {
        evReport.ApproveAllRemain();
    }
    protected void btnRejectAll_Click(object sender, EventArgs e)
    {
        evReport.RejectAllRemain();
    }
    protected void btnApprovePage_Click(object sender, EventArgs e)
    {
        evReport.ApproveAllPageRemain();
    }
    protected void btnRejectPage_Click(object sender, EventArgs e)
    {
        evReport.RejectAllPageRemain();
    }
}
