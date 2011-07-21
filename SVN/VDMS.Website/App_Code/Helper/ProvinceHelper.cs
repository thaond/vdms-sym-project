using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using VDMS.Data.TipTop;

/// <summary>
/// Summary description for ProvinceHelper
/// </summary>
public class ProvinceHelper
{
    static Hashtable _ProvinceList = null;
    public static Hashtable ProvinceList
    {
        get
        {
            if (_ProvinceList == null)
            {
                _ProvinceList = new Hashtable();
                DataSet data = Area.GetListProvince();
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    _ProvinceList.Add(row["ProviceCode"], row["ProviceName"]);
                }
            }
            return _ProvinceList;
        }
    }
    public static string GetProvinceName(string code)
    {
        return (string)ProvinceList[code];
    }
}
