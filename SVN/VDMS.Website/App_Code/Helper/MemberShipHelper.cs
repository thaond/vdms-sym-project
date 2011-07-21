using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace VDMS.Helper
{
	public class MembershipHelper
	{
		static object _lock = new object();

		public static void AddMembershipProvider(string appName)
		{
			lock (_lock)
			{
				string path = HttpContext.Current.Server.MapPath("~/membership.config");
				var membership = XDocument.Load(path);
				var newItem = new XElement("add",
					new XAttribute("name", string.Concat("AspNetOracleMembershipProvider.", appName)),
					new XAttribute("applicationName", appName),
					new XAttribute("description", "dotConnect for Oracle membership provider"),
					new XAttribute("connectionStringName", "ConnectionString"),
					new XAttribute("passwordFormat", "Hashed"),
					new XAttribute("enablePasswordRetrieval", false),
					new XAttribute("minRequiredPasswordLength", 1),
					new XAttribute("requiresQuestionAndAnswer", false),
					new XAttribute("minRequiredNonalphanumericCharacters", 0),
					new XAttribute("type", "Devart.Data.Oracle.Web.Providers.OracleMembershipProvider")
					);
				membership.Element("membership").Element("providers").Add(newItem);
				membership.Save(path);
			}
		}

		public static void RemoveMembershipProvider(string appName)
		{
			lock (_lock)
			{
				string path = HttpContext.Current.Server.MapPath("~/membership.config");
				var membership = XDocument.Load(path);
				var item = membership.Element("membership").Element("providers").Elements("add").Where(p => p.Element("appName").Value == appName);
				item.Remove();
				membership.Save(path);
			}
		}

		public static void AddRoleProvider(string appName)
		{
			lock (_lock)
			{
				string path = HttpContext.Current.Server.MapPath("~/roleManager.config");
				var membership = XDocument.Load(path);
				var newItem = new XElement("add",
					new XAttribute("name", string.Concat("AspNetOracleRoleProvider.", appName)),
					new XAttribute("applicationName", appName),
					new XAttribute("description", "dotConnect for Oracle membership provider"),
					new XAttribute("connectionStringName", "ConnectionString"),
					new XAttribute("type", "Devart.Data.Oracle.Web.Providers.OracleRoleProvider")
					);
				membership.Element("roleManager").Element("providers").Add(newItem);
				membership.Save(path);
			}
		}

		public static void RemoveRoleProvider(string appName)
		{
			lock (_lock)
			{
				string path = HttpContext.Current.Server.MapPath("~/roleManager.config");
				var membership = XDocument.Load(path);
				var item = membership.Element("roleManager").Element("providers").Elements("add").Where(p => p.Element("appName").Value == appName);
				item.Remove();
				membership.Save(path);
			}
		}
	}
}