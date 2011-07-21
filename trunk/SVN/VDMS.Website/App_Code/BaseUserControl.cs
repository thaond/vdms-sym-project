using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using VDMS.Common.Utils;
using VDMS.Helper;

/// <summary>
/// Summary description for BaseUserControl
/// </summary>
public class BaseUserControl : UserControl
{
    #region Eval Data

    public string EvalDate(object date)
    {
        return DataFormat.ToDateString(date);
    }
    public string EvalNumber(object data, int n)
    {
        string fm = string.Format("N{0}", n);
        try { return ((double)data).ToString(fm); }
        catch
        {
            try { return ((long)data).ToString(fm); }
            catch
            {
                try { return ((decimal)data).ToString(fm); }
                catch
                {
                    try { return ((int)data).ToString(fm); }
                    catch { return ""; }
                }
            }
        }
    }
    public string EvalNumber(object data)
    {
        return EvalNumber(data, 0);
    }
    public object SelectLang(object vn, object en)
    {
        return (UserHelper.Language == "vi-VN") ? vn : en;
    }

    #endregion
}
