using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace VDMS.Common.Utils
{
    public static class Ext
    {
        public static string ToString(this object[] arr, string split)
        {
            var res = new StringBuilder();
            if (arr != null)
            {
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    res.AppendFormat("{0}{1}", arr[i].ToString(), split);
                }
            }

            if (arr.Length > 0) res.Append(arr[arr.Length - 1].ToString());

            return res.ToString();
        }
    }

    public class DataFormat
    {
        public static DateTime DateFromString(string dateTime)
        {
            DateTime dt;
            DateTime.TryParse(dateTime, Thread.CurrentThread.CurrentCulture.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces, out dt);
            return dt;
        }

        public static string ToDateString(object dateTime)
        {
            if ((dateTime == null) || (dateTime == DBNull.Value)) return "";
            DateTime dt = (DateTime)dateTime;
            return (dt == DateTime.MinValue) ? "" : dt.ToShortDateString();
        }
        public static string ToDateTimeString(object dateTime)
        {
            if (dateTime == null) return "";
            DateTime dt = (DateTime)dateTime;
            return (dt == DateTime.MinValue) ? "" : dt.ToString();
        }
        public static string DateString(string dateTime)
        {
            DateTime dt;
            DateTime.TryParse(dateTime, Thread.CurrentThread.CurrentCulture.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces, out dt);
            return (dt == DateTime.MinValue) ? "" : dt.ToShortDateString();
        }

        public static int DateToCompareNumber(DateTime date)
        {
            return DateToCompareNumber(date.Day, date.Month, date.Year);
        }

        public static int DateToCompareNumber(int day, int month, int year)
        {
            return year * 10000 + month * 100 + day;
        }

        public static DateTime DateOfFirstDayInMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static string TraceExceptionMessage(Exception ex)
        {
            if (ex == null) return string.Empty;
            else return (ex.InnerException == null) ? ex.Message : string.Format("{0} >>> ", TraceExceptionMessage(ex.InnerException));
        }

        public static DateTime DateFromExcel(string val, string format)
        {
            double dVal;
            DateTime d = DateTime.MinValue;
            if (double.TryParse(val, out dVal))
                d = DateTime.FromOADate(dVal);
            else
            {
                if (string.IsNullOrEmpty(format)) DateTime.TryParse(val, out d);
                else DateTime.TryParseExact(val, format, null, DateTimeStyles.AllowWhiteSpaces, out d);
            }
            return d;
        }
    }
}
