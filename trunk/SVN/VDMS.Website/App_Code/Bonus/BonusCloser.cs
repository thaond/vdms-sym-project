using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.Bonus.Entity;
using System.ComponentModel;
using VDMS.Bonus.Linq;
using VDMS.II.Linq;
using VDMS.Common.Utils;
using VDMS.Helper;

/// <summary>
/// Summary description for BonusCloser
/// </summary>
[DataObject]
public class BonusCloser
{
    public static BonusDataContext DC
    {
        get
        {
            return DCFactory.GetDataContext<BonusDataContext>();
        }
    }


    int _CloseDataCount;
    public int CountCloseData(string dCode)
    {
        return _CloseDataCount;
    }
    public IQueryable<Bonus> GetCloseData(int maximumRows, int startRowIndex, string dCode)
    {
        if (dCode == null) dCode = "";
        var q = DC.Bonus.Where(p => p.Dealer.DatabaseCode.Contains(UserHelper.DatabaseCode) && p.DealerCode.Contains(dCode));

        _CloseDataCount = q.Count();
        if (maximumRows > 0) q = q.Skip(startRowIndex).Take(maximumRows);

        return q;
    }

    public static bool IsLockNow(string dCode)
    {
        return IsLock(dCode, DateTime.Now);
    }
    public static bool IsLock(string dCode, DateTime month)
    {
        var b = DC.Bonus.SingleOrDefault(p => p.DealerCode == dCode);
        if (b == null || !b.LockDate.HasValue) return false;
        return DataFormat.DateOfFirstDayInMonth(b.LockDate.Value) >= DataFormat.DateOfFirstDayInMonth(month);
    }
    public static bool Close(string dCode, DateTime? date)
    {
        var b = DC.Bonus.SingleOrDefault(p => p.DealerCode == dCode);
        if (b != null && (b.LockDate.HasValue || date.HasValue))
        {
            if (b.LockDate.HasValue)
                b.LockDate = b.LockDate.Value.AddMonths(1);
            else
                b.LockDate = DataFormat.DateOfFirstDayInMonth(date.Value);
            DC.SubmitChanges();
            return true;
        }
        else
        {
            if (b != null)
                return false;
            else
                throw new Exception("Invalid DealerCode!");
        }
    }
    public static bool Open(string dCode)
    {
        var b = DC.Bonus.SingleOrDefault(p => p.DealerCode == dCode);
        if (b != null && b.LockDate.HasValue)
        {
            b.LockDate = b.LockDate.Value.AddMonths(-1);
            DC.SubmitChanges();
            return true;
        }
        else
        {
            if (b != null)
                return false;
            else
                throw new Exception("Invalid DealerCode!");
        }
    }
}
