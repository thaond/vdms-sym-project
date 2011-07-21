using System;
using System.Web.UI.WebControls;
using VDMS.Helper;

namespace VDMS.II.WebControls
{
	public class DealerPlaceHolder : PlaceHolder
	{
		public string VisibleBy { get; set; }
		public bool AdminOnly { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				switch (VisibleBy)
				{
					case "Dealer":
						if (!UserHelper.IsDealer) Visible = false;
						break;
					case "VMEP":
						if (UserHelper.IsDealer) Visible = false;
						break;
					default:
						break;
				}
				if (AdminOnly && !UserHelper.IsAdmin) Visible = false;
			}
		}
	}
}