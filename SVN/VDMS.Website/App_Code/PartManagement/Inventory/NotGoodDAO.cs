using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.Security;

namespace VDMS.II.PartManagement
{
	[DataObject(true)]
	public class LackTrackingHeader
	{
		public long NGFormHeaderId { get; set; }
		public string NotGoodNumber { get; set; }
		public string DealerCode { get; set; }
		public string RewardNumber { get; set; }
		public string IssueNumber { get; set; }
		public string DealerName { get; set; }
		public DateTime CreatedDate { get; set; }
		public IEnumerable<LackTrackingDetail> Items { get; set; }
	}
	[DataObject(true)]
	public class LackTrackingDetail
	{
		public long NGFormDetailId { get; set; }
		public string PartCode { get; set; }
		public string PartName { get; set; }
		public int RequestQuantity { get; set; }
		public int ApprovedQuantity { get; set; }
		public string L3Comment { get; set; }
		public bool Passed { get; set; }
		public string TransactionComment { get; set; }
	}

	public class NotGoodDAO
	{
		#region Get List Not Good Form

		int count = 0;
		public int GetCountAll(string fromDate, string toDate, string partCode, string issueNumber, string status, string NGtype, string IsPassed)
		{
			return count;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindAll(string fromDate, string toDate, string partCode, string issueNumber, string status, string NGtype, string IsPassed, int maximumRows, int startRowIndex)
		{
			DateTime d1 = UserHelper.ParseDate(fromDate);
			DateTime d2 = UserHelper.ParseDate(toDate);
			var db = DCFactory.GetDataContext<PartDataContext>();
			var query = db.NGFormHeaders.Where(p => p.DealerCode == UserHelper.DealerCode);

			if (!string.IsNullOrEmpty(partCode)) query = query.Where(p => p.NGFormDetails.Where(d => d.PartCode.Contains(partCode)).Count() != 0);
			if (d1 != DateTime.MinValue) query = query.Where(p => p.CreatedDate >= d1);
			if (d2 != DateTime.MinValue) query = query.Where(p => p.CreatedDate <= d2);
			if (!string.IsNullOrEmpty(status)) query = query.Where(p => p.Status == status);
			if (!string.IsNullOrEmpty(NGtype)) query = query.Where(p => p.NGType == NGtype);
			if (!string.IsNullOrEmpty(issueNumber)) query = query.Where(p => p.NotGoodNumber.Contains(issueNumber));
			if (!string.IsNullOrEmpty(IsPassed))
			{
				var passed = bool.Parse(IsPassed);
				query = from h in query
						join d in db.NGFormDetails on h.NGFormHeaderId equals d.NGFormHeaderId
						where d.Passed == passed
						select h;
			}
			count = query.Count();
			return query.OrderByDescending(p => p.CreatedDate).Skip(startRowIndex).Take(maximumRows);
		}

		[DataObjectMethod(DataObjectMethodType.Delete)]
		public void Delete(long NGFormHeaderId)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			db.NGFormDetails.DeleteAllOnSubmit(db.NGFormDetails.Where(p => p.NGFormHeaderId == NGFormHeaderId));
			var ngh = db.NGFormHeaders.SingleOrDefault(p => p.NGFormHeaderId == NGFormHeaderId);
			db.ReceiveHeaders.Single(p => p.ReceiveHeaderId == ngh.ReceiveHeaderId).HasNGForm = false;
			db.NGFormHeaders.DeleteOnSubmit(ngh);
			db.SubmitChanges();
		}

		public static NGFormHeader GetHeaderByReceiveId(long ReceiveId)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return db.NGFormHeaders.SingleOrDefault(p => p.ReceiveHeaderId == ReceiveId);
		}
		#endregion

		#region Get List Issue

