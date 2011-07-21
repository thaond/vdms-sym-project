using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.PartManagement;
using Resources;
using VDMS;
using VDMS.Common.Web;

public partial class Admin_Part_PartSpec : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        InitErrMsgControl(Msg);
        InitInfoMsgControl(Msg);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        btnSave.Visible = PartSpecDAO.Parts.Count > 0;
    }
    protected void btnFind_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(gv.DataSourceID))
            gv.DataSourceID = ods.ID;
        else
            gv.DataBind();
    }
    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = (e.Row.DataItemIndex +1).ToString();
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            PartSpecDAO.Parts.Clear();
            PartSpecDAO.LoadExcelData(FileUpload1.FileContent, VDMSSetting.CurrentSetting.PartSpecExcelUploadSetting);
            PartSpecDAO.CheckUploadData();
            gvImp.DataBind();
            if (!PartSpecDAO.Parts.Any(p => !string.IsNullOrEmpty(p.Error)))
            {
                try
                {
                    PartSpecDAO.SavePart();
                    AddInfoMsg(Message.ActionSucessful);
                }
                catch (Exception ex) { AddErrorMsg(ex.Message); }
            }
            else
            {
                AddErrorMsg("Some parts have errors");
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (PartSpecDAO.Parts.Count == 0)
        {
            AddErrorMsg("No data to save, upload excel file first");
            return;
        }

        PartSpecDAO.CheckUploadData();
        var errorLines = PartSpecDAO.Parts.Where(p => !string.IsNullOrEmpty(p.Error)).Select(p => p.Line).ToList();
        if (errorLines.Count > 0)
        {
            AddErrorMsg("Some parts have errors");
        }
        else
        {
            try
            {
                PartSpecDAO.SavePart();
                AddInfoMsg(Message.ActionSucessful);
            }
            catch (Exception ex) { AddErrorMsg(ex.Message); }
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        gv.AllowPaging = false;
        gv.Columns[gv.Columns.Count - 1].Visible = gv.Columns[gv.Columns.Count - 2].Visible = false;
        gv.GridLines = GridLines.Both;
        ods.EnablePaging = false;
        gv.DataBind();
        GridView2Excel.Export(gv, "PartSpec.xls");
    }
}
