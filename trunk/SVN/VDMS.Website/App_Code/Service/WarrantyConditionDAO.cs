using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.II.Linq;
using VDMS.I.Linq;
using VDMS.I.Entity;
using System.ComponentModel;
using VDMS.Common.Utils;
using Excel;
using System.Data;
using System.IO;

namespace VDMS.I.Service
{
    [DataObject]
    public class WarrantyConditionDAO
    {
        private static List<WarrantyCondition> _importingItems = new List<WarrantyCondition>();

        public static List<WarrantyCondition> ImportingItems { get { return _importingItems.Where(i => i.SessionID == HttpContext.Current.Session.SessionID).ToList(); } }

        public static void ClearSessionData()
        {
            _importingItems.RemoveAll(i => i.SessionID == HttpContext.Current.Session.SessionID);
        }

        public static void ImportExcelData(Stream excel, VDMS.VDMSSetting.SettingData.WarrantySetting setting)
        {
            IExcelDataReader spreadsheet = ExcelReaderFactory.CreateBinaryReader(excel);

            var startRow = setting.StartRow;
            var dateFormat = setting.DateFormat;
            var partCodeCol = setting.PartCode;
            var partNameVNCol = setting.VietnameseName;
            var partNameENCol = setting.EnglishName;
            var motorCodeCol = setting.MotorCode;
            var warrantyTimeCol = setting.WarrantyTime;
            var warrantyLengthCol = setting.WarrantyLength;
            var startDateCol = setting.StartDate;
            var endDateCol = setting.EndDate;

            var rows = spreadsheet.AsDataSet().Tables[0].AsEnumerable();
            var query = from r in rows
                        select new
                        {
                            PartCode = partCodeCol == 0 || partCodeCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(partCodeCol - 1),
                            PartNameVN = partNameVNCol == 0 || partNameVNCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(partNameVNCol - 1),
                            PartNameEN = partNameENCol == 0 || partNameENCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(partNameENCol - 1),
                            MotorCode = motorCodeCol == 0 || motorCodeCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(motorCodeCol - 1),
                            WarrantyTime = warrantyTimeCol == 0 || warrantyTimeCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(warrantyTimeCol - 1),
                            WarrantyLength = warrantyLengthCol == 0 || warrantyLengthCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(warrantyLengthCol - 1),
                            StartDate = startDateCol == 0 || startDateCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(startDateCol - 1),
                            EndDate = endDateCol == 0 || endDateCol > r.ItemArray.Count() ? string.Empty : r.Field<string>(endDateCol - 1),
                        };
            var data = query.Skip(startRow - 1).TakeWhile(v => !string.IsNullOrEmpty(v.PartCode));

            foreach (var item in data)
            {
                long temp;
                int tempInt;

                var newItem = new WarrantyCondition();

                newItem.PartCode = item.PartCode.Trim().ToUpper();
                newItem.PartNameVN = item.PartNameVN;
                newItem.PartNameEN = item.PartNameEN;
                newItem.MotorCode = item.MotorCode;
                newItem.WarrantyLength = long.TryParse(item.WarrantyLength, out temp) ? temp : 0;
                newItem.WarrantyTime = int.TryParse(item.WarrantyTime, out tempInt) ? tempInt : 0;
                newItem.StartDate = DataFormat.DateFromExcel(item.StartDate, dateFormat);
                newItem.StopDate = DataFormat.DateFromExcel(item.EndDate, dateFormat);
                newItem.SessionID = HttpContext.Current.Session.SessionID;

                _importingItems.Add(newItem);
            }
        }

        public static void SaveImportingWarrantyParts()
        {
            var dc = DCFactory.GetDataContext<ServiceDataContext>();
            foreach (var item in ImportingItems)
            {
                var existingItem = dc.WarrantyConditions.SingleOrDefault(p => p.PartCode.ToUpper() == item.PartCode);
                if (existingItem == null)
                {
                    dc.WarrantyConditions.InsertOnSubmit(item);
                }
            }
            dc.SubmitChanges();
        }



