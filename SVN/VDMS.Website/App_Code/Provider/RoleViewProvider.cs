using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using VDMS.II.Entity;
using VDMS.II.Security;

namespace VDMS.Provider
{
	public class RoleViewProvider : StaticSiteMapProvider
	{
		static readonly string _errmsg2 = "Duplicate node ID";
		public SiteMapNode _root = null;

		public void ClearMap()
		{
			if (_root != null) ClearMap(_root);
			_root = null;
		}

		public void ClearMap(SiteMapNode node)
		{
			//if (node.HasChildNodes)
			{
				while (node.ChildNodes.Count > 0)
				{
					ClearMap(node.ChildNodes[0]);
				}
			}
			RemoveNode(node);
		}

		public override void Initialize(string name, NameValueCollection attributes)
		{
			base.Initialize(name, attributes);
		}

		public override SiteMapNode BuildSiteMap()
		{
			if (_root != null) return _root;// ClearMap(_root);

			string[] roles = new string[] { "*" };
			Dictionary<string, SiteMapNode> nodes = new Dictionary<string, SiteMapNode>();
			Role rootRole = RoleDAO.GetRootRoles((string.IsNullOrEmpty(VDMSProvider.OrgCode)) ? "/" : VDMSProvider.OrgCode);

			_root = new SiteMapNode(this, rootRole.RoleId.ToString(), rootRole.RoleId.ToString(), rootRole.RoleName, string.Empty);
			_root.Roles = roles;

			if (nodes.ContainsKey(rootRole.RoleId.ToString()))
			{
				throw new ConfigurationErrorsException(_errmsg2);
			}
			nodes.Add(rootRole.RoleId.ToString(), _root);
			//if (_root == null) 
			AddNode(_root, null);
			if (rootRole.Roles.Count > 0)
			{
				CreateNodes(rootRole.Roles, _root, nodes, roles);
			}
			return _root;
		}

		private void CreateNodes(System.Data.Linq.EntitySet<Role> entitySet, SiteMapNode parent, Dictionary<string, SiteMapNode> nodes, string[] roles)
		{
			foreach (Role item in entitySet)
			{
				SiteMapNode node = CreateNode(parent, nodes, item.RoleId.ToString(), item.RoleName, item.ParentRoleIndex.ToString(), item.RoleId.ToString(), roles);
				if (item.Roles.Count > 0)
				{
					CreateNodes(item.Roles, node, nodes, roles);
				}
			}
		}

		private SiteMapNode CreateNode(SiteMapNode parent, Dictionary<string, SiteMapNode> nodes, string id, string title, string desc, string url, string[] roles)
		{
			SiteMapNode node = new SiteMapNode(this, id, url, title, string.Empty);
			node.Roles = roles;
			if (nodes.ContainsKey(id))
			{
				throw new ConfigurationErrorsException(_errmsg2);
			}
			nodes.Add(id, node);
			AddNode(node, parent);
			return node;
		}

		protected override SiteMapNode GetRootNodeCore()
		{
			return BuildSiteMap();
		}

		//protected override void AddNode(SiteMapNode node, SiteMapNode parentNode)
		//{
		//    if (parentNode != null)
		//    {
		//        SiteMapNodeCollection childs = new SiteMapNodeCollection(parentNode.ChildNodes.Count + 1);
		//        childs.AddRange(parentNode.ChildNodes);
		//        childs.Add(node);
		//        parentNode.ChildNodes = childs;
		//    }
		//}
	}
}