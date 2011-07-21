using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using NHibernate.Expression;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;

public partial class Admin_Database_WarrantyTime : BasePage
{
	protected string DeleteAlert;
	protected void Page_Load(object sender, EventArgs e)
	{
		DeleteAlert = Resources.Question.DeleteData;
		if (!Page.IsPostBack)
		{
			//placeSearchControl.Visible = false;
		}
		lbErr.Visible = false;
	}
	protected void btnAddNew_Click(object sender, EventArgs e)
	{
		CallViewControl(1);
		placeSearchControl.Visible = false;
		btnAddNew.Visible = true; btnInsertNew.Visible = true;
		btnUpdate.Visible = false;
		txtPartcode.ReadOnly = false; txtPartcode.CssClass = null;

		txtPartcode.Text = "";
		txtPartNameEN.Text = "";
		txtPartNameVN.Text = "";
		txtModel.Text = "";
		txtWarrantyMonth.Text = "";
		txtKMWarranty.Text = "";
		txtWarrantyCost.Text = "";
		txtStandartHours.Text = "";
		txtUnitPrice.Text = "";
	}
	protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		Label lblwc = (Label)gvItems.Rows[e.RowIndex].FindControl("lblWARRANTYCONTIONID");
		DelWarrantyCondition(decimal.Parse(lblwc.ToolTip));
		gvItems.Visible = false;
	}
	private void DelWarrantyCondition(decimal idWCon)
	{
		ISession sess = NHibernateSessionManager.Instance.GetSession();
		try
		{
			Warrantycondition wcon = sess.CreateCriteria(typeof(Warrantycondition))
				.Add(Expression.Eq("Id", idWCon)).List()[0] as Warrantycondition;

			IList epdlst = sess.CreateCriteria(typeof(Exchangepartdetail))
				.Add(Expression.Or(Expression.Eq("Partcodem", wcon.Partcode), Expression.Eq("Partcodeo", wcon.Partcode)))
				.List();
			IList eslst = sess.CreateCriteria(typeof(Servicedetail)).Add(Expression.Eq("Partcode", wcon.Partcode)).List();
			if ((epdlst.Count + eslst.Count) > 0)
			{
				ShowMessage(Resources.Message.PartCodeExisted, true);
			}
			else
				sess.Delete(wcon);
		}
		catch (Exception) { }
	}
	protected void gvItems_RowEditing(object sender, GridViewEditEventArgs e)
	{
		placeSearchControl.Visible = false;
		CallViewControl(1);
		btnUpdate.Visible = true;
		btnInsertNew.Visible = false;
		txtPartcode.ReadOnly = true; txtPartcode.CssClass = "readOnlyInputField";
		//gvItems.Visible = false;

		Label lblwc = (Label)gvItems.Rows[e.NewEditIndex].FindControl("lblWARRANTYCONTIONID");
		decimal idWCon = (decimal.Parse(lblwc.ToolTip));
		ISession sess = NHibernateSessionManager.Instance.GetSession();
		Warrantycondition wcon = sess.CreateCriteria(typeof(Warrantycondition))
				.Add(Expression.Eq("Id", idWCon)).List()[0] as Warrantycondition;

		txtPartcode.Text = wcon.Partcode;
		txtPartNameEN.Text = wcon.Partnameen;
		txtPartNameVN.Text = wcon.Partnamevn;
		txtModel.Text = wcon.Motorcode;
		txtWarrantyMonth.Text = wcon.Warrantytime.ToString();
		txtKMWarranty.Text = wcon.Warrantylength.ToString();
		txtWarrantyCost.Text = wcon.Labour.ToString();
		txtStandartHours.Text = NumberFormatHelper.StrDoubleToStr(wcon.Manpower, "en-US");
		txtUnitPrice.Text = wcon.Unitprice.ToString();
	}
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		ISession sess = NHibernateSessionManager.Instance.GetSession();
		IList lstwcon = sess.CreateCriteria(typeof(Warrantycondition))
				.Add(Expression.Eq("Partcode", txtPartcode.Text.Trim())).List();

		if (lstwcon.Count > 0)
		{
			Warrantycondition wcon = lstwcon[0] as Warrantycondition;
			//Edit update
			wcon.Partnamevn = txtPartNameVN.Text.Trim();
			wcon.Partnameen = txtPartNameEN.Text.Trim();
			wcon.Motorcode = txtModel.Text.Trim();
			wcon.Warrantytime = long.Parse(checkNotEmptyDataNumberic(txtWarrantyMonth.Text.Trim()));
			wcon.Warrantylength = decimal.Parse(checkNotEmptyDataNumberic(txtKMWarranty.Text.Trim()));
			wcon.Labour = decimal.Parse(checkNotEmptyDataNumberic(txtWarrantyCost.Text.Trim()));
			wcon.Manpower = NumberFormatHelper.StrDoubleToStr(txtStandartHours.Text.Trim(), Thread.CurrentThread.CurrentCulture.Name, "en-US");
			wcon.Unitprice = decimal.Parse(checkNotEmptyDataNumberic(txtUnitPrice.Text.Trim()));
			sess.SaveOrUpdate(wcon);
			ShowMessage(Resources.Message.ActionSucessful, false);
			placeSearchControl.Visible = true;
			CallViewControl(-1);
		}
		else
		{			//Raise Err
		}
	}
	protected void btnInsertNew_Click(object sender, EventArgs e)
	{
		ISession sess = NHibernateSessionManager.Instance.GetSession();
		IList lstwcon = sess.CreateCriteria(typeof(Warrantycondition))
				.Add(Expression.Eq("Partcode", txtPartcode.Text.Trim())).List();

		if (!(lstwcon.Count > 0))
		{
			Warrantycondition wcon = new Warrantycondition();
			if (txtPartcode.Text.Trim().Length < 6 || txtPartcode.Text.Trim().Length > 35)
			{
				ShowMessage(Resources.Message.WarrantyCondition_PartcodeLength, true);
				return;
			}
			else wcon.Partcode = txtPartcode.Text.Trim();
			wcon.Partnamevn = txtPartNameVN.Text.Trim();
			wcon.Partnameen = txtPartNameEN.Text.Trim();
			wcon.Motorcode = txtModel.Text.Trim();
			wcon.Warrantytime = long.Parse(checkNotEmptyDataNumberic(txtWarrantyMonth.Text.Trim()));
			wcon.Warrantylength = decimal.Parse(checkNotEmptyDataNumberic(txtKMWarranty.Text.Trim()));
			wcon.Labour = decimal.Parse(checkNotEmptyDataNumberic(txtWarrantyCost.Text.Trim()));
			wcon.Manpower = NumberFormatHelper.StrDoubleToStr(txtStandartHours.Text.Trim(), Thread.CurrentThread.CurrentCulture.Name, "en-US");
			wcon.Unitprice = decimal.Parse(checkNotEmptyDataNumberic(txtUnitPrice.Text.Trim()));
			ITransaction tx = sess.BeginTransaction();
			sess.SaveOrUpdate(wcon);
			tx.Commit();
			ShowMessage(Resources.Message.ActionSucessful, false);
			placeSearchControl.Visible = true;
			CallViewControl(-1);
		}
		else
		{
			ShowMessage(Resources.Message.Part_AddNewFailed, true);
		}
	}
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		placeSearchControl.Visible = true;
		if (gvItems.DataSource != null)
		{
			CallViewControl(0);
		}
		else CallViewControl(-1);
	}
	protected string Convert2Currency(string ObConvert)
	{
		decimal ICurency = decimal.Parse(ObConvert);
		NumberFormatInfo nfi = Thread.CurrentThread.CurrentCulture.NumberFormat;
		nfi.NumberDecimalDigits = 0;
		return ICurency.ToString("N", nfi);
	}

	protected void btnSearch_Click(object sender, EventArgs e)
	{
		//HttpContext.Current.Items["PartCodeNo1"] = txtPartCodeNo1.Text.Trim();
		//HttpContext.Current.Items["PartCodeNo2"] = txtPartCodeNo2.Text.Trim();
		gvItems.PageIndex = 0;
		SearchGrid();
		//placeSearchControl.Visible = false;
	}
	private void ShowMessage(string mesg, bool isError)
	{
		lbErr.Visible = true;
		lbErr.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
		lbErr.Text = mesg;
	}
	private void SearchGrid()
	{
		ISession sess = NHibernateSessionManager.Instance.GetSession();
		ICriteria icrit = sess.CreateCriteria(typeof(Warrantycondition));

		if (checkNotEmptyData(txtPartCodeNo1.Text.Trim()))
		{
			//icrit.Add(Expression.Between("Partcode", txtPartCodeNo1.Text.Trim(), txtPartCodeNo2.Text.Trim()));
			icrit.Add(Expression.Like("Partcode", "%" + txtPartCodeNo1.Text.Trim().ToUpper() + "%"));
		}

		if (checkNotEmptyData(txtWarrantyFrom.Text.Trim()) && checkNotEmptyData(txtWarrantyTo.Text.Trim()))
		{
			icrit.Add(Expression.Between("Warrantytime", txtWarrantyFrom.Text.Trim(), txtWarrantyTo.Text.Trim()));
		}

		if (checkNotEmptyData(txtWarrantyKm1.Text.Trim()) && checkNotEmptyData(txtWarrantyKm2.Text.Trim()))
		{
			icrit.Add(Expression.Between("Warrantylength", txtWarrantyKm1.Text.Trim(), txtWarrantyKm2.Text.Trim()));
		}

		if (checkNotEmptyData(txtPartNameEng.Text.Trim()))
		{
			icrit.Add(Expression.InsensitiveLike("Partnameen", "%" + txtPartNameEng.Text.Trim() + "%"));
		}

		if (checkNotEmptyData(txtPartNameVie.Text.Trim()))
		{
			icrit.Add(Expression.InsensitiveLike("Partnamevn", "%" + txtPartNameVie.Text.Trim() + "%"));
		}
		icrit.AddOrder(Order.Asc("Partcode"));
		CallViewControl(0);
		IList lstPart = icrit.List();
		if (lstPart.Count > 0)
		{
			Session["GrdEditPartWarranty"] = lstPart;
			HttpContext.Current.Items["PartCount"] = lstPart.Count;
			gvItems.DataSource = lstPart;
			gvItems.DataMember = "Id";
			gvItems.DataBind();
			gvItems.Visible = true;
			refreshSearchData();
		}
		else
		{
			gvItems.Visible = false;
			ShowMessage(Resources.Message.PartNotFound, true);
		}
	}

	private void refreshSearchData()
	{
		txtPartCodeNo1.Text = ""; txtPartCodeNo2.Text = ""; txtWarrantyFrom.Text = "";
		txtWarrantyTo.Text = ""; txtWarrantyKm1.Text = "";
		txtWarrantyKm2.Text = "";
		txtPartNameEng.Text = ""; txtPartNameVie.Text = "";
	}

	private bool checkNotEmptyData(string ValidStringRequired)
	{
		if (ValidStringRequired != "" && ValidStringRequired.Length > 0)
		{
			return true;
		}
		else return false;
	}

	private string checkNotEmptyDataNumberic(string ValidStringRequired)
	{
		if (ValidStringRequired != "" && ValidStringRequired.Trim().Length > 0)
		{
			return ValidStringRequired;
		}
		return "0";
	}

	private void CallViewControl(int indexView)
	{
		switch (indexView)
		{
			case 0: ControlView.ActiveViewIndex = 0; break;
			case 1: ControlView.ActiveViewIndex = 1; break;
			default: ControlView.ActiveViewIndex = -1; break;
		}
	}

	protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		LoadOldGrid(e.NewPageIndex);
	}

	protected string ReturnIndex(String RowIndexInline)
	{
		return ((gvItems.PageSize * gvItems.PageIndex) + (int.Parse(RowIndexInline) + 1)).ToString();
	}

	private void LoadOldGrid(int OldPageIndex)
	{
		try
		{
			gvItems.PageIndex = OldPageIndex;
			gvItems.DataSource = (IList)Session["GrdEditPartWarranty"];
			gvItems.DataBind();
		}
		catch (Exception)
		{
			CallViewControl(-1);
			placeSearchControl.Visible = true;
		}
	}

	private void LoadOldGrid()
	{
		try
		{
			IList lstPartOld = (IList)Session["GrdEditPartWarranty"];
			HttpContext.Current.Items["PartCount"] = lstPartOld.Count;
			gvItems.DataSource = lstPartOld;
			gvItems.DataBind();
		}
		catch (Exception)
		{
			CallViewControl(-1);
			placeSearchControl.Visible = true;
		}
	}

	protected void cmdNext_Click(object sender, EventArgs e)
	{
		if (gvItems.PageIndex < gvItems.PageCount)
		{
			gvItems.PageIndex += 1; LoadOldGrid();
		}
	}
	protected void cmdPrevious_Click(object sender, EventArgs e)
	{
		if (gvItems.PageIndex > 0)
		{
			gvItems.PageIndex -= 1; LoadOldGrid();
		}
	}
	protected void cmdFirst_Click(object sender, EventArgs e)
	{
		gvItems.PageIndex = 0; LoadOldGrid();
	}
	protected void cmdLast_Click(object sender, EventArgs e)
	{
		gvItems.PageIndex = gvItems.PageCount - 1; LoadOldGrid();
	}
	protected void gvItems_DataBound(object sender, EventArgs e)
	{
		if (gvItems.TopPagerRow == null) return;
		Literal litPageInfo = gvItems.TopPagerRow.FindControl("litPageInfo") as Literal;
		if (litPageInfo != null) litPageInfo.Text = string.Format(Resources.Message.PagingInfo, gvItems.PageIndex + 1, gvItems.PageCount, HttpContext.Current.Items["PartCount"]);
	}
	protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		GridView gv = (GridView)sender;
		Literal lit = (Literal)WebTools.FindControlById("Literal7", e.Row);
		if (lit != null)
		{
			lit.Text = NumberFormatHelper.StrDoubleToStr(lit.Text, "en-US");
		}
	}
}