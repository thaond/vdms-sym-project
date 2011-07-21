using System;
using System.Web.Security;
using System.Xml;
using VDMS.II.Common.Utils;

namespace VDMS.II.Common.Web
{
	public class VDMSMemberships
	{
		#region Statics

		public static string DefaultMemberShipProvider { get; set; }
		public static string MemberShipProviderNameFormat { get; set; }

		static VDMSMemberships()
		{
			DefaultMemberShipProvider = "AspNetOracleMembershipProvider";
			MemberShipProviderNameFormat = DefaultMemberShipProvider + ".{0}";
		}

		//public static void AddMemberShipProvider(string appName)
		//{
		//    ConfigFileManager webConfig = new ConfigFileManager(ConfigFileType.WebConfig);
		//    webConfig.AddNamespace("wc", "http://schemas.microsoft.com/.NetConfiguration/v2.0");
		//    webConfig.CurrentNodePath = "/wc:configuration/wc:system.web/wc:membership/wc:providers";
		//    webConfig.CurrentNSPrefix = "wc";

		//    string prvdName = string.Format(MemberShipProviderNameFormat, appName);
		//    XmlElement prvd = webConfig.GetElement("wc:add", "name", prvdName);
		//    if (prvd == null)
		//    {
		//        XmlElement defaultProvider = webConfig.GetElement("wc:add", "name", DefaultMemberShipProvider);
		//        prvd = (XmlElement)defaultProvider.Clone();
		//        prvd.SetAttribute("name", prvdName);
		//        prvd.SetAttribute("applicationName", appName);
		//        webConfig.AddElement(prvd);
		//        webConfig.SaveConfigDoc();
		//    }
		//}

		#endregion

		public MembershipProvider Provider { get; private set; }

		public MembershipUser CreateUser(string userName, string password)
		{
			return this.CreateUser(userName, password, string.Empty);
		}
		public MembershipUser CreateUser(string userName, string password, string email)
		{
			MembershipCreateStatus status;
			return this.CreateUser(userName, password, email, string.Empty, string.Empty, true, out status);
		}
		public MembershipUser CreateUser(string userName, string password, string email, string passQuest, string passAns, bool isApproved, out MembershipCreateStatus createStatus)
		{
			return this.CreateUser(userName, password, email, passQuest, passAns, isApproved, Guid.NewGuid(), out createStatus);
		}
		public MembershipUser CreateUser(string userName, string password, string email, string passQuest, string passAns, bool isApproved, object providerUserKey, out MembershipCreateStatus createStatus)
		{
			return Provider.CreateUser(userName, password, email, passQuest, passAns, isApproved, providerUserKey, out createStatus);
		}

		public bool DeleteUser(string userName)
		{
			return this.DeleteUser(userName, true);
		}
		public bool DeleteUser(string userName, bool deleteAllRelatedData)
		{
			return Provider.DeleteUser(userName, deleteAllRelatedData);
		}

		public MembershipUserCollection FindUsersByEmail(string emailToMatch)
		{
			int totalRecords;
			return this.FindUsersByEmail(emailToMatch, 0, int.MaxValue, out totalRecords);
		}
		public MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return this.Provider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
		}

