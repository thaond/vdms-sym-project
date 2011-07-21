using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration.Provider;
using VDMS.II.Common.Web;
using System.Web.Security;
using VDMS.Data.DAL2;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnResetAdminAcc_Click(object sender, EventArgs e)
    {
        MembershipProvider mp = Membership.Providers["AspNetOracleMembershipProvider"];
        string user = txtUserO.Text;
        if (string.IsNullOrEmpty(user)) user = "admin";
        mp.UnlockUser(user);
        Response.Write(mp.ChangePassword(user, mp.ResetPassword(user, null), user));
    }

    protected void cmdResetSequence_Click(object sender, EventArgs e)
    {
        string[] list = new string[] { "V2_P_ACCESSORY", "V2_P_ACCESSORY_TYPE", "V2_P_CATEGORY", "V2_P_CONTACT", "V2_P_CUSTOMER",
			"V2_P_CYCLE_COUNT_DETAIL", "V2_P_CYCLE_COUNT_HEADER", "V2_P_DEALER", "V2_P_FAVORITE", "V2_P_INVENTORY", "V2_P_INVENTORY_LOCK",
			"V2_P_N_G_FORM_DETAIL", "V2_P_N_G_FORM_HEADER", "V2_P_ORDER_DETAIL", "V2_P_ORDER_HEADER", "V2_P_PART_INFO",
			"V2_P_PART_SAFETY",	"V2_P_RECEIVE_DETAIL", "V2_P_RECEIVE_HEADER", "V2_P_SALES_DETAIL", "V2_P_SALES_HEADER",
			"V2_P_SYSTEM_DATA",	"V2_P_TRANSACTION_HISTORY",	"V2_P_TRANSFER_DETAIL", "V2_P_TRANSFER_HEADER",
			"V2_P_VENDOR", "V2_P_WAREHOUSE"
		};
        foreach (string s in list)
            DatabaseDao.ResetSequence2(s);
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        MembershipProvider mp = Membership.Providers["AspNetOracleMembershipProvider"];
        string user = txtUserN.Text;
        if (string.IsNullOrEmpty(user)) user = "admin";
        MembershipCreateStatus status;
        mp.CreateUser(user, user, string.Empty, string.Empty, string.Empty, true, Guid.NewGuid(), out status);
		Roles.AddUserToRole(user, "Administrators");
        Response.Write(status);
    }
    protected void cmdResetSequenceI_Click(object sender, EventArgs e)
    {
        string[] list = new string[] { "sale_OrderDetail", "sale_OrderHeader", "sale_ShippingDetail", "sale_ShippingHeader", "sale_ReturnItem", // order and shipping
			"sale_Payment", "sale_BatchInvoiceDetail", "sale_BatchInvoiceHeader", "data_SubShop", "sale_Invoice", "sale_SellItem", // sell
			"ser_ServiceDetail", "ser_ExchangePartDetail", "ser_ExchangePartHeader", "ser_ServiceHeader", "data_Broken", // service
			"sym_Customer", // customer
			"sale_TransHis", "sale_InventoryLock", "sale_InventoryDay", "sale_Inventory", // inventory
			"data_ItemInstance", // main table
			"data_WarrantyCondition",
			"sym_Attachment", "sym_Articles",
			"app_RolesInTasks", "app_Tasks" // permission
		};
        foreach (string s in list)
            DatabaseDao.ResetSequence(s);
    }
}
