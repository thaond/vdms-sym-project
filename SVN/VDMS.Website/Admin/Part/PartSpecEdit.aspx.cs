using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Part_PartSpecEdit : BasePopup
{
    public long PartSpecId { get { long id; long.TryParse(Request.QueryString["id"], out id); return id; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (PartSpecId > 0)
            form.ChangeMode(FormViewMode.Edit);
        else
            form.ChangeMode(FormViewMode.Insert);
    }
    protected void form_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        ClosePopup("updated()");
    }
    protected void form_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        ClosePopup("updated()");
    }
}