		int issueCount = 0;
		public int GetIssueCount(string fromImportDate, string toImportDate, string fromOrderNumber, string toOrderNumber, string fromIssueNumber, string toIssueNumber)
		{
			return issueCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindAllIssue(string fromImportDate, string toImportDate, string fromOrderNumber, string toOrderNumber, string fromIssueNumber, string toIssueNumber, int maximumRows, int startRowIndex)
		{
			DateTime d1 = UserHelper.ParseDate(fromImportDate);
			DateTime d2 = UserHelper.ParseDate(toImportDate);
			var db = DCFactory.GetDataContext<PartDataContext>();
			var query = from h in db.ReceiveHeaders
						where h.DealerCode == UserHelper.DealerCode && h.HasNGForm == false &&
						(
							from d in db.ReceiveDetails
							where d.BrokenQuantity != 0 || d.WrongQuantity != 0 || d.LackQuantity != 0
							select new { d.ReceiveHeaderId }
						).Contains(new { h.ReceiveHeaderId })
						select new
						{
							h.ReceiveHeaderId,
							h.IssueNumber,
							Items = from rd in db.ReceiveDetails
									join p in db.Parts on rd.PartCode equals p.PartCode
									where rd.ReceiveHeaderId == h.ReceiveHeaderId && p.DatabaseCode == UserHelper.DatabaseCode
										&& (rd.BrokenQuantity != 0 || rd.WrongQuantity != 0 || rd.LackQuantity != 0)
									select new
									{
										rd.PartCode,
										rd.BrokenQuantity,
										rd.WrongQuantity,
										rd.LackQuantity,
										p.EnglishName,
										p.VietnamName,
										rd.DealerComment
									}
						};
			//if (!string.IsNullOrEmpty(partCode)) query = query.Where(p => p.Warehouse.WarehouseCode == warehouseCode);
			//if (!string.IsNullOrEmpty(orderNumber)) query = query.Where(p => p.TipTopNumber == orderNumber);
			//if (!string.IsNullOrEmpty(status)) query = query.Where(p => p.Status == status);
			//if (!string.IsNullOrEmpty(orderType)) query = query.Where(p => p.OrderType == orderType);
			issueCount = query.Count();
			return query.Skip(startRowIndex).Take(maximumRows);
		}
		#endregion

		#region Create Not Good Form

		//static NGFormDetail CreateNGItem(string PartCode, int Quantity, string Status, string Comment, NGFormHeader h)
		//{
		//    return new NGFormDetail
		//    {
		//        DealerComment = Comment,
		//        NGFormHeader = h,
		//        PartCode = PartCode,
		//        PartStatus = Status,
		//        RequestQuantity = Quantity
		//    };
		//}

		//public static void CreateNGFrom(long ReceiveHeaderId)
		//{
		//    var db = DCFactory.GetDataContext<PartDataContext>();

		//    var rh = db.ReceiveHeaders.Single(p => p.ReceiveHeaderId == ReceiveHeaderId);
		//    var h = new NGFormHeader
		//    {
		//        CreatedDate = DateTime.Now,
		//        Status = NGStatus.Open,
		//        DealerCode = UserHelper.DealerCode,
		//        ReceiveHeaderId = ReceiveHeaderId,
		//        ApproveLevel = 0,
		//        NGType = NGType.Normal,
		//        NotGoodNumber = rh.IssueNumber + "-NG-" + ReceiveHeaderId
		//    };
		//    var list = new List<NGFormDetail>();
		//    foreach (var item in db.ReceiveDetails.Where(p => p.ReceiveHeaderId == ReceiveHeaderId))
		//    {
		//        if (item.BrokenQuantity != 0)
		//            list.Add(CreateNGItem(item.PartCode, item.BrokenQuantity, "B", item.DealerComment, h));
		//        if (item.WrongQuantity != 0)
		//            list.Add(CreateNGItem(item.PartCode, item.WrongQuantity, "W", item.DealerComment, h));
		//        if (item.LackQuantity != 0)
		//            list.Add(CreateNGItem(item.PartCode, item.LackQuantity, "L", item.DealerComment, h));
		//    }

		//    db.NGFormHeaders.InsertOnSubmit(h);

		//    rh.HasNGForm = true;
		//    db.SubmitChanges();
		//}

		public static void SendForm(long OrderId)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			var ng = db.NGFormHeaders.Single(p => p.NGFormHeaderId == OrderId);
			ng.Status = OrderStatus.OrderSent;
			if (ng.NGType == NGType.Special)
			{
				var count = db.NGFormHeaders.Count(q => q.NotGoodNumber.Contains("NG-M-" + DateTime.Now.ToString("yyyyMM")));
				ng.NotGoodNumber = "NG-M-" + DateTime.Now.ToString("yyyyMM") + (count + 1).ToString();
			}
			db.SubmitChanges();

			// send message
			MessageDAO.SendNGAlert(ng.NotGoodNumber, ng.DealerCode);
		}
		#endregion

