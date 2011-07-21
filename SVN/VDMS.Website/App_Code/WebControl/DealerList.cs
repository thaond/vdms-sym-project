using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using Resources;
using VDMS.Helper;
using VDMS.II.Entity;
using VDMS.II.BasicData;

namespace VDMS.II.WebControls
{
	public class DealerList : DropDownList
	{
		public bool EnabledSaperateByDB { get; set; }
		public bool EnabledSaperateByArea { get; set; }
        public string DatabaseCode { get; set; }
        public string AreaCode { get; set; }
		public string RootDealer { get; set; }
		public bool MergeCode { get; set; }

		[Description("Enable or disable adding an 'Select All' item when databound."), Category("Misc"), DefaultValue("false"),]
		public bool ShowSelectAllItem { get; set; }
		public bool ShowEmptyItem { get; set; }

		public bool RemoveRootItem { get; set; }
		public string RemoveBranch { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			if ((!Page.IsPostBack) && (this.Items.Count == 0))
			{
				this.DataBind();
			}
		}

		public override void DataBind()
		{
			this.DataTextField = "Name";
			this.DataValueField = "Code";
            if(this.Width.IsEmpty)
                this.Width = new Unit(250, UnitType.Pixel);
			this.Items.Clear();

			GetTreeCategory();
			try
			{
				this.DataSource = result;
				base.DataBind();
				if (MergeCode)
				{
					foreach (ListItem item in this.Items)
					{
						item.Text = string.Format("{0} - {1}", item.Value, item.Text);
					}
				}
			}
			catch { }
		}

		protected override void OnDataBound(EventArgs e)
		{
			if (this.ShowSelectAllItem) this.Items.Insert(0, new ListItem(Constants.All, ""));
			if (this.ShowEmptyItem) this.Items.Insert(0, new ListItem("", ""));
			//this.SelectedIndex = 0;
			base.OnDataBound(e);
		}

		List<object> result = null;
		void GetTreeCategory()
		{
			result = new List<object>();
			if (string.IsNullOrEmpty(RootDealer)) RootDealer = UserHelper.DealerCode;

			if (!this.RemoveRootItem)
			{
				Dealer root = DealerDAO.GetDealerByCode(RootDealer);
				result.Add(new { Code = root.DealerCode, Name = root.DealerName });
			}
			PopulateTree(RootDealer, (this.RemoveRootItem) ? 0 : 1);
		}

		void PopulateTree(string ParentCode, int level)
		{
			if (ParentCode == string.Empty) ParentCode = null;
			IEnumerable<Dealer> e;
			if (ParentCode == null)
			{
				//if (UserHelper.IsDealer) e = GlobalData.Dealers.Where(p => p.ParentCode == UserHelper.DealerCode);
				//else e = GlobalData.Dealers.Where(p => p.ParentCode == "/");
				e = DealerHelper.Dealers.Where(p => p.ParentCode == UserHelper.DealerCode);
			}
			else e = DealerHelper.Dealers.Where(p => p.ParentCode == ParentCode);
            
            //fillter by db code
			if (!string.IsNullOrEmpty(this.DatabaseCode))
				e = e.Where(p => p.DatabaseCode == this.DatabaseCode);
			else 
                if (EnabledSaperateByDB) e = e.Where(p => p.DatabaseCode == UserHelper.DatabaseCode);

            //fillter by area code
            if (!string.IsNullOrEmpty(this.AreaCode))
                e = e.Where(p => p.AreaCode == this.AreaCode);
            else
                if (EnabledSaperateByArea) e = e.Where(p => p.AreaCode == UserHelper.AreaCode);

			e = e.OrderBy(p => p.DealerCode);
			e.ToList().ForEach(
				p =>
				{
					if (p.DealerCode != this.RemoveBranch)
					{
						result.Add(new { Code = p.DealerCode, Name = string.Concat(new string('-', level * 3), p.DealerName) });
						PopulateTree(p.DealerCode, level + 1);
					}
				});
		}
	}
}