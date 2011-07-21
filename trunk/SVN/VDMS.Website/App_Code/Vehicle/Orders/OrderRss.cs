using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using NHibernate.Expression;
using Resources;
using VDMS.Core.Data;
using VDMS.Core.Domain;
using VDMS.Data.IDAL.Interface;

namespace VDMS.I.Vehicle
{
	public class OrderRss : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			XDocument document = new XDocument(
				//new XDeclaration("1.0", "utf-8", "no"),
								 new XElement("rss",
									 new XAttribute("version", "2.0"),
									 new XElement("channel", this.CreateElements())
									));

			context.Response.ContentType = "text/xml";
			document.Save(context.Response.Output);
			context.Response.End();
		}

		private IEnumerable<XElement> CreateElements()
		{
			List<XElement> list = new List<XElement>();
			var link = HttpContext.Current.Request.Url.AbsoluteUri;
			link = link.Substring(0, link.LastIndexOf('/'));

			IDao<Orderheader, long> dao = DaoFactory.GetDao<Orderheader, long>();
			dao.SetCriteria(new ICriterion[] { Expression.Eq("Status", 1), Expression.Between("Createddate", DateTime.Now.AddDays(-30), DateTime.Now.AddDays(1)) });
			dao.SetOrder(new NHibernate.Expression.Order[] { NHibernate.Expression.Order.Desc("Createddate") });
			List<Orderheader> orders = dao.GetPaged(0, 30);

			foreach (Orderheader oh in orders)
			{
				IDao<Orderdetail, long> odDao = DaoFactory.GetDao<Orderdetail, long>();
				odDao.SetCriteria(new ICriterion[] { Expression.Eq("Orderheader", oh) });
				int Count = odDao.GetCount();

				//OrderItem item = new OrderItem();
				string Title = string.Format(Message.Order_NewOrderMsg, oh.Dealercode, oh.Createddate);
				string Description = string.Format(Message.Order_NewOrderDetailCountMsg, Count);
				string Link = link + "/Vehicle/Inventory/ProcessOrder.aspx?OrderId=" + oh.Id.ToString();
				string PubDate = oh.Createddate.ToString();

				XElement itemElement = new XElement("item",
											new XElement("title", Title),
											new XElement("description", Description),
											new XElement("link", Link),
											new XElement("pubDate", PubDate)
									  );
				list.Add(itemElement);
			}
			return list;
		}

		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}