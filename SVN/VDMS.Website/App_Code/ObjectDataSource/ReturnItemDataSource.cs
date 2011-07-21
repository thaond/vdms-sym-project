using System.Data;
using VDMS.Data.DAL2;
using VDMS.Helper;
using System;

/// <summary>
/// Summary description for ReturnItemDataSource
/// </summary>
namespace VDMS.I.ObjectDataSource
{
	public class ReturnItemDataSource
	{
        public int SelectCount(string DealerCode, string EngineNumber, DateTime from, DateTime to, string status)
		{
			return (int)ReturnItemDao.GetReturnItemCount(UserHelper.DatabaseCode, DealerCode,EngineNumber, from,to, status);
		}

		public DataSet Select(int maximumRows, int startRowIndex, string DealerCode, string EngineNumber, DateTime from, DateTime to, string status)
		{
			return ReturnItemDao.GetReturnItem(UserHelper.DatabaseCode, DealerCode, EngineNumber, from, to, status, startRowIndex, maximumRows);
		}
	}
}
