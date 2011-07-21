using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VDMS.II.Entity;

namespace VDMS.II.BasicData
{
	public class TipTopCategoryDAO
	{
		private static List<Category> CatList
		{
			get;
			set;
		}
		static TipTopCategoryDAO()
		{
			CatList = new List<Category>()
            {
                new Category() { Code = "E", Name = "Engine" },
                new Category() { Code = "F", Name = "Frame" },
                new Category() { Code = "N", Name = "Nan hoa" },
            };
		}

		public int CountAll()
		{
			return CatList.Count;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public List<Category> FindAll()
		{
			return CatList;
		}

		#region statics

		public static Category GetCategory(string code)
		{
			return CatList.SingleOrDefault(c => c.Code == code);
		}

		public static Category GetOne()
		{
			return CatList.First();
		}

		public static List<Category> GetAll()
		{
			return CatList;
		}

		#endregion
	}
}