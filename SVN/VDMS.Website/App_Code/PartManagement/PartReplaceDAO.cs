using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.II.Linq;
using VDMS.II.Entity;
using VDMS.Helper;
using System.ComponentModel;


namespace VDMS.II.PartManagement
{
    public class PartReplace
    {
        public PartReplace(V2PPartReplacement pr, Part p)
        {
            this.CreatedDate = pr.CreatedDate;
            this.DatabaseCode = pr.DatabaseCode;
            this.EditedDate = pr.EditedDate;
            this.OptCode = pr.OptCode;
            this.PartNameEn = p.EnglishName;
            this.PartNameVn = p.VietnamName;
            this.PartCode = pr.PartCode;
            this.PartReplaceId = pr.PartReplaceId;
            this.ReplacePartCode = pr.ReplacePartCode;
            this.Status = pr.Status;
        }

        public DateTime CreatedDate { get; protected set; }
        public string DatabaseCode { get; protected set; }
        public DateTime? EditedDate { get; protected set; }
        public string OptCode { get; protected set; }
        public string PartNameVn { get; protected set; }
        public string PartNameEn { get; protected set; }
        public string PartCode { get; protected set; }
        public long PartReplaceId { get; protected set; }
        public string ReplacePartCode { get; protected set; }
        public string Status { get; protected set; }
        public string PartName { get { return UserHelper.Language == "vi-VN" ? PartNameVn : PartNameEn; } }
    }

    public class PartReplaceStatus
    {
        public static string Active = "Y";
        public static string InActive = "N";
    }

    [DataObject]
    public class PartReplaceDAO : SessionPartDAO<PartData>
    {
        public static PartDataContext DC
        {
            get
            {
                return DCFactory.GetDataContext<PartDataContext>();
            }
        }

        #region Query data

        int _PartReplaceCount;
        public int CountPartReplace(string partCode, string repPartCode, string partName, string status)
        {
            return _PartReplaceCount;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<PartReplace> GetPartReplace(string partCode, string repPartCode, string partName, string status)
        {
            return GetPartReplace(partCode,repPartCode,partName,status, -1, -1);
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<PartReplace> GetPartReplace(string partCode, string repPartCode, string partName, string status, int maximumRows, int startRowIndex)
        {
            if (partCode == null) partCode = "";
            if (repPartCode == null) repPartCode = "";
            if (partName == null) partName = "";
            partCode = partCode.ToUpper();
            repPartCode = repPartCode.ToUpper();

            var part = DC.Parts.Where(p => p.DatabaseCode == UserHelper.DatabaseCode && (p.VietnamName.Contains(partName) || p.EnglishName.Contains(partName)));
            var prep = DC.V2PPartReplacements.Where(p => p.DatabaseCode == UserHelper.DatabaseCode && p.PartCode.Contains(partCode) && p.ReplacePartCode.Contains(repPartCode));
            if (!string.IsNullOrEmpty(status)) prep = prep.Where(p => p.Status == status);

            var query = prep.Join(part, ps => ps.ReplacePartCode, p => p.PartCode, (ps, p) => new PartReplace(ps, p));

            _PartReplaceCount = query.Count();
            if (maximumRows > 0) query = query.Skip(startRowIndex).Take(maximumRows);
            return query;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public V2PPartReplacement GetById(long Id)
        {
            return DC.V2PPartReplacements.SingleOrDefault(p => p.PartReplaceId == Id);
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public string Update(V2PPartReplacement data)
        {
            data.PartCode = data.PartCode.Trim().ToUpper();
            data.ReplacePartCode = data.ReplacePartCode.Trim().ToUpper();
            data.Status = data.Status.Trim().ToUpper();

            var ps = DC.V2PPartReplacements.SingleOrDefault(p => p.PartReplaceId == data.PartReplaceId && p.DatabaseCode == UserHelper.DatabaseCode);
            if (ps != null
                //&& PartDAO.IsPartCodeValid(data.PartCode) 
                && PartDAO.IsPartCodeValid(data.ReplacePartCode, false))
            {
                //ps.DatabaseCode = data.DatabaseCode;
                ps.EditedDate = DateTime.Now;// data.EditedDate;
                //ps.OptCode = data.OptCode;
                ps.PartCode = data.PartCode;
                ps.ReplacePartCode = data.ReplacePartCode;
                ps.Status = data.Status;
                DC.SubmitChanges();
            }
            return ps == null ? "Wrong PartReplacementId!" : "";
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public void Insert(V2PPartReplacement data)
        {
            data.PartCode = data.PartCode.Trim().ToUpper();
            data.ReplacePartCode = data.ReplacePartCode.Trim().ToUpper();
            data.Status = data.Status.Trim().ToUpper();

            var exist = DC.V2PPartReplacements.Any(p => p.DatabaseCode == UserHelper.DatabaseCode && p.PartCode == data.PartCode && p.ReplacePartCode == data.ReplacePartCode);
            if (!exist
                //&& PartDAO.IsPartCodeValid(data.PartCode) 
                && PartDAO.IsPartCodeValid(data.ReplacePartCode, false))
            {
                var ps = new V2PPartReplacement()
                {
                    DatabaseCode = UserHelper.DatabaseCode,
                    CreatedDate = DateTime.Now,
                    //OptCode = data.OptCode,
                    PartCode = data.PartCode,
                    ReplacePartCode = data.ReplacePartCode,
                    Status = data.Status,
                };
                DC.V2PPartReplacements.InsertOnSubmit(ps);
                DC.SubmitChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void Delete(long PartReplaceId)
        {
            var ps = DC.V2PPartReplacements.SingleOrDefault(p => p.PartReplaceId == PartReplaceId && p.DatabaseCode == UserHelper.DatabaseCode);
            if (ps != null)
            {
                DC.V2PPartReplacements.DeleteOnSubmit(ps);
                DC.SubmitChanges();
            }
        }

        public static bool PartCodeCanReplaced(string code)
        {
            return DC.V2PPartReplacements.Any(p => p.PartCode == code.ToUpper() && p.DatabaseCode == UserHelper.DatabaseCode && p.Status == PartReplaceStatus.Active);
        }

        #endregion

        public static void CheckUploadData()
        {
            foreach (var p in Parts)
            {
                if (!PartDAO.IsPartCodeValid(p.NewPartCode, false))
                    p.Error = "Invalid new part code";
                else if (p.Status != PartReplaceStatus.Active && p.Status != PartReplaceStatus.InActive)
                    p.Error = "Invalid status value";
                else if (string.IsNullOrEmpty(p.OldPartCode) || string.IsNullOrEmpty(p.NewPartCode) || string.IsNullOrEmpty(p.Status))
                    p.Error = "Not enough information";
            }
        }

        public static void SavePart()
        {
            var date = DateTime.Now;
            foreach (var i in Parts)
            {
                var pr = DC.V2PPartReplacements.SingleOrDefault(p => p.DatabaseCode == UserHelper.DatabaseCode &&  p.PartCode == i.OldPartCode && p.ReplacePartCode == i.NewPartCode);
                if (pr == null)
                {
                    pr = new V2PPartReplacement();
                    pr.CreatedDate = date;
                    DC.V2PPartReplacements.InsertOnSubmit(pr);
                }
                pr.PartCode = i.OldPartCode;
                pr.ReplacePartCode = i.NewPartCode;
                pr.Status = i.Status;
                pr.DatabaseCode = UserHelper.DatabaseCode;
                pr.EditedDate = date;
            }
            DC.SubmitChanges();
            Parts.Clear();
        }
    }
}