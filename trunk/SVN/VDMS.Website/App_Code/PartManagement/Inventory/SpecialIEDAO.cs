using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.PartManagement;

public enum SpecialIEType
{
	Import,
	Export
}

[DataObject(true)]
public class SpecialIE
{
	public Guid PartKey { get; set; }
	public string PartNo { get; set; }
	public int Quantity { get; set; }
	public long VendorId { get; set; }
	public string Remark { get; set; }
	public string PartType { get; set; }
	public string State { get; set; }
	public int UnitPrice { get; set; }
	public int Amount { get { return this.Quantity * this.UnitPrice; } }

	public SpecialIE()
	{
		this.State = SIE_ItemState.New;
	}
}

public class SpecialIEDAO
{
	public SpecialIEDAO() { }

	private static List<SpecialIE> SIEParts(string key, SpecialIEType type)
	{
		List<SpecialIE> parts = HttpContext.Current.Session[key + type.ToString()] as List<SpecialIE>;
		if (parts == null)
		{
			parts = new List<SpecialIE>();
			HttpContext.Current.Session[key + type.ToString()] = parts;
		}
		return parts;
	}

	#region DataObjectMethod

	public static int CountImportParts(string key, int maximumRows, int startRowIndex)
	{
		return SIEParts(key, SpecialIEType.Import).Count;
	}
	public static int CountExportParts(string key, int maximumRows, int startRowIndex)
	{
		return SIEParts(key, SpecialIEType.Export).Count;
	}

	[DataObjectMethod(DataObjectMethodType.Select)]
	public static IEnumerable<SpecialIE> FindImport(string key, int maximumRows, int startRowIndex)
	{
		return SIEParts(key, SpecialIEType.Import).Skip(startRowIndex).Take(maximumRows);
	}
	[DataObjectMethod(DataObjectMethodType.Select)]
	public static IEnumerable<SpecialIE> FindExport(string key, int maximumRows, int startRowIndex)
	{
		return SIEParts(key, SpecialIEType.Export).Skip(startRowIndex).Take(maximumRows);
	}

	[DataObjectMethod(DataObjectMethodType.Delete)]
	public static void DeleteImportPart(string key, object original_PartKey)
	{
		List<SpecialIE> parts = SIEParts(key, SpecialIEType.Import);
		parts.Remove(parts.SingleOrDefault(p => p.PartKey == (Guid)original_PartKey));
	}
	[DataObjectMethod(DataObjectMethodType.Delete)]
	public static void DeleteExportPart(string key, object original_PartKey)
	{
		List<SpecialIE> parts = SIEParts(key, SpecialIEType.Export);
		parts.Remove(parts.SingleOrDefault(p => p.PartKey == (Guid)original_PartKey));
	}

	#endregion

	/// <summary>
	/// update part information (in memory)
	/// </summary>
	/// <param name="key"></param>
	/// <param name="type"></param>
	/// <param name="partKey"></param>
	/// <param name="partNo"></param>
	/// <param name="partType"></param>
	/// <param name="vendorId"></param>
	/// <param name="qty"></param>
	/// <param name="unitPrice"></param>
	/// <param name="remark"></param>
	public static void UpdatePart(string key, SpecialIEType type, object partKey, string partNo, string partType, long? vendorId, int? qty, int? unitPrice, string remark)
	{
		SpecialIE part = SIEParts(key, type).SingleOrDefault(p => p.PartKey == (Guid)partKey);
		if (part != null)
		{
			if (partNo != null) part.PartNo = partNo;
			if (vendorId != null) part.VendorId = (long)vendorId;
			if (qty != null) part.Quantity = (int)qty;
			if (remark != null) part.Remark = remark;
			if (partType != null) part.PartType = partType;
			if (unitPrice != null) part.UnitPrice = (int)unitPrice;
			part.State = SIE_ItemState.New;
		}
	}

