using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using VDMS.Common.Utils;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.PartManagement
{
	[DataObject(true)]
	public class PartTransfer
	{
		public Guid PartKey { get; set; }
		public string PartNo { get; set; }
		public string PartName { get; set; }
		public string PartType { get; set; }
		public string Remark { get; set; }
		public string State { get; set; }
		public int CurrentQuantity { get; set; }
		public int TransferQuantity { get; set; }
		public int ConfirmQuantity { get; set; }
		public int UnitPrice { get; set; }
		public int SubAmount { get; set; }
		public bool ManualSelected { get; set; }
		public long PartInfoId { get; set; }

		public PartTransfer()
		{
			this.ManualSelected = false;
			this.CurrentQuantity = -1;
			this.State = ST_ItemState.New;
		}

		public PartTransfer(TransferDetail td)
			: this()
		{
			var dc = DCFactory.GetDataContext<PartDataContext>();
			this.TransferQuantity = td.Quantity;
			this.PartNo = td.PartCode;
			try
			{
				this.PartName = td.PartInfo.PartType == VDMS.II.Entity.PartType.Part ?
					dc.Parts.SingleOrDefault(p => p.PartCode == td.PartCode && p.DatabaseCode == td.TransferHeader.Dealer.DatabaseCode).VietnamName :
					td.PartInfo.Accessory.VietnamName;
			}
			catch { }
		}
	}
	public class PartTransferDAO
	{
		public PartTransferDAO() { }

		#region Transfer action

		private static List<PartTransfer> STParts(string key)
		{
			List<PartTransfer> parts = HttpContext.Current.Session[key] as List<PartTransfer>;
			if (parts == null)
			{
				parts = new List<PartTransfer>();
				HttpContext.Current.Session[key] = parts;
			}
			return parts;
		}

		public int CountParts(string key)
		{
			return STParts(key).Count;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IEnumerable<PartTransfer> FindAll(string key, int maximumRows, int startRowIndex)
		{
			var res = STParts(key).Skip(startRowIndex).Take(maximumRows);
			return res;
		}

		public static void UpdatePart(long warehouseOd, string key, object partKey, string partNo, string partType, int? tQty, int? cQty, string remark)
		{
			PartTransfer part = STParts(key).SingleOrDefault(p => p.PartKey == (Guid)partKey);
			if (part != null)
			{
				part.State = ST_ItemState.New;
				if (part.ManualSelected && (part.CurrentQuantity < 0))
				{
					PartSafety ps = PartInfoDAO.GetPartSafety(part.PartNo, warehouseOd);
					if (ps != null)
					{
						part.CurrentQuantity = ps.CurrentStock;
						if (part.CurrentQuantity <= 0) part.State = ST_ItemState.Wrong;
					}
					else part.State = ST_ItemState.Wrong;
				}
				if (partNo != null) part.PartNo = partNo;
				if (tQty != null) part.TransferQuantity = (int)tQty;
				if (cQty != null) part.ConfirmQuantity = (int)cQty;
				if (remark != null) part.Remark = remark;
				if (partType != null) part.PartType = partType;
			}
		}

		public static void Append(int RowCount, string key)
		{
			List<PartTransfer> parts = STParts(key);
			for (int i = 0; i < RowCount; i++)
			{
				parts.Add(new PartTransfer() { PartKey = Guid.NewGuid(), ManualSelected = true, });
			}
		}

		public static void Append(string partCode, string pType, string key, int currentStock, long partInfoId)
		{
			Append(partCode, pType, key, currentStock, partInfoId, 0);
		}

		public static void Append(string partCode, string pType, string key, int currentStock, long partInfoId, int transferQty)
		{
			List<PartTransfer> parts = STParts(key);
			PartTransfer part = parts.SingleOrDefault(p => p.PartNo == partCode && p.PartType == pType);
			if (part == null)
			{
				//int stock; if (!int.TryParse(currentStock, out stock)) stock = -1;
				//long pInfoId; if (!long.TryParse(partInfoId, out pInfoId)) pInfoId = -1;

				parts.Add(new PartTransfer() { PartKey = Guid.NewGuid(), PartNo = partCode, PartType = pType, TransferQuantity = transferQty, CurrentQuantity = currentStock });
			}
		}

		public static void Clear(string key)
		{
			HttpContext.Current.Session.Remove(key);
		}
		public static void Clear(string key, string state)
		{
			STParts(key).RemoveAll(p => p.State == state);
		}

		public static TransferHeader GetTransferHeader(long id)
		{
			var dc = DCFactory.GetDataContext<PartDataContext>();
			TransferHeader th = dc.TransferHeaders.SingleOrDefault(h => h.TransferHeaderId == id);
			return th;
		}

		public static long TransferParts(string key, long shId, long fromWhId, long toWhId, DateTime tDate, string status, string comment)
		{
			long res = 0;

			Warehouse wh = WarehouseDAO.GetWarehouse(fromWhId);
			Warehouse wh2 = WarehouseDAO.GetWarehouse(toWhId);
			if ((wh == null) || (wh2 == null)) return res;

			List<PartTransfer> parts = STParts(key);
			parts.RemoveAll(p => p.CurrentQuantity <= 0);

			if (parts.Count > 0)
			{
				var dc = DCFactory.GetDataContext<PartDataContext>();
				TransferHeader th = GetTransferHeader(shId);
				if (th == null)
				{
					th = new TransferHeader()
					{
						CreatedBy = UserHelper.Username,
						CreatedDate = DateTime.Now,
						VoucherNumber = PartTransferDAO.GenVoucherNumber(wh.Code, wh2.Code, DateTime.Now),
					};
				}
				else
				{
					dc.TransferDetails.DeleteAllOnSubmit(th.TransferDetails);
				}

				th.FromWarehouseId = fromWhId;
				th.ToWarehouseId = toWhId;
				th.DealerCode = wh.DealerCode;
				th.Status = status;
				th.TransferDate = tDate.Date;
				th.TransferComment = comment;

				bool ok = true;
				foreach (PartTransfer p in parts.Where(p => p.State == ST_ItemState.New))
				{
					if (string.IsNullOrEmpty(p.PartType))
					{
						p.PartType = AccessoryDAO.IsAccessoryExist(p.PartNo, UserHelper.DealerCode) ? PartType.Accessory : PartType.Part;
					}

					PartInfo pi = PartInfoDAO.GetPartInfo(p.PartNo, wh.DealerCode, p.PartType);
					if ((pi != null) && (p.CurrentQuantity >= p.TransferQuantity))
					{
						TransferDetail td = new TransferDetail() { Quantity = p.TransferQuantity, PartCode = p.PartNo, PartInfoId = pi.PartInfoId, PartComment = p.Remark };
						if (th.TransferHeaderId > 0)
						{
							td.TransferHeaderId = th.TransferHeaderId;
							dc.TransferDetails.InsertOnSubmit(td);
						}
						else td.TransferHeader = th;

						if (status == ST_Status.Transfered)
						{
							ok = ok && PartDAO.StockAdjust(p.PartNo, p.PartType, wh.DealerCode, fromWhId, toWhId, tDate, InventoryAction.StockTransfer, 0, p.TransferQuantity * -1, p.Remark, null, null);
							p.State = ST_ItemState.Moved;
						}
					}
					else
					{
						p.State = ST_ItemState.Wrong;
						ok = false;
					}
				}

				if (th.TransferHeaderId == 0) dc.TransferHeaders.InsertOnSubmit(th);
				if (ok && th.TransferDetails.Count > 0)
				{
					dc.SubmitChanges();
					res = th.TransferHeaderId;
				}
			}

			return res;
		}
		public static string GenVoucherNumber(string fromW, string toW, DateTime crtDate)
		{
			return string.Format("T{0}-{1}-{2}-", crtDate.ToString("yy"), fromW, toW);
		}

		#endregion

		#region Query data

		int _TransferHeaderCount;
		public int CountTransferHeaders(string fromD, string toD, long fromWH, long toWH, string fromDate, string toDate, string status)
		{
			return _TransferHeaderCount;
		}
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable<TransferHeader> SearchTransferHeaders(string fromD, string toD, long fromWH, long toWH, string fromDate, string toDate, string status, int maximumRows, int startRowIndex)
		{
			var dc = DCFactory.GetDataContext<PartDataContext>();
			DateTime dtFrom = DataFormat.DateFromString(fromDate);
			DateTime dtTo = DataFormat.DateFromString(toDate);
			if (dtTo == DateTime.MinValue) dtTo = DateTime.Now;

			// date range
			var query = dc.TransferHeaders.Where(t => t.TransferDate >= dtFrom && t.TransferDate <= dtTo);
			// from place
			if (fromWH > 0)
				query = query.Where(t => t.FromWarehouseId == fromWH);
			else
				if (!string.IsNullOrEmpty(fromD)) query = query.Where(t => t.DealerCode == fromD);
			// to place
			if (toWH > 0)
				query = query.Where(t => t.ToWarehouseId == toWH);
			else
				if (!string.IsNullOrEmpty(toD)) query = query.Where(t => t.Warehouse1.DealerCode == toD);
			// status
			if (!string.IsNullOrEmpty(status)) query = query.Where(t => t.Status == status);

			_TransferHeaderCount = query.Count();

			return ((startRowIndex >= 0) && (maximumRows > 0)) ? query.Skip(startRowIndex).Take(maximumRows) : query;
		}


		int _TransferDetailCount;
		public int CountTransferDetails(long thId)
		{
			return _TransferDetailCount;
		}
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable<TransferDetail> ViewTransferDetail(long thId, int maximumRows, int startRowIndex)
		{
			var dc = DCFactory.GetDataContext<PartDataContext>();
			var res = dc.TransferDetails.Where(t => t.TransferHeaderId == thId);
			_TransferDetailCount = res.Count();

			return ((maximumRows > 0) && (startRowIndex >= 0)) ? res.Skip(startRowIndex).Take(maximumRows) : res;
		}

		#endregion
	}
}