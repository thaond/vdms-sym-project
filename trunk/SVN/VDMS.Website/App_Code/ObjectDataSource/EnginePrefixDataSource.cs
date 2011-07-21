using System.Data;
using VDMS.Data.TipTop;

namespace VDMS.I.ObjectDataSource
{
    public class EnginePrefixDataSource
    {
        int itemCountByModel;
        public int GetCountByModel(string model)
        {
            return itemCountByModel;
        }
        public DataSet GetPrefixByModel(string model)
        {
            string modelLike = (string.IsNullOrEmpty(model)) ? "%" : model.Trim() + "%";

            DataSet res = Motorbike.GetEnginePrefix(modelLike);
            itemCountByModel = res.Tables[0].Rows.Count;

            return res;
        }
    }
}