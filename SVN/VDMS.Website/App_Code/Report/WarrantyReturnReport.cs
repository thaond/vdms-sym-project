using System;
using System.Collections.Generic;
using System.Linq;
using VDMS.Common.Utils;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.Report
{
	public class WarrantyReturnReport
	{
		int warrantyPartsCount;
		public int CountWarrantyParts()
		{
			return warrantyPartsCount;
		}

		public List<NGFormDetail> GetWarrantyParts(string dealerCode, string fromDate, string toDate, string status, string dbCode, string partCode)
		{
			DateTime dtFrom = DataFormat.DateFromString(fromDate);
			DateTime dtTo = DataFormat.DateFromString(toDate);
			if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;
			if (partCode == null) partCode = ""; else partCode = partCode.Trim();

			var dc = DCFactory.GetDataContext<PartDataContext>();
			var query = dc.NGFormDetails.Where(nd => nd.NGFormHeader.CreatedDate >= dtFrom && nd.NGFormHeader.CreatedDate <= dtTo)
										.Where(nd => nd.PartCode.Contains(partCode))
										.Where(nd => nd.NGFormHeader.DealerCode == dealerCode)
										.Where(nd => nd.NGFormHeader.NGType == NGType.Special);
			if (!string.IsNullOrEmpty(dbCode)) query = query.Where(nd => nd.NGFormHeader.Dealer.DatabaseCode == dbCode);
			if (!string.IsNullOrEmpty(status)) query = query.Where(nd => nd.NGFormHeader.Status == status);

			warrantyPartsCount = query.Count();
			return query.ToList();//.Skip(startRowIndex).Take(maximumRows);
		}
	}
}