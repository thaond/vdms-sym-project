using System;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace VDMS.Data.DAL.NHibernateDAL
{
	public class TransactionBlock : IDisposable
	{
		public TransactionBlock()
		{
			object obj = new object();

			if (ActiveTransactions == 0)
			{
				NHibernateSessionManager.Instance.BeginTransaction();
			}
			ActiveTransactions++;
		}


		private int ActiveTransactions
		{
			get
			{
				if (IsInWebContext())
				{
					if (HttpContext.Current.Items[ACTIVE_SESSION_KEY] == null) return 0;
					return (int)HttpContext.Current.Items[ACTIVE_SESSION_KEY];
				}
				else
				{
					if (CallContext.GetData(ACTIVE_SESSION_KEY) == null) return 0;
					return (int)CallContext.GetData(ACTIVE_SESSION_KEY);
				}
			}
			set
			{
				if (IsInWebContext())
				{
					HttpContext.Current.Items[ACTIVE_SESSION_KEY] = value;
				}
				else
				{
					CallContext.SetData(ACTIVE_SESSION_KEY, value);
				}
			}
		}

		private void DisposeActiveTransacions()
		{
			if (IsInWebContext())
			{
				HttpContext.Current.Items.Remove(ACTIVE_SESSION_KEY);
			}
			else
			{
				CallContext.SetData(ACTIVE_SESSION_KEY, null);
			}
		}

		public bool IsValid { get; set; }
		private const string ACTIVE_SESSION_KEY = "ACTIVE_SESSION_KEY";

		private bool IsInWebContext()
		{
			return HttpContext.Current != null;
		}

		public void Dispose()
		{
			ActiveTransactions--;
			if (IsValid)
			{
				if (IsValid && ActiveTransactions == 0 && NHibernateSessionManager.Instance.GetSession().Transaction.IsActive)
				{
					DisposeActiveTransacions();
					NHibernateSessionManager.Instance.CommitTransaction();
				}
			}
			else if (NHibernateSessionManager.Instance.GetSession().Transaction.IsActive)
			{
				DisposeActiveTransacions();
				NHibernateSessionManager.Instance.RollbackTransaction();
			}
		}
	}
}
