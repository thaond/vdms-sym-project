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
    public class PartSpec
    {
        public PartSpec(PartSpecification ps, Part p)
        {
            this.PackBy = ps.PackBy;
            this.PackQuantity = ps.PackQuantity;
            this.PackUnit = ps.PackUnit;
            this.PartCode = ps.PartCode;
            this.PartNameEn = p.EnglishName;
            this.PartNameVn = p.VietnamName;
            this.PartSpecId = ps.PartSpecId;
            this.SpecNote = ps.SpecNote;
            this.Status = ps.Status;
        }

        public string PackBy { get; protected set; }
        public int? PackQuantity { get; protected set; }
        public string PackUnit { get; protected set; }
        public string PartCode { get; protected set; }
        public long PartSpecId { get; protected set; }
        public string SpecNote { get; protected set; }
        public string Status { get; protected set; }
        public string PartNameEn { get; protected set; }
        public string PartNameVn { get; protected set; }
        public string PartName { get { return UserHelper.Language == "vi-VN" ? PartNameVn : PartNameEn; } }
    }

    [DataObject]
    public class PartSpecDAO : SessionPartDAO<PartData>
    {
        public static PartDataContext DC
        {
            get
            {
                return DCFactory.GetDataContext<PartDataContext>();
            }
        }

        public static PartSpecification GetPartSpec(string pcode)
        {
            return DC.PartSpecifications.FirstOrDefault(p => p.PartCode == pcode);
        }

        int _PartSpecCount;
        public int CountPartSpec(string partCode, string partName, string unit, string packing)
        {
            return _PartSpecCount;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<PartSpec> GetPartSpec(string partCode, string partName, string unit, string packing)
        {
            return GetPartSpec(partCode, partName, unit, packing, -1, -1);
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable<PartSpec> GetPartSpec(string partCode, string partName, string unit, string packing, int maximumRows, int startRowIndex)
        {
            if (partCode == null) partCode = "";
            if (partName == null) partName = "";
            if (unit == null) unit = "";
            if (packing == null) packing = "";

            var part = DC.Parts.Where(p => p.DatabaseCode == UserHelper.DatabaseCode && (p.VietnamName.Contains(partName) || p.EnglishName.Contains(partName)));
            var pspec = DC.PartSpecifications.Where(p => p.PackBy.Contains(packing) && p.PackUnit.Contains(unit) && p.PartCode.Contains(partCode));
            var query = pspec.Join(part, ps => ps.PartCode, p => p.PartCode, (ps, p) => new PartSpec(ps, p));

            _PartSpecCount = query.Count();
            if (maximumRows > 0) query = query.Skip(startRowIndex).Take(maximumRows);
            return query;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public PartSpecification GetById(long Id)
        {
            return DC.PartSpecifications.SingleOrDefault(p => p.PartSpecId == Id);
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public string Update(PartSpecification data)
        {
            var ps = DC.PartSpecifications.SingleOrDefault(p => p.PartSpecId == data.PartSpecId);
            if (ps != null && PartDAO.IsPartCodeValid(data.PartCode, false))
            {
                ps.PackBy = data.PackBy;
                ps.PartCode = data.PartCode;
                ps.PackQuantity = data.PackQuantity;
                ps.PackUnit = data.PackUnit;
                ps.SpecNote = data.SpecNote;
                //ps.PartCode = data.Status;
                DC.SubmitChanges();
            }
            return ps == null ? "Wrong PartSpecificationId!" : "";
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public void Insert(PartSpecification data)
        {
            data.PartCode = data.PartCode.Trim().ToUpper();

            var exist = DC.PartSpecifications.Any(p => p.PartCode == data.PartCode);
            if (!exist && PartDAO.IsPartCodeValid(data.PartCode, false))
            {
                var ps = new PartSpecification()
                {
                    PackBy = data.PackBy,
                    PackQuantity = data.PackQuantity,
                    PackUnit = data.PackUnit,
                    PartCode = data.PartCode,
                    SpecNote = data.SpecNote,
                    Status = "N",
                };
                DC.PartSpecifications.InsertOnSubmit(ps);
                DC.SubmitChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void DeletePartSpec(long PartSpecId)
        {
            var ps = DC.PartSpecifications.SingleOrDefault(p => p.PartSpecId == PartSpecId);
            if (ps != null)
            {
                DC.PartSpecifications.DeleteOnSubmit(ps);
                DC.SubmitChanges();
            }
        }



        public static void CheckUploadData()
        {
            foreach (var p in Parts)
            {
                if (!PartDAO.IsPartCodeValid(p.PartCode, false))
                    p.Error = "Invalid part code";
                else if (p.Quantity <= 0)
                    p.Error = "Invalid quantity value";
                else if (string.IsNullOrEmpty(p.PartCode) || string.IsNullOrEmpty(p.Packing) || string.IsNullOrEmpty(p.Unit))
                    p.Error = "Not enough information";
            }
        }

        public static void SavePart()
        {
            foreach (var i in Parts)
            {
                var pr = DC.PartSpecifications.SingleOrDefault(p => p.PartCode == i.PartCode);
                if (pr == null)
                {
                    pr = new PartSpecification();
                    DC.PartSpecifications.InsertOnSubmit(pr);
                }
                pr.PartCode = i.PartCode;
                pr.PackBy = i.Packing;
                pr.PackUnit = i.Unit;
                pr.PackQuantity = i.Quantity;
                pr.Status = "N";
            }
            DC.SubmitChanges();
            Parts.Clear();
        }
    }
}