	// remove parts to be I/E from memory
	public static void ClearImportPart(string key)
	{
		SIEParts(key, SpecialIEType.Import).Clear();
	}
	public static void ClearExportPart(string key)
	{
		SIEParts(key, SpecialIEType.Export).Clear();
	}
	public static void ClearExportPart(string key, string state)
	{
		SIEParts(key, SpecialIEType.Export).RemoveAll(p => p.State == state);
	}
	public static void ClearImportPart(string key, string state)
	{
		SIEParts(key, SpecialIEType.Import).RemoveAll(p => p.State == state);
	}

	// get part(s) to be I/E from memory
	public static SpecialIE GetImportPart(string key, Guid partKey)
	{
		return SIEParts(key, SpecialIEType.Import).Single(p => p.PartKey == partKey);
	}
	public static List<SpecialIE> GetImportParts(string key)
	{
		return SIEParts(key, SpecialIEType.Import);
	}
	public static SpecialIE GetExportPart(string key, Guid partKey)
	{
		return SIEParts(key, SpecialIEType.Export).Single(p => p.PartKey == partKey);
	}
	public static List<SpecialIE> GetExportParts(string key)
	{
		return SIEParts(key, SpecialIEType.Export);
	}

	/// <summary>
	/// append several parts to IE parts list
	/// </summary>
	/// <param name="RowCount"></param>
	/// <param name="type"></param>
	/// <param name="key"></param>
	public static void Append(int RowCount, SpecialIEType type, string key)
	{
		List<SpecialIE> parts = SIEParts(key, type);
		for (int i = 0; i < RowCount; i++)
		{
			parts.Add(new SpecialIE() { PartKey = Guid.NewGuid() });
		}
	}
	/// <summary>
	/// append one part to parts list
	/// </summary>
	/// <param name="partCode"></param>
	/// <param name="type"></param>
	/// <param name="pType"></param>
	/// <param name="key"></param>
	public static void Append(string partCode, SpecialIEType type, string pType, string key)
	{
		List<SpecialIE> parts = SIEParts(key, type);
		SpecialIE part = parts.SingleOrDefault(p => p.PartNo == partCode && p.PartType == pType);
		if (part == null)
		{
			parts.Add(new SpecialIE()
			{
				PartKey = Guid.NewGuid(),
				PartNo = partCode,
				PartType = pType,
				UnitPrice = PartInfoDAO.GetPrice(partCode, pType),
			});
		}
	}

	/// <summary>
	/// Do special Import
	/// </summary>
	/// <param name="key"></param>
	/// <param name="warehouseId"></param>
	/// <returns></returns>
	public static string DoImport(string key, long warehouseId)
	{
		List<SpecialIE> parts = SIEParts(key, SpecialIEType.Import);
		DateTime tranDate = DateTime.Now;
		string vch = GetVoucherNo(key, InventoryAction.SpecialImport, UserHelper.DealerCode, warehouseId, tranDate);

		// remove empty parts
		parts.RemoveAll(p => (string.IsNullOrEmpty(p.PartNo)) || (p.PartNo.Trim() == "") || p.Quantity <= 0);
		// change inventory data
		parts.Where(p => p.State == SIE_ItemState.New).ToList().ForEach(p =>
		{
			// if part type has not been set, 
			// try to get as Accessory, if not exist assume as SYM part 
			if (string.IsNullOrEmpty(p.PartType))
			{
				p.PartType = AccessoryDAO.IsAccessoryExist(p.PartNo, UserHelper.DealerCode) ? PartType.Accessory : PartType.Part;
			}
			// change stock quantity
			if (PartDAO.StockAdjust(p.PartNo, p.PartType, UserHelper.DealerCode, warehouseId, null, tranDate, InventoryAction.SpecialImport, p.Amount, p.Quantity, p.Remark, vch, (p.VendorId <= 0) ? null : (long?)p.VendorId))
				p.State = SIE_ItemState.Imported;
			else p.State = SIE_ItemState.NotFound;
		});
		PartDAO.PartDC.SubmitChanges();
		return vch;
	}

