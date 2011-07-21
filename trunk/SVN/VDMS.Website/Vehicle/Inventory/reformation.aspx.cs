using System;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.UI;
using NHibernate;
using NHibernate.Expression;
using VDMS.Core.Domain;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.Vehicle;

public partial class Sales_Inventory_reformation : BasePage
{
	public sealed class NHibernateHelper
	{
		private const string CurrentSessionKey = "nhibernate.current_session";
		private static readonly ISessionFactory sessionFactory;
		static NHibernateHelper()
		{
			sessionFactory = new NHibernate.Cfg.Configuration().Configure().BuildSessionFactory();
		}
		public static ISession GetCurrentSession()
		{
			HttpContext context = HttpContext.Current;
			ISession currentSession = context.Items[CurrentSessionKey] as ISession;
			if (currentSession == null)
			{
				currentSession = sessionFactory.OpenSession();
				context.Items[CurrentSessionKey] = currentSession;
			}
			return currentSession;
		}
		public static void CloseSession()
		{
			HttpContext context = HttpContext.Current;
			ISession currentSession = context.Items[CurrentSessionKey] as ISession;
			if (currentSession == null)
			{
				//Nocurrentsession
				return;
			}
			currentSession.Close();
			context.Items.Remove(CurrentSessionKey);
		}
		public static void CloseSessionFactory()
		{
			if (sessionFactory != null)
			{
				sessionFactory.Close();
			}
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{

		}
		lbMes.Visible = false;
	}

	protected void btnApply_Click(object sender, EventArgs e)
	{
		string EngStr = txtEngineNo.Text.Trim();
		string EngDropStr = txtEngineNoCancel.Text.Trim();
		if (txtEngineNo.Text.Trim() != "" && txtEngineNoCancel.Text.Trim() != "")
		{
			ISession sess = NHibernateHelper.GetCurrentSession();
			ITransaction tx = sess.BeginTransaction();
			Iteminstance newItemIns = GetMotorDatabyEngineNumber(ref sess, ref EngStr);
			if (newItemIns != null)
			{
				liMes.Text = Resources.Message.EngineNumberIsHaved;
				//plInf.Visible = false;
				return;
			}
			Iteminstance itemins = GetMotorDatabyEngineNumberImported(ref sess, ref EngDropStr);
			itemins.Enginenumber = EngStr;
			if (itemins.Status.Equals((int)ItemStatus.AdmitTemporarily))
			{
				itemins.Status = (int)ItemStatus.Imported;
			}

			Shippingdetail sd = GetShippingdetailbyEngineNumber(ref sess, ref EngDropStr);
			sd.Enginenumber = EngStr;

			try
			{
				sess.SaveOrUpdate(itemins);
				sess.SaveOrUpdate((new ItemHepler()).SaveTranHis(itemins, DateTime.Now, ItemStatus.ChangeEngineNumber, 0, 0, UserHelper.Fullname, EngDropStr, UserHelper.FullBranchCode, null));
				sess.SaveOrUpdate(sd);
				Invoice inv = GetInvoiceInsbyEngineNumber(ref sess, ref EngDropStr);
				if (inv != null)
				{
					inv.Enginenumber = EngStr;
					sess.SaveOrUpdate(inv);
				}
				tx.Commit();
				lbMes.Visible = true;
				lbMes.Text = Resources.Message.ActionSucessful;
			}
			catch (Exception)
			{
				tx.Rollback();
			}

			NHibernateHelper.CloseSession();
			refreshData();
		}
	}
	private void ShowMessage(string mesg, bool isError)
	{
		lbMes.Visible = true;
		lbMes.ControlStyle.ForeColor = (isError) ? Color.Red : Color.RoyalBlue;
		lbMes.Text = mesg;
	}
	private bool CheckAppliable(Iteminstance itemins)
	{
		switch (Motorbike.CheckExchangeEngineNo(txtEngineNo.Text.Trim(), txtEngineNoCancel.Text.Trim(), itemins.Item.Id.ToString()))
		{
			case 0:
				return true;
			case -1:
				ShowMessage(Resources.Message.Reformation_ApplyCase1, true);
				return false;
			case -2:
				ShowMessage(Resources.Message.Reformation_ApplyCase2, true);
				return false;
			case -3:
				ShowMessage(Resources.Message.Reformation_ApplyCase3, true);
				return false;
			default: return false;
		}
	}

