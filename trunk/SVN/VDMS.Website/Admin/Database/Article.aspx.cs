using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Provider;

public partial class Admin_Database_Article : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (!IsPostBack)
        {
            txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
        }
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		grdArticles.PageIndex = 0;
        grdArticles.DataBind();
	}

	protected string Left(string s)
	{
		if (s.Length <= 15) return s;
		return s.Substring(0, 15);
	}

	protected string Dept(string s)
	{
		//ProfileCommon pc = (ProfileCommon)ProfileBase.Create(s, true);
		//return pc.Dept;
		return s;
	}
}
