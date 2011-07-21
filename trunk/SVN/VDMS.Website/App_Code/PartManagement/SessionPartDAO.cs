using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using VDMS.II.Common.ExcelReader;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.PartManagement
{
    [DataObject(true)]
    public class PartData
    {
        public int UnitPrice { get; set; }
        public long PartInfoId { get; set; }
        public bool AlreadyGetPrice { get; set; }
        public object OriginalObj { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string PartType { get; set; }
        public string State { get; set; }
        public int Line { get; set; }
        public int Quantity { get; set; }
        public int SafetyQuantity { get; set; }
        public string Error { get; set; }
        public string Comment { get; set; }
        public string OldPartCode { get; set; }
        public string NewPartCode { get; set; }
        public string Packing { get; set; }
        public string Unit { get; set; }
        public string Status { get; set; }
        public int? OriginalQty { get; set; }
        public char? Quo_Status { get; set; }
        public int QuotationQuantity { get; set; }
        public int? DelivaryQuantity { get; set; }
        public PartData()
        {
            this.State = PartData_Sate.New;
        }
    }

    /// <summary>
    /// Contains the list of object in the session, so this data can be exist between two pages
    /// </summary>
    /// <typeparam name="T">Must inherit from PartData</typeparam>
    public class SessionPartDAO<T> where T : PartData, new()
    {
        protected static string key;

        public static List<T> Parts
        {
            get
            {
                List<T> parts = HttpContext.Current.Session[key] as List<T>;
                if (parts == null)
                {
                    parts = new List<T>();
                    HttpContext.Current.Session[key] = parts;
                }
                return parts;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IEnumerable<T> FindAll()
        {
            return Parts;
        }

        public static int CountAll()
        {
            return Parts.Count;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IEnumerable<T> FindAll(int startRowIndex, int maximumRows)
        {
            return Parts.OrderBy(p => p.Line).Skip(startRowIndex).Take(maximumRows);
        }

        static int error_Count = 0;
        public static int CountAllError()
        {
            return error_Count;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static object GetAllError(int startRowIndex, int maximumRows)
        {
            var q = Parts.Where(p => !string.IsNullOrEmpty(p.Error));
            error_Count = q.Count();

            if (maximumRows > 0) q = q.Skip(startRowIndex).Take(maximumRows);

            return q;
        }

        public static void GetPrice()
        {
            Parts.ForEach(p =>
            {
                if (!p.AlreadyGetPrice)
                {
                    p.AlreadyGetPrice = true;
                    if (p.PartInfoId == 0) return;
                    if (p.PartType == "P")
                        p.UnitPrice = (int)VDMS.Data.TipTop.Part.GetPartPrice(p.PartCode);
                    else
                    {
                        var db = DCFactory.GetDataContext<PartDataContext>();
                        var price = db.PartInfos.Single(u => u.PartInfoId == p.PartInfoId).Price;
                        if (price.HasValue) p.UnitPrice = price.Value;
                    }
                }
            });
        }

        public static void Append(string PartCode, Action<T> Init)
        {
            //  simulate putting this record back into the database
            int lastIndex = Parts.Count;
            if (Parts.SingleOrDefault(x => x.PartCode == PartCode) == null)
            {
                var t = new T
                {
                    Line = lastIndex + 1,
                    PartCode = PartCode
                };
                Init(t);
                Parts.Add(t);
            }
        }

        public static void Append(int RowCount)
        {
            int lastIndex = Parts.Count;
            for (int i = 0; i < RowCount; i++)
            {
                Parts.Add(new T() { Line = lastIndex + i + 1 });
            }
        }

        public static void UpdateLine(int Index, Action<T> action)
        {
            action(Parts.SingleOrDefault(p => p.Line == Index));
        }

        public static void Change(int RowIndex, Action<T> action)
        {
            action(Parts[RowIndex]);
        }

        public static void Delete(int Line)
        {
            Parts.Remove(Parts.SingleOrDefault(p => p.Line == Line));
            Parts.Where(p => p.Line > Line).ToList().ForEach(p => { p.Line--; });
        }

        /// <summary>
        /// Remove item which partcode = null or quantity = 0
        /// mvbinh: Remove item which partcode = null or quantity >=0
        /// </summary>
        public static void CleanUp()
        {
            var t = new List<T>();
            var r = from p in Parts
                    where p.Quantity >= 0 && !string.IsNullOrEmpty(p.PartCode)
                    select p;
            t.AddRange(r);
            Parts.Clear();
            Parts.AddRange(t);
        }

        /// <summary>
        /// Remove item which partcode = null or quantity = 0
        /// </summary>
        public static void CleanByPartCode()
        {
            var t = new List<T>();
            var r = from p in Parts
                    where !string.IsNullOrEmpty(p.PartCode)
                    select p;
            t.AddRange(r);
            Parts.Clear();
            Parts.AddRange(t);
        }

        /// <summary>
        /// Remove all item
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Session.Remove(key);
        }

        public static bool LoadExcelData(Stream excel, VDMS.VDMSSetting.SettingData.ExcelSetting setting)
        {
            var result = false;
            try
            {
                ExcelDataReader spreadsheet = new ExcelDataReader(excel);

                var pcc = setting.PartCodeColumn;
                var qc = setting.QuantityColumn;
                var mc = setting.ModelColumn;
                var sr = setting.StartRow;
                var ptc = setting.PartTypeColumn;
                var stc = setting.SafetyStockColumn;
                var cmtc = setting.CommentColumn;

                var npc = setting.NewPartCodeColumn;
                var opc = setting.OldPartCodeColumn;
                var pckc = setting.PackingColumn;
                var unc = setting.UnitColumn;
                var sc = setting.StatusColumn;

                var rows = spreadsheet.WorkbookData.Tables[0].AsEnumerable();
                var query = from r in rows
                            select new
                            {
                                PartCode = pcc == 0 ? string.Empty : r.Field<string>(pcc - 1),

                                NewPartCode = npc == 0 ? string.Empty : r.Field<string>(npc - 1),
                                OldPartCode = opc == 0 ? string.Empty : r.Field<string>(opc - 1),
                                Packing = pckc == 0 ? string.Empty : r.Field<string>(pckc - 1),
                                Unit = unc == 0 ? string.Empty : r.Field<string>(unc - 1),
                                Status = sc == 0 ? string.Empty : r.Field<string>(sc - 1),

                                Quantity = qc == 0 ? "0" : r.Field<string>(qc - 1),
                                Model = mc == 0 ? string.Empty : r.Field<string>(mc - 1),
                                PartType = ptc == 0 ? string.Empty : r.Field<string>(ptc - 1),
                                SafetyStock = (stc == 0) ? string.Empty : r.Field<string>(stc - 1),
                                Comment = (cmtc == 0) ? string.Empty : r.Field<string>(cmtc - 1),
                            };
                var data = query.Skip(sr - 1).TakeWhile(p => !string.IsNullOrEmpty(p.PartCode) || !string.IsNullOrEmpty(p.OldPartCode));

                List<T> parts = new List<T>();

                int line = 0;
                foreach (var item in data)
                {
                    var p = new T();
                    p.OriginalObj = item;
                    p.Line = ++line;

                    try
                    {
                        p.Comment = item.Comment;
                        p.PartCode = item.PartCode.Trim().ToUpper();
                        p.PartType = item.PartType.Trim();
                        p.Quantity = int.Parse(item.Quantity);
                        p.OldPartCode = item.OldPartCode.Trim().ToUpper();
                        p.NewPartCode = item.NewPartCode.Trim().ToUpper();
                        p.Packing = item.Packing.Trim();
                        p.Unit = item.Unit.Trim();
                        p.Status = item.Status.Trim();
                        p.SafetyQuantity = string.IsNullOrEmpty(item.SafetyStock) ? -1 : int.Parse(item.SafetyStock);
                    }
                    catch
                    {
                        result = false;
                        p.State = PartData_Sate.Invalid;
                        p.Error = Resources.Message.DataFormatWrong;
                    };
                    parts.Add(p);
                }

                HttpContext.Current.Session[key] = parts;
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}