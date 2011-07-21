using System;
using System.Data.Linq;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;

public partial class Admin_Part_Category : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.InitErrMsgControl(divRight);
    }

    protected bool EvalCanDelete(object partInfos)
    {
        EntitySet<PartInfo> childs = (EntitySet<PartInfo>)partInfos;
        return childs.Count == 0;
    }

	protected void cmdSave_Click(object sender, EventArgs e)
    {
        if (UserHelper.IsDealer)
        {
            Category cat = CategoryDAO.CreateCategory(txtCode.Text.Trim(), txtName.Text.Trim(), UserHelper.DealerCode);
            if (cat != null)
            {
                gv.DataBind();
                ResetControl(divRight);
            }
            else
            {
                base.AddErrorMsg(string.Format(Resources.Message.ItemAlreadyExist, txtCode.Text.Trim()));
            }
        }
        else
        {
            base.AddErrorMsg(Resources.Message.YouMustBeDealer);
        }
    }
}

