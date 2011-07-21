using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace VDMS.Common.Web.Validator
{
    public class Validator
    {
        public static void SetDateRange(RangeValidator rv, DateTime from, DateTime to, bool setErrMsg)
        {
            SetDateRange(rv, from, to, setErrMsg, null);
        }

        public static void SetDateRange(RangeValidator rv, DateTime from, DateTime to, bool setErrMsg, int? iindex)
        {
            rv.MinimumValue = from.ToShortDateString();
            rv.MaximumValue = to.ToShortDateString();
            rv.Type = ValidationDataType.Date;
            if (setErrMsg)
            {
                if (iindex != null)
                {
                    rv.ErrorMessage = string.Format(rv.ErrorMessage, from.ToShortDateString(), to.ToShortDateString(), iindex.ToString());
                }
                else
                {
                    rv.ErrorMessage = string.Format(rv.ErrorMessage, from.ToShortDateString(), to.ToShortDateString());
                }
            }
        }
    }
}
