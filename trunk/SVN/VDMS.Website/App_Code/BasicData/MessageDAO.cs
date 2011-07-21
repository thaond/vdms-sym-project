using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using VDMS.Helper;
using VDMS.II.Common.Utils;
using VDMS.II.Entity;
using VDMS.II.Linq;
using VDMS.II.Security;
using System.Text.RegularExpressions;

namespace VDMS.II.BasicData
{
	public class MessageDAO
	{
		public class MessageDetail
		{
			public long MessageBoxId { get; set; }
			public long MessageId { get; set; }
			public string Body { get; set; }
			public DateTime CreatedDate { get; set; }
			public string FromUser { get; set; }
			public string ToUser { get; set; }
			public string Image { get; set; }
			public string Flag { get; set; }
		}
        public class MessageWithFile
        {
            public long MessageId { get; set; }
            public string Body { get; set; }
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public List<File> Files { get; set; }
            public string BodyNonHTML { get { return RemoveHTMLTag(Body);} }
            public char Type { get; set; }
            public string Flag { get; set; }
        }

		#region Private Message

		int pmCount = 0;
		public int GetPMCount(string Position)
		{
			return pmCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public List<MessageDetail> FindAllPM(string Position, int maximumRows, int startRowIndex)
		{
			var db = DCFactory.GetDataContext<BasicDataContext>();
			var query = from m in db.Messages
						join b in db.MessageBoxes on m.MessageId equals b.MessageId
						where b.Position == Position
						select new MessageDetail
						{
							MessageBoxId = b.MessageBoxId,
							MessageId = m.MessageId,
							Body = m.Body,
							CreatedDate = m.CreatedDate,
							FromUser = b.FromUser,
							ToUser = b.ToUser,
							Image = b.Flag == MessageFlag.AnswerFlag ? "check.png" : "spacer.gif",
							Flag = m.Flag
						};
			if (UserHelper.IsDealer)
			{
				if (Position == MessageFlag.InboxPosition) query = query.Where(p => p.ToUser == UserHelper.DealerCode);
				if (Position == MessageFlag.OutboxPosition) query = query.Where(p => p.FromUser == UserHelper.DealerCode);
			}
			else
			{
				var s = UserHelper.Username;
				if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["id"])) s = HttpContext.Current.Request.QueryString["id"].ToUpper();
                if (Position == MessageFlag.InboxPosition) query = query.Where(p => p.ToUser.ToUpper() == s);
                if (Position == MessageFlag.OutboxPosition) query = query.Where(p => p.FromUser.ToUpper() == s);
			}
			pmCount = query.Count();
			return query.OrderByDescending(p=>p.CreatedDate).Skip(startRowIndex).Take(maximumRows).ToList();
		}

		[DataObjectMethod(DataObjectMethodType.Delete)]
		public void DeletePM(long MessageBoxId)
		{
			var db = DCFactory.GetDataContext<BasicDataContext>();
			db.MessageBoxes.DeleteOnSubmit(db.MessageBoxes.SingleOrDefault(p => p.MessageBoxId == MessageBoxId));
			db.SubmitChanges();
		}

		public static void SendMessage(string from, string to, long MessageId)
		{
			var db = DCFactory.GetDataContext<BasicDataContext>();
			db.MessageBoxes.InsertAllOnSubmit(new MessageBox[]{
				new MessageBox
				{
					Flag = MessageFlag.UnanswerFlag,
					FromUser = from.ToUpper(),
					ToUser = to.ToUpper(),
					MessageId = MessageId,
					Position = MessageFlag.InboxPosition
				},
				new MessageBox
				{
					Flag = MessageFlag.UnanswerFlag,
					FromUser = from.ToUpper(),
					ToUser = to.ToUpper(),
					MessageId = MessageId,
					Position = MessageFlag.OutboxPosition
				}
			});
			db.SubmitChanges();
		}

		public static void SendMessage(string to, long MessageId)
		{
            var db = DCFactory.GetDataContext<BasicDataContext>();
			db.MessageBoxes.InsertAllOnSubmit(new MessageBox[]{
				new MessageBox
				{
					Flag = MessageFlag.UnanswerFlag,
					FromUser = "SYSTEM",
					ToUser = to.ToUpper(),
					MessageId = MessageId,
					Position = MessageFlag.InboxPosition
				}
			});
			db.SubmitChanges();
		}
		#endregion

