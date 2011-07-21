using System.Threading;
using VDMS.I.Service;
using VDMS.Core.Domain;

namespace VDMS.I.Report.DataObject
{
	public class RptExchangePart : Exchangepartdetail
	{
		public string PartNameVn { get; set; }
		public string PartNameEn { get; set; }
		public string PartName { get; set; }
		public string BrokenCode { get; set; }
		public decimal Labour { get; set; }

		public RptExchangePart() { }

		public RptExchangePart(Exchangepartdetail spare)
		{
			if (spare != null)
			{
				Warrantycondition warr = WarrantyContent.GetWarrantyCondition(spare.Partcodem);
				if (warr != null)
				{
					this.Labour = warr.Labour;
					this.PartNameEn = warr.Partnameen;
					this.PartNameVn = warr.Partnamevn;
					this.PartName = (Thread.CurrentThread.CurrentCulture.Name == "vi-VN") ? warr.Partnamevn : warr.Partnameen;
				}
				else
				{
					//this.PartName = Message.DataLost; 
				}

				this.Broken = spare.Broken;
				this.BrokenCode = spare.Broken.Brokencode;
				this.Exchangepartheader = spare.Exchangepartheader;
				this.Id = spare.Id;
				this.Partcodem = spare.Partcodem;
				this.Partcodeo = spare.Partcodeo;
				this.Partqtym = spare.Partqtym;
				this.Partqtyo = spare.Partqtyo;
				this.Serialnumber = spare.Serialnumber;
				this.Totalfeem = spare.Totalfeem;
				this.Totalfeeo = spare.Totalfeeo;
				this.Unitpricem = spare.Unitpricem;
				this.Unitpriceo = spare.Unitpriceo;
				this.Vmepcomment = spare.Vmepcomment;
			}
		}
	}
}