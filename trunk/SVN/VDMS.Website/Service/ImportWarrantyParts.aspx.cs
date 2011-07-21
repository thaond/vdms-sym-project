using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel;
using System.IO;
using VDMS.I.Service;

public partial class Service_ImportWarrantyParts : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        WarrantyConditionDAO.ClearSessionData();
        if (FileUpload.HasFile)
        {
            WarrantyConditionDAO.ImportExcelData(FileUpload.PostedFile.InputStream, VDMS.VDMSSetting.CurrentSetting.WarrantyPartSetting);
            lv.DataSource = WarrantyConditionDAO.ImportingItems;
            lv.DataBind();
        }
    }

   
    protected void btnImport_Click(object sender, EventArgs e)
    {
        //var dc = DCFactory.GetDataContext<ServiceDataContext>();
        //foreach (var item in ImportingItems)
        //{
        //    var existingItem = dc.WarrantyConditions.SingleOrDefault(p => p.PartCode.ToUpper() == item.PartCode);
        //    if (existingItem == null)
        //    {
        //        dc.WarrantyConditions.InsertOnSubmit(item);
        //    }
        //}
        //dc.SubmitChanges();
        WarrantyConditionDAO.SaveImportingWarrantyParts();
        WarrantyConditionDAO.ClearSessionData();
        lv.DataBind();
    }
}
