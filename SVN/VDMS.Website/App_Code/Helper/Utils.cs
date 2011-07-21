using System;
using System.Net.Mail;

namespace VDMS.Helper
{
	public class DateTimeHelper
	{
		public static DateTime FirstDayInMonth
		{
			get { return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); }
		}

		public static string To24h(DateTime d)
		{
			return d.ToShortDateString() + " " + d.ToString("HH:mm");
		}

		public static string To24h(DateTime? nullable)
		{
			if (nullable.HasValue && nullable.Value != DateTime.MinValue) return To24h(nullable.Value);
			return string.Empty;
		}
	}

	public class EmailHelper
	{
		public static void SendMail(string from, string to, string subject, string body)
		{
			try
			{
				var mm = new MailMessage(from, to);

				mm.Subject = subject;
				mm.Body = body;
				mm.IsBodyHtml = true;

				var smtp = new SmtpClient();
				smtp.Send(mm);
			}
			catch { }
		}
	}

	public class ErrorHelper
	{
		public static Exception Exception { get; set; }
	}
}