		#region Not Good Detail

		int ngDetailCount = 0;
		public int GetDetailCount(long NGFormHeaderId)
		{
			return ngDetailCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindAllDetail(long NGFormHeaderId, int maximumRows, int startRowIndex)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			var query = from h in db.NGFormDetails
						join p in db.Parts on h.PartCode equals p.PartCode
						where h.NGFormHeaderId == NGFormHeaderId && p.DatabaseCode == UserHelper.DatabaseCode
						select new
						{
							h.NGFormDetailId,
							h.PartCode,
							p.EnglishName,
							p.VietnamName,
							h.RequestQuantity,
							h.PartStatus,
							h.BrokenCode,
							h.ApprovedQuantity,
							h.ProblemAgainQuantity,
							h.Passed,
							h.TransactionComment
						};
			ngDetailCount = query.Count();
			return query.Skip(startRowIndex).Take(maximumRows);
		}

		public static List<NGFormDetail> FindAllDetail(long NGFormHeaderId)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return db.NGFormDetails.Where(p => p.NGFormHeaderId == NGFormHeaderId).ToList();
		}

		#endregion

		#region Get List Sent Form

		int sentCount = 0;
		public int GetCountDetails(string fromDate, string toDate, string issueNumber, string dealer, string status, string _type, string level)
		{
			return sentCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindAllDetails(string fromDate, string toDate, string issueNumber, string dealer, string status, string _type, string level, int maximumRows, int startRowIndex)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			var query = from h in db.NGFormHeaders
						join dl in db.Dealers on h.DealerCode equals dl.DealerCode
						where dl.DatabaseCode == UserHelper.DatabaseCode
						select new
						{
							h.NGFormHeaderId,
							h.CreatedDate,
							h.DealerCode,
							h.ApproveLevel,
							h.Status,
							h.NotGoodNumber,
							h.NGType,
							dl.AreaCode,
							Items = from d in db.NGFormDetails
									join p in db.Parts on d.PartCode equals p.PartCode
									where d.NGFormHeaderId == h.NGFormHeaderId && p.DatabaseCode == UserHelper.DatabaseCode
									select new
									{
										d.NGFormDetailId,
										d.PartCode,
										p.EnglishName,
										p.VietnamName,
										d.RequestQuantity,
										ApprovedQuantity = h.ApproveLevel > 0 ? d.ApprovedQuantity : d.RequestQuantity,
										d.DealerComment,
										d.L1Comment,
										d.L2Comment,
										d.L3Comment,
										d.PartStatus,
										d.BrokenCode
									}
						};
			DateTime fd = UserHelper.ParseDate(fromDate);
			DateTime td = UserHelper.ParseDate(toDate);
			if (fd != DateTime.MinValue) query = query.Where(p => p.CreatedDate >= fd);
			if (td != DateTime.MinValue) query = query.Where(p => p.CreatedDate <= td);
			if (!string.IsNullOrEmpty(issueNumber)) query = query.Where(p => p.NotGoodNumber.Contains(issueNumber));
			if (!string.IsNullOrEmpty(dealer)) query = query.Where(p => p.DealerCode == dealer);
			if (!string.IsNullOrEmpty(status)) query = query.Where(p => p.Status == status);
			if (!string.IsNullOrEmpty(_type)) query = query.Where(p => p.NGType == _type);
			if (int.Parse(level) != 0) query = query.Where(p => (p.ApproveLevel == int.Parse(level) - 1 && (p.Status == NGStatus.Sent || p.Status == NGStatus.Confirmed))
				|| (p.ApproveLevel == int.Parse(level) && p.Status == NGStatus.Reject)
				);
			if (UserHelper.Profile.NGLevel == 1) query = query.Where(p => p.AreaCode == UserHelper.Profile.AreaCode);
			sentCount = query.Count();
			return query.OrderBy(p => p.NGFormHeaderId).Skip(startRowIndex).Take(maximumRows);
		}
		#endregion

		#region Lack tracking
		int lackTrackingCount = 0;
		public int GetLackTrakingCount(string fromDate, string toDate, string issueNumber, string rewardNumber, string dealer, string status)
		{
			return lackTrackingCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		// sua thang` nay nho' sua luon GetLackingData
		public IQueryable FindLackTracking(string fromDate, string toDate, string issueNumber, string rewardNumber, string dealer, string status, int maximumRows, int startRowIndex)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			var subQuery = from t0 in db.NGFormDetails
						   where t0.PartStatus == PartNGType.Lack
						   select t0.NGFormHeaderId;
			if (!string.IsNullOrEmpty(status))
			{
				var passed = bool.Parse(status);
				subQuery = from t0 in db.NGFormDetails
						   where t0.PartStatus == PartNGType.Lack && t0.Passed == passed
						   select t0.NGFormHeaderId;
			}
			if (subQuery.Count() == 0) return null;
			var query = from h in db.NGFormHeaders
						join dl in db.Dealers on h.DealerCode equals dl.DealerCode
						where dl.DatabaseCode == UserHelper.DatabaseCode &&
							subQuery.Contains(h.NGFormHeaderId) &&
							h.ApproveLevel == 3 &&
							h.NGType == NGType.Normal
						select new
						{
							h.NGFormHeaderId,
							h.NotGoodNumber,
							h.DealerCode,
							h.RewardNumber,
							dl.DealerName,
							h.CreatedDate,
							Items = from d in db.NGFormDetails
									join p in db.Parts on d.PartCode equals p.PartCode
									where d.NGFormHeaderId == h.NGFormHeaderId && p.DatabaseCode == UserHelper.DatabaseCode &&
										d.PartStatus == PartNGType.Lack
									select new
									{
										d.NGFormDetailId,
										d.PartCode,
										PartName = UserHelper.IsVietnamLanguage ? p.VietnamName : p.EnglishName,
										d.RequestQuantity,
										d.ApprovedQuantity,
										d.L3Comment,
										d.Passed,
										d.TransactionComment
									}
						};
			DateTime fd = UserHelper.ParseDate(fromDate);
			DateTime td = UserHelper.ParseDate(toDate);
			if (fd != DateTime.MinValue) query = query.Where(p => p.CreatedDate >= fd);
			if (td != DateTime.MinValue) query = query.Where(p => p.CreatedDate <= td);
			if (!string.IsNullOrEmpty(dealer)) query = query.Where(p => p.DealerCode == dealer);
			if (!string.IsNullOrEmpty(issueNumber)) query = query.Where(p => p.NotGoodNumber.Contains(issueNumber));
			if (!string.IsNullOrEmpty(rewardNumber)) query = query.Where(p => p.RewardNumber.Contains(rewardNumber));

			lackTrackingCount = query.Count();
			return query.OrderBy(p => p.NGFormHeaderId).Skip(startRowIndex).Take(maximumRows);
		}

		public static LackTrackingHeader GetLackingData(long NGheaderID, string status)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			var subQuery = from t0 in db.NGFormDetails
						   where t0.PartStatus == PartNGType.Lack
						   select t0.NGFormHeaderId;
			if (!string.IsNullOrEmpty(status))
			{
				var passed = bool.Parse(status);
				subQuery = from t0 in db.NGFormDetails
						   where t0.PartStatus == PartNGType.Lack && t0.Passed == passed
						   select t0.NGFormHeaderId;
			}
			if (subQuery.Count() == 0) return null;
			var query = from h in db.NGFormHeaders
						join dl in db.Dealers on h.DealerCode equals dl.DealerCode
						where dl.DatabaseCode == UserHelper.DatabaseCode &&
							subQuery.Contains(h.NGFormHeaderId) &&
							h.ApproveLevel == 3 &&
							h.NGType == NGType.Normal &&
							h.NGFormHeaderId == NGheaderID
						select new LackTrackingHeader
						{
							NGFormHeaderId = h.NGFormHeaderId,
							NotGoodNumber = h.NotGoodNumber,
							DealerCode = h.DealerCode,
							RewardNumber = h.RewardNumber,
							DealerName = dl.DealerName,
							IssueNumber = h.ReceiveHeader.IssueNumber,
							CreatedDate = h.CreatedDate,
							Items = from d in db.NGFormDetails
									join p in db.Parts on d.PartCode equals p.PartCode
									where d.NGFormHeaderId == h.NGFormHeaderId && p.DatabaseCode == UserHelper.DatabaseCode &&
										d.PartStatus == PartNGType.Lack
									select new LackTrackingDetail
									{
										NGFormDetailId = d.NGFormDetailId,
										PartCode = d.PartCode,
										PartName = UserHelper.IsVietnamLanguage ? p.VietnamName : p.EnglishName,
										RequestQuantity = d.RequestQuantity,
										ApprovedQuantity = d.ApprovedQuantity,
										L3Comment = d.L3Comment,
										Passed = d.Passed,
										TransactionComment = d.TransactionComment
									}
						};
			return query.FirstOrDefault();
		}

		#endregion

		#region Approve

		public static void ApproveDetail(long NGFormDetailId, int ApprovedQuantity, string FeedBack)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			var d = db.NGFormDetails.Single(p => p.NGFormDetailId == NGFormDetailId);
			d.ApprovedQuantity = ApprovedQuantity;
			switch (UserHelper.Profile.NGLevel.Value)
			{
				case 1:
					d.L1Comment = FeedBack;
					break;
				case 2:
					d.L2Comment = FeedBack;
					break;
				case 3:
					d.L3Comment = FeedBack;
					break;
				default:
					break;
			}
			db.SubmitChanges();
		}

		/// <summary>
		/// Approve the NG form
		/// </summary>
		/// <param name="NGFormHeaderId"></param>
		/// <param name="total"></param>
		public static void ApproveHeader(long NGFormHeaderId, int total)
		{
			if (!UserHelper.Profile.NGLevel.HasValue) return;
			var db = DCFactory.GetDataContext<PartDataContext>();
			var h = db.NGFormHeaders.Single(p => p.NGFormHeaderId == NGFormHeaderId);
			h.ApproveLevel = (byte)UserHelper.Profile.NGLevel.Value;
			if (h.ApproveLevel == 3)
			{
				h.Status = NGStatus.Confirmed;
				h.ApproveDate = DateTime.Now;

				// in case vmep do not approve any lack, then close order
				if (total == 0)
				{
					var oh = db.OrderHeaders.Single(p => p.OrderHeaderId == db.ReceiveHeaders.Single(q => q.ReceiveHeaderId == h.ReceiveHeaderId).OrderHeaderId);
					oh.Status = OrderStatus.OrderClosedAbnormal;
					db.SubmitChanges();
				}

				// send message
				var MessageId = MessageDAO.SaveMessage(string.Format("A NGForm {0} has been approved", h.NotGoodNumber), MessageFlag.SystemMessage, null, null,false);
				MessageDAO.SendMessage(h.DealerCode, MessageId); // send message as system
			}
			else MessageDAO.SendNGConfirm(h.ApproveLevel + 1);
			db.SubmitChanges();
		}

		public static void RejectHeader(long NGFormHeaderId)
		{
			if (!UserHelper.Profile.NGLevel.HasValue) return;
			var db = DCFactory.GetDataContext<PartDataContext>();
			var h = db.NGFormHeaders.Single(p => p.NGFormHeaderId == NGFormHeaderId);
			h.Status = NGStatus.Reject;
			h.ApproveLevel = UserHelper.Profile.NGLevel.Value - 1;
			if (h.ApproveLevel > 0) MessageDAO.SendNGReject(h.ApproveLevel);
			else MessageDAO.SendNGReject(h.DealerCode);
			db.SubmitChanges();
		}
		#endregion

		#region Helper

		public static NGFormHeader GetNGHeader(long id)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return db.NGFormHeaders.SingleOrDefault(h => h.NGFormHeaderId == id);
		}

		public static long GetIdByNGNumber(string NGNumber)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			var obj = db.NGFormHeaders.SingleOrDefault(p => p.NotGoodNumber == NGNumber);
			if (obj == null) return 0;
			return obj.NGFormHeaderId;
		}
		#endregion
	}
}