		public MembershipUserCollection FindUsersByName(string usernameToMatch)
		{
			int totalRecords;
			return this.FindUsersByName(usernameToMatch, 0, int.MaxValue, out totalRecords);
		}
		public MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return this.Provider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
		}
		//
		// Summary:
		//     Generates a random password of the specified length.
		//
		// Parameters:
		//   length:
		//     The number of characters in the generated password. The length must be between
		//     1 and 128 characters.
		//
		//   numberOfNonAlphanumericCharacters:
		//     The minimum number of punctuation characters in the generated password.
		//
		// Returns:
		//     A random password of the specified length.
		//
		// Exceptions:
		//   System.ArgumentException:
		//     length is less than 1 or greater than 128 -or- numberOfNonAlphanumericCharacters
		//     is less than 0 or greater than length.
		public string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
		{
			return Membership.GeneratePassword(length, numberOfNonAlphanumericCharacters);
		}

		public MembershipUserCollection GetAllUsers()
		{
			int totalRecords;
			return this.GetAllUsers(0, int.MaxValue, out totalRecords);
		}
		public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			return this.Provider.GetAllUsers(pageIndex, pageSize, out totalRecords);
		}

		public int GetNumberOfUsersOnline()
		{
			return this.Provider.GetNumberOfUsersOnline();
		}

		//
		// Summary:
		//     Gets the information from the data source and updates the last-activity date/time
		//     stamp for the current logged-on membership user.
		//
		// Returns:
		//     A System.Web.Security.MembershipUser object representing the current logged-on
		//     user.
		public MembershipUser GetUser()
		{
			return Membership.GetUser();
		}
		//
		// Summary:
		//     Gets the information from the data source for the current logged-on membership
		//     user. Updates the last-activity date/time stamp for the current logged-on
		//     membership user, if specified.
		//
		// Parameters:
		//   userIsOnline:
		//     If true, updates the last-activity date/time stamp for the specified user.
		//
		// Returns:
		//     A System.Web.Security.MembershipUser object representing the current logged-on
		//     user.
		public MembershipUser GetUser(bool userIsOnline)
		{
			return Membership.GetUser(userIsOnline);
		}

		/// <summary>
		/// Gets the information from the data source for the membership user associated with the specified unique identifier.
		/// </summary>
		/// <param name="providerUserKey">The unique user identifier from the membership data source for the user.</param>
		/// <returns>A System.Web.Security.MembershipUser object representing the user associated with the specified unique identifier.</returns>
		public MembershipUser GetUser(object providerUserKey)
		{
			return this.GetUser(providerUserKey, false);
		}

		/// <summary>
		/// Gets the information from the data source for the specified membership user.
		/// </summary>
		/// <param name="username">The name of the user to retrieve.</param>
		/// <returns>A System.Web.Security.MembershipUser object representing the specified user.</returns>
		public MembershipUser GetUser(string username)
		{
			return this.GetUser(username, false);
		}

		/// <summary>
		/// Gets the information from the data source for the membership user associated with the specified unique identifier. Updates the last-activity date/time stamp for the user, if specified.
		/// </summary>
		/// <param name="providerUserKey">The unique user identifier from the membership data source for the user.</param>
		/// <param name="userIsOnline">If true, updates the last-activity date/time stamp for the specified user.</param>
		/// <returns>A System.Web.Security.MembershipUser object representing the user associated with the specified unique identifier.</returns>
		public MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			return this.Provider.GetUser(providerUserKey, userIsOnline);
		}

		/// <summary>
		/// Gets the information from the data source for the specified membership user. Updates the last-activity date/time stamp for the user, if specified.
		/// </summary>
		/// <param name="username">The name of the user to retrieve.</param>
		/// <param name="userIsOnline">If true, updates the last-activity date/time stamp for the specified user.</param>
		/// <returns>A System.Web.Security.MembershipUser object representing the specified user.</returns>
		public MembershipUser GetUser(string username, bool userIsOnline)
		{
			return this.Provider.GetUser(username, userIsOnline);
		}

		/// <summary>
		/// Gets a user name where the e-mail address for the user matches the specified e-mail address.
		/// </summary>
		/// <param name="emailToMatch">The e-mail address to search for.</param>
		/// <returns>The user name where the e-mail address for the user matches the specified e-mail address. If no match is found, null is returned.</returns>
		public string GetUserNameByEmail(string emailToMatch)
		{
			return this.Provider.GetUserNameByEmail(emailToMatch);
		}

		public void UpdateUser(MembershipUser user)
		{
			this.Provider.UpdateUser(user);
		}

		/// <summary>
		/// Verifies that the supplied user name and password are valid.
		/// </summary>
		/// <param name="username">The name of the user to be validated.</param>
		/// <param name="password">The password for the specified user.</param>
		/// <returns>true if the supplied user name and password are valid; otherwise, false.</returns>
		public bool ValidateUser(string username, string password)
		{
			return this.Provider.ValidateUser(username, password);
		}

		public static string GetProviderName(string appName)
		{
			return (string.IsNullOrEmpty(appName)) ? VDMSMemberships.DefaultMemberShipProvider : string.Format(VDMSMemberships.MemberShipProviderNameFormat, appName);
		}
		public VDMSMemberships(string appName)
		{
			//Membership.CreateUser(
			this.Provider = Membership.Providers[VDMSMemberships.GetProviderName(appName)];
		}

	}
}
