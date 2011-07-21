using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Common.Web;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.IDAL.Interface;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.ObjectDataSource;

public partial class Admin_Database_ListMotorSale : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
	}

	protected void ImageButton1_Load(object sender, EventArgs e)
	{
		((ImageButton)sender).OnClientClick = DeleteConfirmQuestion;
	}

	protected void btnTest_Click(object sender, EventArgs e)
	{
		gvItem.Visible = true;
        if (UserHelper.DatabaseCode.Equals("DNF", StringComparison.OrdinalIgnoreCase))
        {
            gvItem.Columns[1].Visible = false;
            gvItem.Columns[2].Visible = true;
        }
        else
        {
            gvItem.Columns[1].Visible = true;
            gvItem.Columns[2].Visible = false;
        }

        gvItem.DataSourceID = ObjectDataSource1.ID;
        gvItem.DataBind();
	}

	protected void gvItem_PreRender(object sender, EventArgs e)
	{
		if (gvItem.TopPagerRow == null) return;
		Literal litPageInfo = gvItem.TopPagerRow.FindControl("litPageInfo") as Literal;
        if (litPageInfo != null) litPageInfo.Text = string.Format(Message.PagingInfo, gvItem.PageIndex + 1, gvItem.PageCount, HttpContext.Current.Items["listRowCount"]);

	}

	protected void gvItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		e.Cancel = DataItemDataSource.HasChildRow(e.Keys[0].ToString());
		if (e.Cancel) MessageBox.Show(Message.ListMotoSale_CantDelete_Childrow);
	}

	// empty template
	protected void Label5_Load(object sender, EventArgs e)
	{
		((Label)sender).Text = Message.ListMotoSale_NoItemFound;
	}

	protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		// insert Item No
		Literal lit = (Literal)e.Row.FindControl("litNo");
		GridView gv = ((GridView)sender);
		if (lit != null)
		{
			int no = gv.PageIndex * gv.PageSize + e.Row.RowIndex + 1;
			lit.Text = no.ToString();
		}

		// format price
		lit = (Literal)e.Row.FindControl("litPrice");
		if (lit != null)
		{
			long price;
			long.TryParse(lit.Text, out price);
			NumberFormatInfo ni = Thread.CurrentThread.CurrentCulture.NumberFormat;
			ni.CurrencyDecimalDigits = 0;
			ni.CurrencySymbol = "";
			lit.Text = price.ToString("C", ni);
		}
	}

	protected void cmdSynchronize_Click(object sender, EventArgs e)
	{
		DataSet ds = Motorbike.GetAllAvailable();

		IDao<Item, string> dao = DaoFactory.GetDao<Item, string>();
		using (TransactionBlock tran = new TransactionBlock())
		{
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				Item item1 = dao.GetById((string)row["ItemCode"], false); //true -> false
				if (item1 != default(Item))
				{
					if (item1.DatabaseCode != null && item1.DatabaseCode.IndexOf(UserHelper.DatabaseCode) > -1)
						continue;
					item1.DatabaseCode += UserHelper.DatabaseCode;
					dao.SaveOrUpdate(item1);
					continue;
				}

				Item item = new Item();
				item.Id = (string)row["ItemCode"];
				if (!row.IsNull("ItemName")) item.Itemname = (string)row["ItemName"];
				else item.Itemname = " ";
				item.Itemtype = ((string)row["ItemCode"]).Split('-')[0];
				item.Price = (long)(decimal)row["Price"];
				item.Colorcode = ((string)row["ColorCode"]).Trim();
				if (!row.IsNull("ColorName")) item.Colorname = ((string)row["ColorName"]).Trim();
				else item.Colorname = " ";
				item.DatabaseCode = UserHelper.DatabaseCode;
				//item.Available = false;

				dao.Save(item);
			}
			tran.IsValid = true;
		}
		ReLoad();
	}
}
