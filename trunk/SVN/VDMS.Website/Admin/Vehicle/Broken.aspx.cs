using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using Resources;
using VDMS.I.ObjectDataSource;
using VDMS.Core.Domain;

public partial class Admin_Database_Spare : BasePage
{

	private bool wrongUpdateBrokenCode = false;  // update failed cause by BrokenCode
	private const string strUnKnown = "  ";

	private Collection<BrokenErrorCode> errorCode = new Collection<BrokenErrorCode>();

	protected void AddError(BrokenErrorCode error)
	{
		if (errorCode.Contains(error) || (error == BrokenErrorCode.OK)) return;
		errorCode.Add(error);
	}
	private void ShowError()
	{
		bllErrorMsg.Visible = errorCode.Count > 0;
		bllErrorMsg.Items.Clear();
		foreach (BrokenErrorCode err in errorCode)
		{
			switch (err)
			{
				case BrokenErrorCode.BrokenCodeExist: bllErrorMsg.Items.Add(Message.AddBroken_BrokenCodeExist); break;
				case BrokenErrorCode.BrokenInUse: bllErrorMsg.Items.Add(Message.BrokenCodeInUse); break;
			}
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		//
		errorCode.Clear();
		if (!IsPostBack)
		{
            GridView1.EmptyDataText = Message.DataNotFound;
            GridView1.Visible = false;
		}
		//if (IsChangeLanguage && GridView1.Visible) GridView1.DataBind();
	}

	protected void Page_PreRender(object sender, EventArgs e)
	{
		ShowError();
	}

	protected void btnTest_Click(object sender, EventArgs e)
	{
		GridView1.Visible = true;
		GridView1.DataBind();
	}

	protected void btnAdd_Click(object sender, EventArgs e)
	{
		Page.Response.Redirect("AddBroken.aspx");
	}

	protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		DateTime dt;

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			GridView gv = ((GridView)sender);

			// format date time
			Literal lbl = (Literal)e.Row.FindControl("lblLastUpdate");
			if (lbl == null) return;
			if (!DateTime.TryParse(lbl.Text, Thread.CurrentThread.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dt)) return;
			lbl.Text = dt.ToShortDateString();

			// set confirm message
			ImageButton ib = (ImageButton)e.Row.FindControl("imgbDelete");
			if (ib == null) return;
			ib.OnClientClick = DeleteConfirmQuestion;

			// set Item No
			Literal lit = (Literal)e.Row.FindControl("litNo");
			if (lit == null) return;
			int no = gv.PageIndex * gv.PageSize + e.Row.RowIndex + 1;
			lit.Text = no.ToString();
		}
	}
	protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
	{
		e.KeepInEditMode = (e.AffectedRows <= 0) && wrongUpdateBrokenCode;
	}
	protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		e.NewValues["Editby"] = (UserHelper.Fullname == "") ? strUnKnown : UserHelper.Fullname;
		// check for new broken code already exist
		if (e.OldValues["Brokencode"].ToString().ToLower() == e.NewValues["Brokencode"].ToString().ToLower()) return;
		BrokenDatasource ds = new BrokenDatasource();
		string bcode = e.NewValues["Brokencode"].ToString().Trim();
		IList<Broken> lst = ds.Select(10, 0, bcode, bcode);
		if (lst.Count > 0)
		{
			e.Cancel = true;
			wrongUpdateBrokenCode = true;
			AddError(BrokenErrorCode.BrokenCodeExist);
		}
	}

	// manual to delete
	protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		if (!BrokenDatasource.Delete(e.Values[0].ToString()))
		{
			AddError(BrokenErrorCode.BrokenInUse);

		}
		else
		{
			GridView1.DataBind();
		}
		e.Cancel = true;
	}
}
