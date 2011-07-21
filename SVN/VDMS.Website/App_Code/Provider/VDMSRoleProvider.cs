using System;
using System.Web.Security;

namespace VDMS.Provider
{
	public class VDMSRoleProvider : RoleProvider
	{
		private string _currentOrgCode; // tuong tung voi Provider hien thoi
		private RoleProvider _currentProvider;

		public RoleProvider Provider
		{
			get
			{
				if ((_currentOrgCode != VDMSProvider.OrgCode) || (_currentProvider == null))
				{
					_currentOrgCode = VDMSProvider.OrgCode;
					_currentProvider = GetProvider(_currentOrgCode);
					if (_currentProvider == null)
					{
						throw new Exception("Invalid Organization Code!");
					}
				}
				return _currentProvider;
			}
			//private set;
		}

		public static RoleProvider GetProvider(string app)
		{
			RoleProvider res = Roles.Providers[VDMSProvider.GetRoleProviderName(app.Trim())];
			return res;
		}

		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{
			base.Initialize(name, config);
			_currentOrgCode = VDMSProvider.OrgCode;
		}

		#region RoleProvider

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			Provider.AddUsersToRoles(usernames, roleNames);
		}

		public override string ApplicationName
		{
#warning check this
			get
			{
				throw new NotImplementedException();
				//return VDMSProvider.OrgCode;
			}
			set
			{
				throw new NotImplementedException();
				//VDMSProvider.OrgCode = value;
			}
		}

		public override void CreateRole(string roleName)
		{
			Provider.CreateRole(roleName);
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			return Provider.DeleteRole(roleName, throwOnPopulatedRole);
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			return Provider.FindUsersInRole(roleName, usernameToMatch);
		}

		public override string[] GetAllRoles()
		{
			return Provider.GetAllRoles();
		}

		public override string[] GetRolesForUser(string username)
		{
			return Provider.GetRolesForUser(username);
		}

		public override string[] GetUsersInRole(string roleName)
		{
			return Provider.GetUsersInRole(roleName);
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			return Provider.IsUserInRole(username, roleName);
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			Provider.RemoveUsersFromRoles(usernames, roleNames);
		}

		public override bool RoleExists(string roleName)
		{
			return Provider.RoleExists(roleName);
		}

		#endregion
	}
}