	/// <summary>
	/// Do special Export
	/// </summary>
	/// <param name="key"></param>
	/// <param name="warehouseId"></param>
	/// <returns></returns>
	public static string DoExport(string key, long warehouseId)
	{
		List<SpecialIE> parts = SIEParts(key, SpecialIEType.Export);
		DateTime tranDate = DateTime.Now;
		string vch = GetVoucherNo(key, InventoryAction.SpecialExport, UserHelper.DealerCode, warehouseId, tranDate);

		// remove empty parts
		parts.RemoveAll(p => (string.IsNullOrEmpty(p.PartNo)) || (p.PartNo.Trim() == "") || p.Quantity <= 0);
		// change inventory data
		parts.Where(p => p.State == SIE_ItemState.New).ToList().ForEach(p =>
		{
			VDMS.II.Entity.PartSafety pi = PartInfoDAO.GetPartSafety(p.PartNo, warehouseId);
			if (pi != null)
			{
				// check current stock quanity before export
				if (pi.CurrentStock < p.Quantity)
				{
					throw new Exception(string.Format("Not enough part quantity for export: [{0}]!", p.PartNo));
				}
				// if part type has not been set, 
				// try to get as Accessory, if not exist assume as SYM part 
				if (string.IsNullOrEmpty(p.PartType))
				{
					p.PartType = AccessoryDAO.IsAccessoryExist(p.PartNo, UserHelper.DealerCode) ? PartType.Accessory : PartType.Part;
				}
				// change stock quantity
				PartDAO.StockAdjust(p.PartNo, p.PartType, UserHelper.DealerCode, warehouseId, null, tranDate, InventoryAction.SpecialExport, p.Amount, p.Quantity * -1, p.Remark, vch, null);
				p.State = SIE_ItemState.Exported;
			}
			else p.State = SIE_ItemState.NotFound;
		});
		PartDAO.PartDC.SubmitChanges();

		return vch;
	}

	/// <summary>
	/// build I/E voucher number
	/// </summary>
	/// <param name="pkey"></param>
	/// <param name="act"></param>
	/// <param name="dCode"></param>
	/// <param name="wId"></param>
	/// <param name="date"></param>
	/// <returns></returns>
	public static string GetVoucherNo(string pkey, string act, string dCode, long wId, DateTime date)
	{
		Warehouse w = WarehouseDAO.GetWarehouse(wId);
		string wh = (w == null) ? "" : w.Code;
		//return string.Format("{0}-{1}-{2}-", act, wh, date.ToString("yyMMdd"));
		return string.Format("{0}-{1}-{2}-{3}", act, wh, date.ToString("yyMMdd"), GetId(pkey, act, dCode, wId, date));
		//return string.Format("{0}", GetId(pkey, act, dCode, wId, date));
	}

	static Hashtable _SIkey = new Hashtable();
	public static string GetId(string pkey, string act, string dealerCode, long wid, DateTime date)
	{
		string key = string.Format("{0}.{1}.{2}.{3}", act, dealerCode, wid.ToString(), date.Date.ToString("yyyyMMdd"));
		string mykey = string.Format("{0}.{1}", key, pkey);
		int id = 0;
		if (!_SIkey.Contains(mykey))
		{
			if (!_SIkey.Contains(key))
			{
				var dc = DCFactory.GetDataContext<PartDataContext>();
				TransactionHistory th = dc.TransactionHistories.Where(t => t.DealerCode == dealerCode
												&& t.WarehouseId == wid
												&& t.TransactionCode == act
												&& t.TransactionDate == date.Date)
										  .OrderByDescending(t => t.InvoiceNumber)
										  .OrderByDescending(t => t.TransactionHistoryId)
										  .FirstOrDefault();
				id = ((th == null) || string.IsNullOrEmpty(th.InvoiceNumber)) ? 1 : int.Parse(th.InvoiceNumber.Substring(th.InvoiceNumber.Length - 5));
				_SIkey[key] = id;
			}
			else
			{
				id = (int)_SIkey[key];
				id++;
			}
			_SIkey[mykey] = id;
		}
		else
		{
			id = (int)_SIkey[mykey];
			_SIkey[mykey] = id;
		}

		return id.ToString().PadLeft(5, '0');
	}
}