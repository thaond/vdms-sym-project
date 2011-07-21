using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ComponentModel;

[DataObject(true)]
public class ExcelDataSource
{
    //public int CountData(string query, string tableName, string viewName, int from, int to)
    //{
    //}

    [DataObjectMethod(DataObjectMethodType.Select)]
    public DataTable GetData(string query, string tableName, string viewName, int from, int to)
    {
        string obj = string.IsNullOrEmpty(tableName) ? viewName : tableName;
        string sql = !string.IsNullOrEmpty(query) ? query :
            (string.IsNullOrEmpty(obj) ? string.Empty : string.Format("SELECT * FROM {0}", obj));

        if (from != to)
        {
            sql = string.Format("select * from ({0}) where rownum between {1} and {2}", sql, from, to);
        }
        return (!string.IsNullOrEmpty(sql)) ? new VDMS.WebService.Service.Common().RunQCommand(sql) : null;
    }
}