		#region Common message


        public int CommonMessageCount = 0;
		public int GetCommonMessageCount(string filltercontent)
		{
            return CommonMessageCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public List<MessageWithFile> FindCommonMessage(int maximumRows, int startRowIndex,string filltercontent)
		{   
            var db = DCFactory.GetDataContext<BasicDataContext>();
		    var query = from mes in db.Messages
		               where mes.Flag == MessageFlag.CommonMassage || mes.Flag == MessageFlag.CommonMessageDNF || mes.Flag == MessageFlag.CommonMessageHTF
		               select new MessageWithFile
		                          {

		                              MessageId = mes.MessageId,
		                              Body = mes.Body,
		                              CreatedDate = mes.CreatedDate,
		                              CreatedBy = mes.CreatedBy,
                                      Flag = mes.Flag,
                                      //Type = (mes.Type == null) ? MessageType.NormalMessage: mes.Type.Value
		                              //Files = db.Files.Where(p => p.MessageId == mes.MessageId).ToList()
                                      Type= (mes.Type.HasValue) ? mes.Type.Value: MessageType.NormalMessage
		                          };
            if (!string.IsNullOrEmpty(filltercontent) && !( filltercontent == "Từ khóa tìm kiếm" || filltercontent == "Keyworld") )
            {
                query = query.Where( v => v.Body.Contains(filltercontent));
            }
            if (UserHelper.DealerCode != "/" && UserHelper.DatabaseCode == "DNF")
            {
                query = query.Where(v => v.Flag != MessageFlag.CommonMessageHTF);
            }
            if (UserHelper.DealerCode != "/" && UserHelper.DatabaseCode == "HTF" )
            {
                query = query.Where(v => v.Flag != MessageFlag.CommonMessageDNF);
            }
            CommonMessageCount = query.Count();
		    return query.OrderByDescending(pmCount => pmCount.CreatedDate).Skip(startRowIndex).Take(maximumRows).ToList();
		    
		}

		public int GetCommonMessageCount(string fromDate, string toDate)
		{
            var db = DCFactory.GetDataContext<BasicDataContext>();
			return db.Messages.Where(p => p.Flag == MessageFlag.CommonMassage && p.CreatedDate >= UserHelper.ParseDate(fromDate) && p.CreatedDate <= UserHelper.ParseDate(toDate).AddDays(1)).Count();
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindCommonMessage(string fromDate, string toDate, int maximumRows, int startRowIndex)
		{
            var db = DCFactory.GetDataContext<BasicDataContext>();
            var query = from p in db.Messages
                        where (p.Flag == MessageFlag.CommonMassage || p.Flag == MessageFlag.CommonMessageDNF || p.Flag == MessageFlag.CommonMessageHTF) && p.CreatedDate >= UserHelper.ParseDate(fromDate) && p.CreatedDate <= UserHelper.ParseDate(toDate).AddDays(1)
                        orderby p.CreatedDate descending
                        select new
                        {
                            p.CreatedDate,
                            p.CreatedBy,
                            p.Body,
                            BodyNonHTML = RemoveHTMLTag(p.Body),
                            p.MessageId,
                            p.Flag,
                            p.Files
                        };
			return query.Skip(startRowIndex).Take(maximumRows);
		}

		[DataObjectMethod(DataObjectMethodType.Delete)]
		public void Delete(long MessageId)
		{
			var db = DCFactory.GetDataContext<BasicDataContext>();
			db.Files.DeleteAllOnSubmit(db.Files.Where(p => p.MessageId == MessageId));
			db.Messages.DeleteOnSubmit(db.Messages.SingleOrDefault(p => p.MessageId == MessageId));
			db.SubmitChanges();
		}

		public static long SaveMessage(string body, string flag, long? ParentId, File[] files,bool ishotnews)
		{
            var db = DCFactory.GetDataContext<BasicDataContext>();
			var msg = new Message
			{
				Body = body,
				CreatedBy = flag == MessageFlag.SystemMessage ? "SYSTEM" : UserHelper.IsDealer ? UserHelper.DealerCode : UserHelper.Username,
				CreatedDate = DateTime.Now,
				Flag = flag,
				ParentId = ParentId,
                Type= MessageType.NormalMessage
			};
            if (ishotnews)
                msg.Type = MessageType.HotMesssage;
			if (files != null)
			{
				foreach (var item in files)
					if (item != null)
						item.Message = msg;
			}

			db.Messages.InsertOnSubmit(msg);
			db.SubmitChanges();

			return msg.MessageId;
		}

		public static Message GetById(long MessageId)
		{
			var db = DCFactory.GetDataContext<BasicDataContext>();
			return db.Messages.SingleOrDefault(p => p.MessageId == MessageId);
		}

		public static void UpdateBody(long MessageId, string Body,bool ishotnew, string display)
		{
			var db = DCFactory.GetDataContext<BasicDataContext>();
			var m = db.Messages.SingleOrDefault(p => p.MessageId == MessageId);
			if (m != null)
			{
				m.Body = Body;
                m.Type = (ishotnew) ? MessageType.HotMesssage : MessageType.NormalMessage;
                m.Flag = display;
				db.SubmitChanges();
			}
		}
		#endregion

		#region Helper

		public static File CreateFile(HttpPostedFile postedFile)
		{
			if (postedFile == null) return null;
			File obj = null;
			int length = postedFile.ContentLength;
			string Filename = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1);
			if (length != 0 && length / 1024 < 5000)
			{
				obj = new File() { Body = new byte[length], FileName = Filename };
				postedFile.InputStream.Read(obj.Body, 0, length);
				obj.Body = Compressor.Compress(obj.Body);
				obj.FileName = Filename;
			}
			return obj;
		}

        public static string RemoveHTMLTag(string html)
        {
            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }
		#endregion

		#region Message Text Data

		public static string OverQuotationMessage
		{
			get
			{
				return FileHelper.ReadFileTextContent("App_Data/Over-Quotation-Message.txt");
			}
		}

		public static string OverShippingMessage
		{
			get
			{
				return FileHelper.ReadFileTextContent("App_Data/Over-Shipping-Message.txt");
			}
		}

		public static string OverShippingMail
		{
			get
			{
				return FileHelper.ReadFileTextContent("App_Data/Over-Shipping-Mail.txt");
			}
		}
		#endregion

		#region Send Message

		public static void SendNGAlert(string NotGoodNumber, string DealerCode)
		{
			var MessageId = MessageDAO.SaveMessage(string.Format("New NGForm {0} from dealer {1} has been sent", NotGoodNumber, DealerCode), MessageFlag.SystemMessage, null, null,false);

			var list = ProfileDAO.GetUsernameByProfile("E", DeptType.SparePart, 1); // Employee of Spare Part
			foreach (var item1 in list)
			{
				MessageDAO.SendMessage(item1, MessageId); // send message as system
			}
		}

		public static void SendNGConfirm(int Level)
		{
			var MessageId = MessageDAO.SaveMessage("New NGForm from dealer has been confirmed", MessageFlag.SystemMessage, null, null,false);

			var list = ProfileDAO.GetUsernameByProfile(string.Empty, DeptType.SparePart, Level); // Employee of Spare Part
			foreach (var item1 in list)
			{
				MessageDAO.SendMessage(item1, MessageId); // send message as system
			}
		}

		public static void SendNGReject(int Level)
		{
			var MessageId = MessageDAO.SaveMessage("New NGForm from dealer has been rejected", MessageFlag.SystemMessage, null, null,false);

			var list = ProfileDAO.GetUsernameByProfile(string.Empty, DeptType.SparePart, Level); // Employee of Spare Part
			foreach (var item1 in list)
			{
				MessageDAO.SendMessage(item1, MessageId); // send message as system
			}
		}

		public static void SendNGReject(string DealerCode)
		{
			var MessageId = MessageDAO.SaveMessage("New NGForm from dealer has been rejected", MessageFlag.SystemMessage, null, null,false);

			MessageDAO.SendMessage(DealerCode, MessageId); // send message as system
		}
		#endregion
	}
}