	protected void btnTest_Click(object sender, EventArgs e)
	{
		plInf.Visible = true;
		string EngStr = txtEngineNo.Text.Trim();
		string EngDropStr = txtEngineNoCancel.Text.Trim();
		ISession sess = NHibernateHelper.GetCurrentSession();
		ITransaction tx = sess.BeginTransaction();

		//Test Engine number
		Iteminstance itemins = GetMotorDatabyEngineNumberImported(ref sess, ref EngDropStr);

		if (itemins != null)
		{
			if (!CheckAppliable(itemins))
			{
				plInf.Visible = false;
				return;
			}
			lMotorType.Text = itemins.Itemtype;
			lColor.Text = itemins.Color;
			lProduceDate.Text = (itemins.Madedate.Equals(DateTime.MinValue)) ? "" : itemins.Madedate.ToShortDateString();
			lCurrStore.Text = itemins.Dealercode;
			lCurrStatus.Text = ItemHepler.GetNativeItemStatusName((ItemStatus)itemins.Status);

			Shippingdetail sd = GetShippingdetailbyEngineNumber(ref sess, ref EngDropStr);
			if (sd != null)
			{
				lVoucher.Text = (sd.Voucherstatus) ? Resources.Constants.Yes : Resources.Constants.No;
			}
			else lVoucher.Text = Resources.Constants.No;

			lDMotorType.Text = lMotorType.Text;
			lDColor.Text = lColor.Text;
			lDProduceDate.Text = lProduceDate.Text;
			lDCurrStore.Text = lCurrStore.Text;
			lDCurrStatus.Text = lCurrStatus.Text;
			lDVoucher.Text = lVoucher.Text;

			txtEngineNoCancel.Enabled = false;
			btnTest.Enabled = false;
			btnApply.Enabled = true;
		}
		else
		{
			ShowMessage(Resources.Message.EngineNumberNotFoundInDB, true);
			btnApply.Enabled = false;
			plInf.Visible = false;
		}

		tx.Commit();
		NHibernateHelper.CloseSession();

	}

	private Shippingdetail GetShippingdetailbyEngineNumber(ref ISession sess, ref string EngStr)
	{
		ITransaction tx = sess.BeginTransaction();
		IList lstShippingDetail = sess.CreateCriteria(typeof(Shippingdetail))
										.Add(Expression.Eq("Enginenumber", EngStr))
										.AddOrder(NHibernate.Expression.Order.Desc("Id")).List();
		if (lstShippingDetail.Count > 0)
		{
			return (Shippingdetail)lstShippingDetail[0];
		}
		else return null;
	}

	private Iteminstance GetMotorDatabyEngineNumberImported(ref ISession sess, ref string EngStr)
	{
		ITransaction tx = sess.BeginTransaction();
		IList lstItemIns = sess.CreateCriteria(typeof(Iteminstance))
			.Add(Expression.Eq("Databasecode", UserHelper.DatabaseCode))
			//.Add(Expression.Eq("Dealercode", UserHelper.DealerCode))
			//.Add(Expression.Eq("Branchcode", UserHelper.BranchCode))
			.Add(Expression.Eq("Enginenumber", EngStr))
			.Add(Expression.In("Status", new object[] { (int)ItemStatus.Imported, (int)ItemStatus.AdmitTemporarily }))
			.List();
		if (lstItemIns.Count > 0)
		{
			return (Iteminstance)lstItemIns[0];
		}
		else return null;
	}

	private Iteminstance GetMotorDatabyEngineNumber(ref ISession sess, ref string EngStr)
	{
		IList lstItemIns = sess.CreateCriteria(typeof(Iteminstance)).Add(Expression.Eq("Enginenumber", EngStr)).List();
		if (lstItemIns.Count > 0)
		{
			return (Iteminstance)lstItemIns[0];
		}
		else return null;
	}

	private Invoice GetInvoiceInsbyEngineNumber(ref ISession sess, ref string EngStr)
	{
		IList lstInvoice = sess.CreateCriteria(typeof(Invoice)).Add(Expression.Eq("Enginenumber", EngStr)).List();
		if (lstInvoice.Count > 0)
		{
			return (Invoice)lstInvoice[0];
		}
		else return null;
	}

	private void refreshData()
	{
		txtEngineNo.Text = "";
		txtEngineNoCancel.Enabled = true; txtEngineNoCancel.Text = "";
		btnTest.Enabled = true;
		plInf.Visible = false;
	}

	protected void btnCancel_Click(object sender, EventArgs e)
	{
		refreshData();
	}
}