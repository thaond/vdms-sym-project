using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;

namespace VDMS.Common.Utils
{
    public enum NumberFormat
    {
        Default,
        CurentMoney
    }
    public class NumberFormatHelper //: System.Decimal
    {
        public static double StrToDouble(string val, string lang)
        {
            double result;
            CultureInfo ci = new CultureInfo(lang);
            double.TryParse(val, NumberStyles.Any, ci, out result);
            return result;
        }
        public static string StrDoubleToStr(string val, string fromLang)
        {
            return StrDoubleToStr(val, fromLang, Thread.CurrentThread.CurrentCulture.Name);
        }
        public static string StrDoubleToStr(string val, string fromLang, string toLang)
        {
            double value = StrToDouble(val, fromLang);
            return DoubleToStr(value, toLang, 3);
        }
        public static string DoubleToStr(double val, string lang)
        {
            return DoubleToStr(val, lang, 3);
        }
        public static string DoubleToStr(double val, string lang, int decDigits)
        {
            CultureInfo ci = new CultureInfo(lang);
            ci.NumberFormat.NumberDecimalDigits = decDigits;

            return val.ToString(ci);
        }

        public static NumberFormatInfo GetCurentNumberFormatForMoney()
        {
            NumberFormatInfo res;
            res = (NumberFormatInfo)System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
            res.NumberDecimalDigits = 0;
            return res;
        }
        public static string NumberToCurentMoneyFormatString(decimal number)
        {
            string res;
            res = number.ToString("N", GetCurentNumberFormatForMoney());
            return res;
        }
        public static string NumberToCurentMoneyFormatString(long number)
        {
            string res;
            res = number.ToString("N", GetCurentNumberFormatForMoney());
            return res;
        }
        public static string NumberToCurentMoneyFormatString(int number)
        {
            string res;
            res = number.ToString("N", GetCurentNumberFormatForMoney());
            return res;
        }
        //public string ToString(NumberFormat nf)
        //{
        //    string res = string.Empty;
        //    switch (nf)
        //    {
        //        case NumberFormat.CurentMoney:
        //            res = NumberFormatHelper.NumberToCurentMoneyFormatString((Decimal)this);
        //            break;
        //        default:
        //            res = NumberFormatHelper.NumberToCurentMoneyFormatString((Decimal)this);
        //            break;
        //    }
        //    return res;
        //}
    }

}
