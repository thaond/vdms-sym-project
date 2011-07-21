using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_UpdateProgress : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public string AssociatedUpdatePanelID
    {
        get { return UpdateProgress1.AssociatedUpdatePanelID; }
        set { UpdateProgress1.AssociatedUpdatePanelID = value; }
    }
}
