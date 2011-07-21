using System;
using VDMS.Helper;

public partial class ErrorPage : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Exception exc = ErrorHelper.Exception;
		if (exc != null && exc.InnerException != null)
		{
			litMessage.Text = exc.InnerException.Message;
			litDetail.Text = exc.InnerException.ToString();
		}
	}

	protected void bBack_Click(object sender, EventArgs e)
	{
		Response.Redirect(Request.QueryString["aspxerrorpath"]);
	}
}
