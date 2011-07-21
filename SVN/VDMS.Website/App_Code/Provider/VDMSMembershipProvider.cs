using System;
using System.Web.Security;

namespace VDMS.Provider
{
	public class VDMSMembershipProvider : MembershipProvider
	{
		private string _currentOrgCode; // tuong tung voi Provider hien thoi
		private MembershipProvider _currentProvider;

		public MembershipProvider Provider
		{
			get
			{
				if (!_currentOrgCode.Equals(VDMSProvider.OrgCode) || (this._currentProvider == null) || !(_currentProvider.ApplicationName.Equals(_currentOrgCode)))
				{
					_currentOrgCode = VDMSProvider.OrgCode;
					_currentProvider = Membership.Providers[VDMSProvider.GetMemberShipProviderName(_currentOrgCode)];
					if (_currentProvider == null)
					{
						throw new Exception("Invalid Organization Code!");
					}
				}
				return _currentProvider;
			}
		}

		public static MembershipProvider GetProvider(string app)
		{
			MembershipProvider res = Membership.Providers[VDMSProvider.GetMemberShipProviderName(app.Trim())];
			return res;
		}

		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{
			base.Initialize(name, config);
			_currentOrgCode = VDMSProvider.OrgCode;
		}

		#region MembershipProvider

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			return this.Provider.ChangePassword(username, oldPassword, newPassword);
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			return this.Provider.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
		}

		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			return this.Provider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			return this.Provider.DeleteUser(username, deleteAllRelatedData);

		}

		public override bool EnablePasswordReset
		{
			get { return this.Provider.EnablePasswordReset; }
		}

		public override bool EnablePasswordRetrieval
		{
			get { return this.Provider.EnablePasswordRetrieval; }
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return this.Provider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return this.Provider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
		}

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			return this.Provider.GetAllUsers(pageIndex, pageSize, out totalRecords);
		}

		public override int GetNumberOfUsersOnline()
		{
			return this.Provider.GetNumberOfUsersOnline();
		}

		public override string GetPassword(string username, string answer)
		{
			return this.Provider.GetPassword(username, answer);
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			return this.Provider.GetUser(username, userIsOnline);
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			return this.Provider.GetUser(providerUserKey, userIsOnline);
		}

		public override string GetUserNameByEmail(string email)
		{
			return this.Provider.GetUserNameByEmail(email);
		}

		public override int MaxInvalidPasswordAttempts
		{
			get { return this.Provider.MaxInvalidPasswordAttempts; }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { return this.Provider.MinRequiredNonAlphanumericCharacters; }
		}

		public override int MinRequiredPasswordLength
		{
			get { return this.Provider.MinRequiredPasswordLength; }
		}

		public override int PasswordAttemptWindow
		{
			get { return this.Provider.PasswordAttemptWindow; }
		}

		public override MembershipPasswordFormat PasswordFormat
		{
			get { return this.Provider.PasswordFormat; }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { return this.Provider.PasswordStrengthRegularExpression; }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { return this.Provider.RequiresQuestionAndAnswer; }
		}

		public override bool RequiresUniqueEmail
		{
			get { return this.Provider.RequiresUniqueEmail; }
		}

		public override string ResetPassword(string username, string answer)
		{
			return this.Provider.ResetPassword(username, answer);
		}

		public override bool UnlockUser(string userName)
		{
			return this.Provider.UnlockUser(userName);
		}

		public override void UpdateUser(MembershipUser user)
		{
			this.Provider.UpdateUser(user);
		}

		public override bool ValidateUser(string username, string password)
		{
			return this.Provider.ValidateUser(username, password);
		}

		public override string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		#endregion
	}
}