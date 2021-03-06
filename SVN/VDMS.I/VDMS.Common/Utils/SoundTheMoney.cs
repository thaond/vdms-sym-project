using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace VDMS.Common.Utils
{
    /// <summary>
    /// This tool used to sound money string in number format to VietNamese.
    /// Decimal value has been supported.
    /// This class was copied, renamed and fixed some sections by ChiNM.
    /// Source: http://www.cuasotinhoc.vn/lofiversion/index.php/t95106.html 
    /// </summary>
    public class Monetary
    {
        private static string ReadGroup3(string G3)
        {
            string[] ReadDigit = new string[10] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bảy", " tám", " chín" };
            string temp = "";
            if (G3 == "000") return "";

            //Đọc số hàng trăm
            temp = ReadDigit[int.Parse(G3[0].ToString())] + " trăm";
            //Đọc số hàng chục
            if (G3[1].ToString() == "0")
                if (G3[2].ToString() == "0") return temp;
                else
                {
                    temp += " lẻ" + ReadDigit[int.Parse(G3[2].ToString())];
                    return temp;
                }
            else
                temp += ReadDigit[int.Parse(G3[1].ToString())] + " mươi";
            //--------------Đọc hàng đơn vị

            if (G3[2].ToString() == "5") temp += " lăm";
            else if (G3[2].ToString() != "0") temp += ReadDigit[int.Parse(G3[2].ToString())];
            return temp;


        }
        private static string ReadGroup9(string G9, string unit)
        {
            string temp = "";
            // Cho đủ 9 số 
            G9 = G9.PadLeft(9, '0');

            string g1 = G9.Substring(0, 3);
            string g2 = G9.Substring(3, 3);
            string g3 = G9.Substring(6, 3);

            //Đọc nhóm 1-----------------------
            if (g1 != "000")
            {
                temp += ReadGroup3(g1);
                temp += " triệu";
            }
            //---------------------------------
            if (g2 != "000")
            {
                temp += ReadGroup3(g2);
                temp += " nghìn";
            }
            //-----------------------------------
            //Chỗ này ko biết có nên ko ?
            //temp =temp + ReadGroup3(g3).Replace("Không Trăm Lẻ","Lẻ"); // Đọc 1000001 là Một Triệu Lẻ Một thay vì Một Triệu Không Trăm Lẻ 1;
            temp = temp + ReadGroup3(g3);

            temp = temp.Trim();
            if (temp != "") temp = temp + " " + unit;

            return temp;
        }

        private static string Group9Unit(int group)
        {
            string unit;
            switch (group)
            {
                case 1: unit = "đồng"; break;
                case 2: unit = "tỷ"; break;
                case 3: unit = "lần"; break;
                default: unit = ""; break;
            }
            return unit;
        }
        private static string DecimalUnit(int group)
        {
            string unit;
            switch (group)
            {
                case 1: unit = "hào"; break;
                case 2: unit = "xu"; break;
                default: unit = ""; break;
            }
            return unit;
        }
        private static string Format(string temp)
        {
            temp = temp.Replace("một mươi", "mười");
            temp = temp.Trim();
            if (temp.IndexOf("không trăm") == 0) temp = temp.Remove(0, 10);
            temp = temp.Trim();
            if (temp.IndexOf("lẻ") == 0) temp = temp.Remove(0, 2);
            temp = temp.Replace("mươi một", "mươi mốt");
            temp.Trim();
            return temp;
        }

        public static string Clean(string src, string sept)
        {
            return src.Replace(" ", "");
        }

        public static string SoundDecimalPart(string dec)
        {
            string temp = "";
            if (dec.Length > 2) dec = dec.Substring(0, 2);
            if (DecimalUnit(1) == "") temp += Format(ReadGroup3(dec.PadLeft(3, '0'))) + " " + DecimalUnit(2);
            else
            {
                temp += Format(ReadGroup3(dec[0].ToString().PadLeft(3, '0'))) + " " + DecimalUnit(1);
                temp += Format(ReadGroup3(dec[1].ToString().PadLeft(3, '0'))) + " " + DecimalUnit(2);
            }
            return temp;
        }
        public static string SoundWholePart(string MoneyStr)
        {
            string temp = "";
            int length = MoneyStr.Length;
            int groupCount = (length / 9) + 1;
            int pos, range;

            for (int i = 1; i <= groupCount; i++)
            {
                pos = length - i * 9;
                range = length - (i - 1) * 9;
                temp = ReadGroup9(MoneyStr.Substring(pos < 0 ? 0 : pos, range > 9 ? 9 : range), Group9Unit(i)) + " " + temp;
            }

            //---------------------------------
            // Tinh chỉnh
            temp = Format(temp);

            temp = (string.IsNullOrEmpty(temp)) ? "không " + Group9Unit(1) : temp;
            return temp;
        }
        public static string Sound(string MoneyStr, string sept)
        {
            MoneyStr = Clean(MoneyStr, sept);
            string[] parts = MoneyStr.Split(new string[] { sept }, StringSplitOptions.RemoveEmptyEntries);
            string temp = "";
            if (parts.Length > 0) temp += SoundWholePart(parts[0]) + " chẵn"; ;
            if (parts.Length > 1) temp += ", lẻ " + SoundDecimalPart(parts[1]);

            temp = temp.Trim();

            //Change Case
            return temp.Substring(0, 1).ToUpper() + temp.Substring(1);
        }
        public static string Sound(string MoneyStr)
        {
            return Sound(MoneyStr, ",");
        }
    }

}