        int _partCount;
        public int CountPart(string partCode, string partName, string model)
        {
            return _partCount;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<WarrantyCondition> FindPart(bool byDealerCode, string partCode, string partName, string model, DateTime startDate, DateTime stopDate, int maximumRows, int startRowIndex)
        {
            var dc = DCFactory.GetDataContext<ServiceDataContext>();
            var dc2 = DCFactory.GetDataContext<PartDataContext>();
            var query = dc.WarrantyConditions.AsEnumerable();

            if (!string.IsNullOrEmpty(partCode)) query = query.Where(p => p.PartCode.Contains(partCode.Trim().ToUpper()));
            if (!string.IsNullOrEmpty(partName)) query = query.Where(p => (p.PartNameEN.ToLower().Contains(partName.Trim().ToLower()) || p.PartNameVN.ToLower().Contains(partName.Trim().ToLower())));
            if (!string.IsNullOrEmpty(model)) query = query.Where(p => p.MotorCode.Contains(model.Trim().ToUpper()));
            if (startDate != DateTime.MinValue) query = query.Where(p => p.StartDate >= startDate || p.StartDate == null);
            if (stopDate != DateTime.MinValue) query = query.Where(p => p.StopDate <= stopDate || p.StopDate == null);

            var rs = query.ToList();

            if (byDealerCode)
            {
                var dealerParts = dc2.PartInfos.Where(i => i.DealerCode == VDMS.Helper.UserHelper.DealerCode).Select(i => i.PartCode).ToList();
                rs = rs.Where(p => dealerParts.Contains(p.PartCode)).ToList();
            }

            _partCount = rs.Count();
            if ((startRowIndex >= 0) && (maximumRows > 0)) rs = rs.Skip(startRowIndex).Take(maximumRows).ToList();
            return rs;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<WarrantyCondition> FindPart(string partCode, string partName, string model, int maximumRows, int startRowIndex)
        {
            var dc = DCFactory.GetDataContext<ServiceDataContext>();
            var query = dc.WarrantyConditions.AsEnumerable();

            if (!string.IsNullOrEmpty(partCode)) query = query.Where(p => p.PartCode.Contains(partCode.Trim().ToUpper()));
            if (!string.IsNullOrEmpty(partName)) query = query.Where(p => (p.PartNameEN.ToLower().Contains(partName.Trim().ToLower()) || p.PartNameVN.ToLower().Contains(partName.Trim().ToLower())));
            if (!string.IsNullOrEmpty(model)) query = query.Where(p => p.MotorCode.Contains(model.Trim().ToUpper()));

            var rs = query.ToList();

            _partCount = rs.Count();
            if ((startRowIndex >= 0) && (maximumRows > 0)) rs = rs.Skip(startRowIndex).Take(maximumRows).ToList();
            return rs;
        } 
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<WarrantyCondition> FindPart(string partCode, string partName, string model)
        {
            var dc = DCFactory.GetDataContext<ServiceDataContext>();
            var query = dc.WarrantyConditions.AsEnumerable();

            if (!string.IsNullOrEmpty(partCode)) query = query.Where(p => p.PartCode.Contains(partCode.Trim().ToUpper()));
            if (!string.IsNullOrEmpty(partName)) query = query.Where(p => (p.PartNameEN.ToLower().Contains(partName.Trim().ToLower()) || p.PartNameVN.ToLower().Contains(partName.Trim().ToLower())));
            if (!string.IsNullOrEmpty(model)) query = query.Where(p => p.MotorCode.Contains(model.Trim().ToUpper()));

            var rs = query.ToList();

            _partCount = rs.Count();
            
            return rs.ToList();
        }

        #region Statics

        public static void SyncWithTipTop()
        {
        }

        #endregion
    }
}