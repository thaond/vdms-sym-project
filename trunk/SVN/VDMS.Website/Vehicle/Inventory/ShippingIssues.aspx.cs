using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.I.Vehicle;
using VDMS.II.Linq;
using VDMS.Helper;
using System.Data;

public partial class Vehicle_Inventory_ShippingIssues : BasePage
{
	public string OrderNumber
	{
		get { return Request.QueryString["on"]; }
	}
	private List<ShippingDetail> _ShippingDetails;
	private List<ShippingDetail> ShippingDetails
	{
		get
		{
			if (_ShippingDetails == null)
			{
				_ShippingDetails = DCFactory.GetDataContext<VehicleDataContext>().ShippingDetails.Where(d => d.OrderNumber == OrderNumber).ToList();
			}
			return _ShippingDetails;
		}
	}

	protected object EvalStatus(string engineNo)
	{
		int status = -100;
		var dc = DCFactory.GetDataContext<VehicleDataContext>();

		ItemInstance iis = dc.ItemInstances.SingleOrDefault(d => d.EngineNumber == engineNo);
		if (iis == null)
		{
			ShippingDetail sd = ShippingDetails.OrderByDescending(d=>d.ShippingDetailId).FirstOrDefault(d => d.EngineNumber == engineNo);
			if (sd != null)
			{
				status = sd.Status;
			}
		}
		else
		{
			status = iis.Status;
		}
		return ItemHepler.GetNativeItemStatusName(status);
	}
	protected object EvalImportNote(string engineNo)
	{
        ShippingDetail sd = ShippingDetails.OrderByDescending(d => d.ShippingDetailId).FirstOrDefault(d => d.EngineNumber == engineNo);
		string note = "";
		if (sd != null)
		{
			note = sd.Exception;
		}
		return note;
	}

	protected object GetIssueDetail(string shippNumber)
	{
		//return Shipping.GetShippingDetail(shippNumber, OrderNumber);
		return VDMS.I.ObjectDataSource.ShippingDetailDataSource.GetShippingDetail(shippNumber, OrderNumber);
	}

    protected string GetInfoIssueDetail(string field,string issueno)
    {
        DataTable tb = VDMS.Data.TipTop.Shipping.GetInfoOfIssue(issueno, UserHelper.DatabaseCode);
        //DataTable tb = VDMS.Data.TipTop.Shipping.GetInfoOfIssue("DTA-840052", "DNF");
        if(tb.Rows.Count > 0)
        {
            if (string.IsNullOrEmpty(tb.Rows[0][field].ToString()))
                return string.Empty;
            else
                return tb.Rows[0][field].ToString();
        }
        else
            return string.Empty;
    }

	protected void litTitle_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			Literal lit = (Literal)sender;
			lit.Text = string.Format(lit.Text, OrderNumber);
		}
	}
}
