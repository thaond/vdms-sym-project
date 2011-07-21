using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace VDMS.Data.DAL2
{
	public class AttachmentDao
	{
		public static DataSet GetAttachmentByArticle(long ArticleId)
		{
			string sql = "select ATTACHMENTID, FILENAME from SYM_ATTACHMENT where ARTICLEID = :ARTICLEID";
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(sql);
			db.AddInParameter(dbCommand, ":ARTICLEID", DbType.Int64, ArticleId);
			return db.ExecuteDataSet(dbCommand);
		}
	}
}
