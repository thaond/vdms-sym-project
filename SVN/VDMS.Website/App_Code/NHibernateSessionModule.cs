using System;
using System.Web;
using Devart.Data.Oracle;
using VDMS.Data.DAL.NHibernateDAL;
using VDMS.II.Linq;
using System.Collections;

namespace VDMS.Web
{
    /// <summary>
    /// Implements the Open-Session-In-View pattern using <see cref="NHibernateSessionManager" />.
    /// Assumes that each HTTP request is given a single transaction for the entire page-lifecycle.
    /// Inspiration for this class came from Ed Courtenay at 
    /// http://sourceforge.net/forum/message.php?msg_id=2847509.
    /// </summary>
    public class NHibernateSessionModule : IHttpModule
    {
        static OracleMonitor monitor = null;

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginTransaction;
            context.EndRequest += CommitAndCloseSession;
        }

        /// <summary>
        /// Opens a session within a transaction at the beginning of the HTTP request.
        /// This doesn't actually open a connection to the database until needed.
        /// </summary>
        private static void BeginTransaction(object sender, EventArgs e)
        {
//            if (monitor == null) monitor = new OracleMonitor();
//            monitor.IsActive = true;
            NHibernateSessionManager.Instance.BeginTransaction();
        }

        /// <summary>
        /// Commits and closes the NHibernate session provided by the supplied <see cref="NHibernateSessionManager"/>.
        /// Assumes a transaction was begun at the beginning of the request; but a transaction or session does
        /// not *have* to be opened for this to operate successfully.
        /// </summary>
        private static void CommitAndCloseSession(object sender, EventArgs e)
        {
            try
            {
                NHibernateSessionManager.Instance.CommitTransaction();

                foreach (DictionaryEntry item in HttpContext.Current.Items)
                {
                    Devart.Data.Linq.DataContext db = item.Value as Devart.Data.Linq.DataContext;
                    if (db != null) db.Dispose();
                }
            }
            finally
            {
                NHibernateSessionManager.Instance.CloseSession();
            }
        }

        public void Dispose() { }
    }
}