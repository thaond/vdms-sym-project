using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Helper;
using NHibernate.Expression;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;

public partial class Admin_Database_Branch : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
	}

	protected void btnAdd_Click(object sender, EventArgs e)
	{
		Subshop ss = new Subshop();
		ss.Status = true;
		SubShopToEdit = ss;
		mvMain.ActiveViewIndex = 1;
	}

	Subshop _subshop = null;
	protected void Page_Init(object sender, EventArgs e)
	{
		this.Page.RegisterRequiresControlState(this);
	}

	protected override void LoadControlState(object savedState)
	{
		object[] ctlState = (object[])savedState;
		base.LoadControlState(ctlState[0]);
		this._subshop = (Subshop)ctlState[1];
	}

	protected override object SaveControlState()
	{
		object[] ctlState = new object[2];
		ctlState[0] = base.SaveControlState();
		ctlState[1] = this._subshop;
		return ctlState;
	}

	public Subshop SubShopToEdit
	{
		get
		{
			_subshop.Address = txtAddress.Text;
			_subshop.Code = txtCode.Text.ToUpper();
			_subshop.Dealercode = UserHelper.DealerCode;
			_subshop.Name = txtName.Text;
			_subshop.Status = !chkStatus.Checked;
			return _subshop;
		}
		set
		{
			_subshop = value;
			txtAddress.Text = _subshop.Address;
			txtCode.Text = _subshop.Code;
			txtName.Text = _subshop.Name;
			chkStatus.Checked = !_subshop.Status;
		}
	}

	int count = 0;
	protected void gvSubShop_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			count++;
			e.Row.Cells[0].Text = ((gvSubShop.PageIndex) * gvSubShop.PageSize + count).ToString();
			ImageButton btn = e.Row.Cells[6].Controls[1] as ImageButton;
			btn.OnClientClick = DeleteConfirmQuestion;
		}
	}

	protected void gvSubShop_DataBound(object sender, EventArgs e)
	{
		if (gvSubShop.TopPagerRow != null)
		{
			Literal litPageInfo = gvSubShop.TopPagerRow.FindControl("litPageInfo") as Literal;
			if (litPageInfo != null) litPageInfo.Text = string.Format(Resources.Message.PagingInfo, gvSubShop.PageIndex + 1, gvSubShop.PageCount, HttpContext.Current.Items["rowCount"]);
		}
	}

	protected void gvSubShop_RowEditing(object sender, GridViewEditEventArgs e)
	{
		IDao<Subshop, long> subshopDao = DaoFactory.GetDao<Subshop, long>();
		long id = (long)gvSubShop.DataKeys[e.NewEditIndex].Value;
		SubShopToEdit = subshopDao.GetById(id, false);
		txtCode.ReadOnly = true;
		e.NewEditIndex = -1;
		mvMain.ActiveViewIndex = 1;
	}

	protected void btnUpdate_OnClick(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			Subshop obj = SubShopToEdit;
			IDao<Subshop, long> subshopDao = DaoFactory.GetDao<Subshop, long>();
			if (obj.Id == 0)
			{
				subshopDao.SetCriteria(new ICriterion[] { Expression.Eq("Code", obj.Code), Expression.Eq("Dealercode", UserHelper.DealerCode) });
				if (subshopDao.GetCount() > 0)
				{
					litError.Visible = true;
					return;
				}
			}
			
			subshopDao.SaveOrUpdate(SubShopToEdit);
		}
		ReLoad();
	}

	protected void btnCancel_Click(object sender, EventArgs e)
	{
		ReLoad();
	}
}
