using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using NHibernate;
using NHibernate.Expression;
using Resources;
using VDMS.Common.Utils;
using VDMS.Common.Web;
using VDMS.Common.Web.Validator;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.Data.DAL2;
using VDMS.Data.IDAL.Interface;
using VDMS.Data.TipTop;
using VDMS.Helper;
using VDMS.I.Entity;
using VDMS.I.Linq;
using VDMS.I.ObjectDataSource;
using VDMS.I.Vehicle;
using VDMS.II.Linq;
using Item = VDMS.I.Entity.Item;
using ShippingDetail = Resources.ShippingDetail;
using ShippingHeader = VDMS.Core.Domain.ShippingHeader;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (drSH.ShipDate >= DealerHelper.GetAutoVehicleStartDate(drSH.DealerCode) && drSH.ShipDate.AddHours(DealerHelper.GetAutoVehicleDealerSpan(drSH.DealerCode)) >= DateTime.Now)
        //string VouchersStatus = "0";
        //var sdc = VDMS.II.Linq.DCFactory.GetDataContext<VDMS.I.Linq.VehicleDataContext>();
        //var query = sdc.ItemInstances.Where(i => i.DealerCode == "NKP001C")
        //                             .Where(i => ItemHepler.GetInstockItemStatus().Contains(i.Status));
        //if (VouchersStatus == "0")
        //{
        //    query = query.Where(i => i.ShippingDetails.Where(s => s.VoucherStatus == 0).Count() > 0);
        //    if (query.Count() == 0)
        //        query = query.Where(i => i.ShippingDetails.Where(s => s.ShippingHeader.DealerCode == i.DealerCode).OrderByDescending(s => s.ShippingDetailId).FirstOrDefault() == null);
        //}
        //else
        //{
        //    query = query.Where(i => i.ShippingDetails.Where(s => s.ShippingHeader.DealerCode == i.DealerCode && s.VoucherStatus == 1).OrderByDescending(s => s.ShippingDetailId).FirstOrDefault() != null);
        //}
        //gv.DataSource = query;
        //gv.DataBind();
        
    }
}
