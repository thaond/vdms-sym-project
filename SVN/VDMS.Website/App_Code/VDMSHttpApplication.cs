using System;
using System.Configuration;
using System.Web;
using VDMS.Helper;
using VDMS.II.PartManagement;
using VDMS.II.PartManagement.Order;
using VDMS.II.PartManagement.Sales;
using VDMS.Provider;
using VDMS.Common.Utils;

namespace VDMS.Web
{
	public class VDMSHttpApplication : HttpApplication
	{
        public static Scheduler scheduler = new Scheduler();
        public static BGScheduler bgscheduler = new BGScheduler();
        public static CloseSchedulerI closeSchedulerI = new CloseSchedulerI();
        public static SyncPartSchedulerI syncPartSchedulerI = new SyncPartSchedulerI();

		public void Application_Start(object sender, EventArgs e)
		{
            DealerHelper.Init();
            AreaHelper.Init();
            scheduler.Start(5);
            bgscheduler.Start(60000);
            // auto close month.I
            closeSchedulerI.Start(3);
            // sync VDMS-I parts
            syncPartSchedulerI.Start(3);

            PartOrderDAO.Init();
            PartSalesDAO.Init();
            NotGoodManualDAO.Init();

            // other
            if (VDMSProvider.CrystalJobsLimit.HasValue) ReportFactory.JobsLimit = VDMSProvider.CrystalJobsLimit.Value;
		}

		public void Application_End(object sender, EventArgs e)
		{
            scheduler.Dispose();
            closeSchedulerI.Dispose();
            syncPartSchedulerI.Dispose();
            bgscheduler.Dispose();
            scheduler = null;
            syncPartSchedulerI = null;
            bgscheduler = null;
            closeSchedulerI = null;
		}

		public void Application_Error(object sender, EventArgs e)
		{
			ErrorHelper.Exception = Server.GetLastError();
		}

		public void Session_Start(object sender, EventArgs e)
		{
			string name = ConfigurationManager.AppSettings["Lang1"];
            VDMSProvider.Language = string.IsNullOrEmpty(name) ? "vi-VN" : name;

            HttpRequest httpRequest = HttpContext.Current.Request;
            if (httpRequest.Browser.IsMobileDevice)
            {
                string path = httpRequest.Url.PathAndQuery;
                bool isOnMobilePage = path.StartsWith("/Mobile/",
                                       StringComparison.OrdinalIgnoreCase);
                if (!isOnMobilePage)
                {
                    string redirectTo = "~/Mobile/";
                    HttpContext.Current.Response.Redirect(redirectTo);
                }
            }
		}

		public void Session_End(object sender, EventArgs e)
		{
            VDMS.II.BonusSystem.BonusPlans.CleanSessionItems(Session.SessionID);
            VDMS.II.BonusSystem.OrderBonus.CleanSessionOrderPayments(Session.SessionID);
            VDMS.I.Vehicle.PaymentManager.CleanSession(Session.SessionID);
        }
	}
}