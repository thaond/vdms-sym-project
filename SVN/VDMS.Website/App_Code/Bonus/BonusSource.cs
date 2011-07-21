using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDMS.Bonus.Linq;
using VDMS.Bonus.Entity;
using System.ComponentModel;
using VDMS.II.Linq;

namespace VDMS.II.BonusSystem
{
    [DataObject]
    public class BonusSourceDAO
    {
        public BonusSourceDAO()
        {

        }
        public static BonusDataContext DC
        {
            get
            {
                return DCFactory.GetDataContext<BonusDataContext>();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IQueryable<BonusSource> GetAll()
        {
            return DC.BonusSources.AsQueryable();
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void Update(BonusSource o)
        {
            var obj = DC.BonusSources.SingleOrDefault(p => p.BonusSourceId == o.BonusSourceId);
            if (obj != null)
            {
                obj.BonusSourceName = o.BonusSourceName;
                obj.Description = o.Description;
            }
            DC.SubmitChanges();
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void Delete(long id)
        {
            var obj = DC.BonusSources.SingleOrDefault(p => p.BonusSourceId == id);
            if (obj != null)
            {
                DC.BonusSources.DeleteOnSubmit(obj);
                DC.SubmitChanges();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void Delete(BonusSource o)
        {
            Delete(o.BonusSourceId);
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static BonusSource Insert(string name, string desc)
        {
            var o = new BonusSource()
            {
                BonusSourceName = name,
                Description = desc,
            };
            DC.BonusSources.InsertOnSubmit(o);
            DC.SubmitChanges();
            return o;
        }
    }
}