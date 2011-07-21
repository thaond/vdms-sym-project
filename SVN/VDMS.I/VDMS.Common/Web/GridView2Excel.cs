using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;

namespace VDMS.Common.Web
{
    public class GridView2Excel
    {
        /// <summary>
        /// Export a gridview to excel file
        /// </summary>
        /// <param name="grid">The grid control to export</param>
        /// <param name="page">The current page</param>
        /// <param name="Filename">The name of file, include .xls</param>
        public static void Export(WebControl grid, string Filename)
        {
            Export(grid, new Page(), Filename);
        }
        public static void Export(WebControl grid, Page page, string Filename)
        {
            Export(grid, page, Filename, false, null);
        }
        public static void Export(WebControl grid, Page page, string Filename, bool treatAsText, params int[] textColumns)
        {
            if ((treatAsText))
            {
                try
                {
                    foreach (GridViewRow row in ((GridView)grid).Rows)
                        if (textColumns == null || textColumns.Length == 0)
                        {
                            foreach (TableCell cell in row.Cells) cell.Attributes.Add("class", "text");
                        }
                        else
                        {
                            foreach (int i in textColumns) row.Cells[i].Attributes.Add("class", "text");
                        }
                }
                catch { }
            }

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Buffer = true;
            response.AddHeader("content-disposition", string.Concat("attachment;filename=", Filename));
            response.ContentEncoding = Encoding.Unicode;
            response.BinaryWrite(Encoding.Unicode.GetPreamble());

            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = "application/vnd.ms-excel";
            page.EnableViewState = false;
            HtmlForm frm = new HtmlForm();
            page.Controls.Add(frm);
            frm.Controls.Add(grid);
            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            frm.RenderControl(htmlWrite);
            response.Write(@"<style>.text { mso-number-format:\@; }</style>");
            response.Write(stringWrite.ToString());
            response.Flush();
            response.End();
        }

        //protected static string ReadCss()
        //{
        //    string res;
        //    string fileName = string.Format("table.css");
        //    string path = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Themes\Default\" + fileName);

        //    using (StreamReader s = new StreamReader(path))
        //    {
        //        res = s.ReadToEnd();
        //    }
        //    return res;
        //}

        public static bool Save(WebControl grid, string path)
        {
            try
            {
                using (StreamWriter s = new StreamWriter(path, false, Encoding.Unicode))
                {
                    Page page = new Page();
                    page.EnableViewState = false;
                    HtmlForm frm = new HtmlForm();
                    frm.Attributes["runat"] = "server";
                    page.Controls.Add(frm);
                    frm.Controls.Add(grid);
                    StringWriter stringWrite = new StringWriter();
                    HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                    frm.RenderControl(htmlWrite);

                    string start = @"<style>.text { mso-number-format:\@; }</style>";
                    s.Write(start);
                    s.Write(stringWrite.ToString());
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
