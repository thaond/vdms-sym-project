using System;
using System.Collections.Generic;
using System.Text;

namespace VDMS
{
    namespace Common.Web.JavaScript
    {
        public class StringScript : IClientScriptBlock
        {
            public static string ScriptKey
            {
                get { return "Zero.JavaScript.StringScript"; }
            }

            public static string GetScript()
            {
                StringBuilder script = new StringBuilder();
                script.Append("<script type=\"text/javascript\"> \n");
                script.Append("//<![CDATA[ \n");
                script.Append("function trim(str) { \n");
                script.Append("     return str.replace(/\\s+$/, '').replace(/^\\s+/, ''); \n");
                script.Append("}");
                script.Append("//]]> \n");
                script.Append("</script>");

                return script.ToString();
            }

            public static string GetScriptKey()
            {
                return "Zero.JavaScript.StringScript";
            }
        }
    }
}