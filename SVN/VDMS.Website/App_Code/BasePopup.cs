using System.Web.UI.WebControls;
using VDMS.Common.Web;

/// <summary>
/// Summary description for BasePopup
/// </summary>
public class BasePopup : BasePage
{
	public string Key
	{
		get { return Request.QueryString["key"]; }
	}

	protected void ClosePopup(string callBack)
	{
		//  register the script to close the popup
		//this.Page.ClientScript.RegisterStartupScript(this.GetType(), string.Format("closeThickBox{0}", callBack.Trim()), string.Format("self.parent.{0}();", callBack.Trim()), true);
		(WebTools.FindControlById("__litPopupSignHolder", this.Master) as Literal).Text = string.Format("<input id=\"__hdPopupOkSign\" type=\"hidden\" value=\"{0}\" />", callBack);
	}
}
