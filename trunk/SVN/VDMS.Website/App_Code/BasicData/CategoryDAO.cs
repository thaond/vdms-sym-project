using System.ComponentModel;
using System.Linq;
using VDMS.Helper;
using VDMS.II.Linq;
using VDMS.II.Entity;

namespace VDMS.II.BasicData
{
    public class CategoryDAO
    {
        public int GetCount()
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            return db.Categories.Where(p => p.DealerCode == UserHelper.DealerCode).Count();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IQueryable FindAll(int maximumRows, int startRowIndex)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            return db.Categories.Where(p => p.DealerCode == UserHelper.DealerCode).OrderBy(p => p.Code).Skip(startRowIndex).Take(maximumRows);
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void Delete(long CategoryId)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            db.Categories.DeleteOnSubmit(db.Categories.SingleOrDefault(p => p.CategoryId == CategoryId));
            db.SubmitChanges();
        }

        public void Update(long CategoryId, string Code, string Name)
        {
            if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Name)) return;

            var cat = CategoryDAO.GetCategory(Code);
            if ((cat != null) && (cat.CategoryId != CategoryId)) return;

            cat = CategoryDAO.GetCategory(CategoryId);

            if (cat != null)
            {
                cat.Code = Code;
                cat.Name = Name;
                DC.SubmitChanges();
            }
        }

        #region statics

        public static PartDataContext DC
        {
            get
            {
                return DCFactory.GetDataContext<PartDataContext>();
            }
        }

        public static Category GetCategory(long CategoryId)
        {
            return DC.Categories.SingleOrDefault(c => c.CategoryId == CategoryId);
        }

        public static Category GetCategory(string code)
        {
            return CategoryDAO.GetCategory(code, UserHelper.DealerCode);
        }

        public static Category GetCategory(string code, string dealerCode)
        {
            return DC.Categories.SingleOrDefault(c => c.Code == code && c.DealerCode == dealerCode);
        }

        public static Category CreateCategory(string code, string name, string dealerCode)
        {
            Category cat = CategoryDAO.GetCategory(code, dealerCode);
            if (cat != null) return null;

            cat = new Category() { DealerCode = dealerCode, Name = name, Code = code };
            DC.Categories.InsertOnSubmit(cat);
            DC.SubmitChanges();

            return cat;
        }

        public static Category GetOne()
        {
            return DC.Categories.First(c => c.DealerCode == UserHelper.DealerCode);
        }

        #endregion
    }
}