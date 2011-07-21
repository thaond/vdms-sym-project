using System.Collections.Generic;
using System.Data;
using VDMS.Data.TipTop;
using VDMS.Helper;

/// <summary>
/// Summary description for AreaHelperDataSource
/// </summary>
namespace VDMS.I.ObjectDataSource
{
	public class ItemRefObject
	{
		public string Value { get; set; }
		public string Display { get; set; }
	}

	public class AreaHelperDataSource
	{
		private List<T> MergeList<T>(List<T> list, List<T> add)
		{
			foreach (T item in add)
			{
				list.Add(item);
			}
			return list;
		}

		public List<ItemRefObject> ListArea()
		{
			List<ItemRefObject> res = new List<ItemRefObject>();
			ItemRefObject iroAll = new ItemRefObject();
			iroAll.Value = string.Empty;
			iroAll.Display = Resources.Constants.All;
			res.Add(iroAll);

			DataSet dsAreas = Area.GetListArea();
			int Count = dsAreas.Tables[0].Rows.Count;
			if (Count > 0)
			{
				foreach (DataRow dr in dsAreas.Tables[0].Rows)
				{
					ItemRefObject iro = new ItemRefObject();
					iro.Value = dr["AreaCode"].ToString();
					iro.Display = dr["AreaName"].ToString();
					res.Add(iro);
				}
			}
			return res;
		}

        //public List<ItemRefObject> ListDealer()
        //{
        //    List<ItemRefObject> res = new List<ItemRefObject>();
        //    ItemRefObject iroAll = new ItemRefObject();
        //    iroAll.Value = string.Empty;
        //    iroAll.Display = Resources.Constants.All;
        //    res.Add(iroAll);

        //    DataSet dsDealers = Dealer.GetListDealer();
        //    int Count = dsDealers.Tables[0].Rows.Count;
        //    if (Count > 0)
        //    {
        //        foreach (DataRow dr in dsDealers.Tables[0].Rows)
        //        {
        //            ItemRefObject iro = new ItemRefObject();
        //            iro.Value = dr["BranchCode"].ToString();
        //            iro.Display = dr["Address"].ToString();
        //            res.Add(iro);
        //        }
        //    }
        //    return res;
        //}

        //public static List<ItemRefObject> GetListBranchByDealerCode()
        //{
        //    return GetListBranchByDealerCode(Helper.UserHelper.DealerCode);
        //}

        //public static List<ItemRefObject> GetListBranchByDealerCode(string DealerCode)
        //{
        //    List<ItemRefObject> res = new List<ItemRefObject>();
        //    ItemRefObject iroAll = new ItemRefObject();
        //    iroAll.Value = string.Empty;
        //    iroAll.Display = Resources.Constants.All;
        //    res.Add(iroAll);

        //    DataSet dsBranch = Dealer.GetListBranchOfDealer(DealerCode);
        //    int Count = dsBranch.Tables[0].Rows.Count;
        //    if (Count > 0)
        //    {
        //        foreach (DataRow dr in dsBranch.Tables[0].Rows)
        //        {
        //            ItemRefObject iro = new ItemRefObject();
        //            iro.Value = dr["BranchCode"].ToString();
        //            iro.Display = dr["Address"].ToString();
        //            res.Add(iro);
        //        }
        //    }
        //    return res;
        //}

        //public List<ItemRefObject> ListDealer(string AreaCode)
        //{
        //    List<ItemRefObject> res = new List<ItemRefObject>();
        //    ItemRefObject iroAll = new ItemRefObject();
        //    iroAll.Value = string.Empty;
        //    iroAll.Display = Resources.Constants.All;
        //    res.Add(iroAll);

        //    DataSet dsDealers;
        //    if (string.IsNullOrEmpty(AreaCode))
        //    {
        //        foreach (string area in AreaHelper.Area)
        //        {
        //            dsDealers = Dealer.GetListDealer(area);
        //            foreach (DataRow dr in dsDealers.Tables[0].Rows)
        //            {
        //                ItemRefObject iro = new ItemRefObject();
        //                iro.Value = dr["BranchCode"].ToString();
        //                iro.Display = dr["BranchCode"].ToString() + " - " + dr["BranchName"].ToString();
        //                res.Add(iro);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        dsDealers = Dealer.GetListDealer(AreaCode);
        //        foreach (DataRow dr in dsDealers.Tables[0].Rows)
        //        {
        //            ItemRefObject iro = new ItemRefObject();
        //            iro.Value = dr["BranchCode"].ToString();
        //            iro.Display = dr["BranchCode"].ToString() + " - " + dr["BranchName"].ToString();
        //            res.Add(iro);
        //        }
        //    }
        //    //if (Helper.UserHelper.DealerCode.Trim().Equals(""))
        //    //    dsDealers = Dealer.GetListDealer();
        //    //else
        //    //    dsDealers = Dealer.GetListDealer(Helper.UserHelper.AreaCode);

        //    return res;
        //}

		public List<ItemRefObject> ListProvice()
		{
			List<ItemRefObject> res = new List<ItemRefObject>();
			ItemRefObject iroAll = new ItemRefObject();
			iroAll.Value = string.Empty;
			iroAll.Display = Resources.Constants.All;
			res.Add(iroAll);

			DataSet dsProvinces = Area.GetListProvince();
			int Count = dsProvinces.Tables[0].Rows.Count;
			if (Count > 0)
			{
				foreach (DataRow dr in dsProvinces.Tables[0].Rows)
				{
					ItemRefObject iro = new ItemRefObject();
					iro.Value = dr["ProviceCode"].ToString();
					iro.Display = dr["ProviceName"].ToString();
					res.Add(iro);
				}
			}
			return res;
		}
	}
}