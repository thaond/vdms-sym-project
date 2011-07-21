using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.II.Entity;
using VDMS.II.Linq;

namespace VDMS.II.PartManagement
{
	public partial class PartDAO
	{
		public static PartDataContext PartDC
		{
			get
			{
				return DCFactory.GetDataContext<PartDataContext>();
			}
		}

		#region Part List

		int allPart;
		public int SelectAllCount(string partCode, string partName, string engineNo, string model, string manualModel, string partType, string actionType, string warehouseId)
		{
			return allPart;
		}

		/// <summary>
		/// Search the part by the condition
		/// </summary>
		/// <param name="partCode">The part code</param>
		/// <param name="partName">The part name</param>
		/// <param name="engineNo">The engine number</param>
		/// <param name="model">The model</param>
		/// <param name="partType">The type: Part or Accessory</param>
		/// <param name="actionType">The action type: for order, for sales, for special import/export</param>
		/// <param name="warehouseId">The warehouse Id, used for sales or stock transfer</param>
		/// <param name="maximumRows">The maximum rows count, for paging</param>
		/// <param name="startRowIndex">The start row index, for paging</param>
		/// <returns></returns>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public object FindAllPart(string partCode, string partName, string engineNo, string model, string manualModel, string partType, string actionType, string warehouseId, int maximumRows, int startRowIndex)
		{
			long whId;
			if (!long.TryParse(warehouseId, out whId)) whId = UserHelper.WarehouseId;
			if (partName != null) partName = partName.ToLower();

			var db = DCFactory.GetDataContext<PartDataContext>();
			switch (actionType)
			{
				case InventoryAction.Order:
				case InventoryAction.SpecialImport:
				case InventoryAction.SpecialNotGood:
					if (partType == PartType.Part)
					{
						var orderQuery = from t in db.Parts
										 where t.DatabaseCode == UserHelper.DatabaseCode
										 select new
										 {
											 t.PartCode,
											 t.EnglishName,
											 t.VietnamName,
											 t.Model,
											 PartInfoId = 0,
											 CurrentStock = string.Empty
										 };
						if (!string.IsNullOrEmpty(engineNo))
							orderQuery = (from p in orderQuery
										  join q in db.PartModels on p.PartCode equals q.PartCode
										  join v in db.Vehicles on q.Model equals v.ItemCode
										  where v.EngineNumber == engineNo
										  select p).Distinct();
						if (!string.IsNullOrEmpty(partCode)) orderQuery = orderQuery.Where(p => p.PartCode.Contains(partCode));
						if (!string.IsNullOrEmpty(partName)) orderQuery = orderQuery.Where(p => p.EnglishName.ToLower().Contains(partName) || p.VietnamName.ToLower().Contains(partName));
						if (!string.IsNullOrEmpty(manualModel))
							orderQuery = orderQuery.Where(p => p.Model.Contains(manualModel));

						// doi theo yeu cau mr.Giang ngay 1/12/2009
						if (!string.IsNullOrEmpty(model)) //orderQuery = orderQuery.Where(p => p.Model.Contains(model));
							orderQuery = from p in orderQuery
										 join q in db.PartModels on p.PartCode equals q.PartCode
										 where q.Model.Contains(model)
										 select p;
						allPart = orderQuery.Count();
						return orderQuery.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);
					}
					else
					{
						var query = from t in db.Accessories
									join t0 in db.PartInfos on t.AccessoryId equals t0.AccessoryId
									join t1 in db.PartSafeties on t0.PartInfoId equals t1.PartInfoId
									where t0.PartType == partType && t1.WarehouseId == whId
									select new
									{
										t0.PartInfoId,
										t0.PartCode,
										t.EnglishName,
										t.VietnamName,
										t1.SafetyQuantity,
										t1.CurrentStock,
										Model = string.Empty
									};
						allPart = query.Count();
						return query.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);

					}
				case InventoryAction.Sales:
				case InventoryAction.SpecialExport:
				case InventoryAction.StockTransfer:
                    var reservedQuantity = db.SalesDetails.Where(d => d.SalesHeader.DealerCode == UserHelper.DealerCode &&
                                                                      d.SalesHeader.Status == OrderStatus.OrderOpen &&
                                                                      d.SalesHeader.WarehouseId == UserHelper.WarehouseId);
					if (partType == PartType.Part)
					{
						var query = from t in db.Parts
									join t0 in db.PartInfos on t.PartCode equals t0.PartCode
									join t1 in db.PartSafeties on t0.PartInfoId equals t1.PartInfoId
									where t0.PartType == partType && t1.WarehouseId == whId && t1.CurrentStock > 0 && t.DatabaseCode == UserHelper.DatabaseCode
									select new
									{
										t0.PartInfoId,
										t0.PartCode,
										t.EnglishName,
										t.VietnamName,
										t1.SafetyQuantity,
										t1.CurrentStock,
                                        AvailableStock = t1.CurrentStock - 
                                                         (int)db.SalesDetails.Where(d => d.PartCode == t.PartCode && 
                                                                                    d.SalesHeader.Status == OrderStatus.OrderOpen &&
                                                                                    d.SalesHeader.DealerCode == UserHelper.DealerCode &&
                                                                                    d.SalesHeader.WarehouseId == whId).Sum(d => d.OrderQuantity),
										Model = string.Empty
									};
						if (!string.IsNullOrEmpty(partCode)) query = query.Where(p => p.PartCode.Contains(partCode));
						if (!string.IsNullOrEmpty(partName)) query = query.Where(p => p.EnglishName.Contains(partName) || p.VietnamName.Contains(partName));
						if (!string.IsNullOrEmpty(model))
						{
							query = from p in query
									join q in db.PartModels on p.PartCode equals q.PartCode
									where q.Model.Contains(model)
									select p;
						}
						allPart = query.Count();
						return query.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);
					}
					else
					{
						var query = from t in db.Accessories
									join t0 in db.PartInfos on t.AccessoryId equals t0.AccessoryId
									join t1 in db.PartSafeties on t0.PartInfoId equals t1.PartInfoId
									where t0.PartType == partType && t1.WarehouseId == whId && t1.CurrentStock > 0
									select new
									{
										t0.PartInfoId,
										t0.PartCode,
										t.EnglishName,
										t.VietnamName,
										t1.SafetyQuantity,
										t1.CurrentStock,
										Model = string.Empty
									};
						allPart = query.Count();
						return query.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);
					}
				case InventoryAction.CycleCount:
					if (partType == PartType.Part)
					{
						//var query = from t in db.Parts
						//            join t0 in db.PartInfos on t.PartCode equals t0.PartCode
						//            join t1 in db.PartSafeties on t0.PartInfoId equals t1.PartInfoId
						//            where t0.PartType == partType && t1.WarehouseId == whId && t.DatabaseCode == UserHelper.DatabaseCode
						//            select new
						//            {
						//                t0.PartInfoId,
						//                t0.PartCode,
						//                t.EnglishName,
						//                t.VietnamName,
						//                t1.SafetyQuantity,
						//                t1.CurrentStock,
						//                Category = string.Empty
						//            };
						//if (!string.IsNullOrEmpty(partCode)) query = query.Where(p => p.PartCode.Contains(partCode));
						//if (!string.IsNullOrEmpty(partName)) query = query.Where(p => p.EnglishName.Contains(partName) || p.VietnamName.Contains(partName));
						//if (!string.IsNullOrEmpty(model))
						//{
						//    query = from p in query
						//            join q in db.PartModels on p.PartCode equals q.PartCode
						//            where q.Model.Contains(model)
						//            select p;
						//}
						//allPart = query.Count();
						//return query.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);
						var orderQuery = from t in db.Parts
										 where t.DatabaseCode == UserHelper.DatabaseCode
										 select new
										 {
											 t.PartCode,
											 t.EnglishName,
											 t.VietnamName,
											 t.Model,
											 PartInfoId = 0,
											 CurrentStock = string.Empty
										 };
						if (!string.IsNullOrEmpty(partCode)) orderQuery = orderQuery.Where(p => p.PartCode.Contains(partCode));
						if (!string.IsNullOrEmpty(partName)) orderQuery = orderQuery.Where(p => p.EnglishName.Contains(partName) || p.VietnamName.Contains(partName));
						if (!string.IsNullOrEmpty(model)) orderQuery = orderQuery.Where(p => p.Model.Contains(partCode));
						allPart = orderQuery.Count();
						return orderQuery.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);
					}
					else
					{
						var query = from t in db.Accessories
									join t0 in db.PartInfos on t.AccessoryId equals t0.AccessoryId
									join t1 in db.PartSafeties on t0.PartInfoId equals t1.PartInfoId
									where t0.PartType == partType && t1.WarehouseId == whId
									select new
									{
										t0.PartInfoId,
										t0.PartCode,
										t.EnglishName,
										t.VietnamName,
										t1.SafetyQuantity,
										t1.CurrentStock,
										Model = string.Empty
									};
						allPart = query.Count();
						return query.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);
					}
				default:
					return null;
			}
		}

		int newPart = 0;
		public int CountNewPart(DateTime createFrom, DateTime createTo, string category, string databaseCode)
		{
			return newPart;
		}

		/// <summary>
		/// Find the new part from Tip-Top
		/// </summary>
		/// <param name="createFrom"></param>
		/// <param name="createTo"></param>
		/// <param name="category"></param>
		/// <param name="databaseCode"></param>
		/// <param name="maximumRows"></param>
		/// <param name="startRowIndex"></param>
		/// <returns></returns>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindNewPart(DateTime createFrom, DateTime createTo, string category, string databaseCode, int maximumRows, int startRowIndex)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			var query = db.Parts.Where(p => p.DatabaseCode == databaseCode);
			if (createFrom != DateTime.MinValue) query = query.Where(p => p.CreatedDate >= createFrom);
			if (createTo != DateTime.MinValue) query = query.Where(p => p.CreatedDate <= createTo);
			if (!string.IsNullOrEmpty(category)) query = query.Where(p => p.Category == category);

			newPart = query.Count();
			return query.Skip(startRowIndex).Take(maximumRows);
		}

		public static Part GetPart(string PartCode)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return db.Parts.Single(p => p.PartCode == PartCode);
		}

		public static string GetPartName(string PartCode)
		{
			var part = GetPart(PartCode);
			if (UserHelper.Language == "vi-VN") return part.VietnamName;
			return part.EnglishName;
		}

		public static DataSet GetModelList()
		{
			return CacheHelper.GetAndCache<DataSet>(string.Concat("GetModelList", UserHelper.DatabaseCode), () =>
			{
				return VDMS.Data.TipTop.Part.GetModelList(UserHelper.DatabaseCode);
			});
		}

		public static DataSet GetModelList(string partCode, string engineNo)
		{
			return CacheHelper.GetAndCache<DataSet>(string.Concat("GetModelList", UserHelper.DatabaseCode, partCode, engineNo), () =>
			{
				return VDMS.Data.TipTop.Part.GetModelList(UserHelper.DatabaseCode, partCode, engineNo);
			});
		}

		#endregion

		#region Favorite Part

		int favoriteCount;
		public int GetFavoriteCount(string partType, string partCode, string favType)
		{
			return favoriteCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindFavorite(string partType, string partCode, string favType, int maximumRows, int startRowIndex)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			if (partType == "A")
			{
				var query = from f in db.Favorites
							join p in db.Accessories on f.PartCode equals p.AccessoryCode
							where f.PartType == partType && f.Type == favType && f.DealerCode == UserHelper.DealerCode
							select new
							{
								PartCode = f.PartCode,
								EnglishName = p.EnglishName,
								VietnamName = p.VietnamName,
								Rank = f.Rank
							};
				if (!string.IsNullOrEmpty(partCode)) query = query.Where(p => p.PartCode.Contains(partCode.ToLower()) || p.PartCode.Contains(partCode.ToUpper()));
				favoriteCount = query.Count();
				return query.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);
			}
			else
			{
				var query = from f in db.Favorites
							join p in db.Parts on f.PartCode equals p.PartCode
							where f.PartType == partType && f.Type == favType && f.DealerCode == UserHelper.DealerCode && p.DatabaseCode == UserHelper.DatabaseCode
							select new
							{
								PartCode = f.PartCode,
								EnglishName = p.EnglishName,
								VietnamName = p.VietnamName,
								Rank = f.Rank
							};
				if (!string.IsNullOrEmpty(partCode)) query = query.Where(p => p.PartCode.Contains(partCode.ToLower()) || p.PartCode.Contains(partCode.ToUpper()));
				favoriteCount = query.Count();
				return query.OrderBy(p => p.PartCode).Skip(startRowIndex).Take(maximumRows);
			}
		}

		[DataObjectMethod(DataObjectMethodType.Delete)]
		public static void DeleteFavorite(string favType, string PartCode)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			db.Favorites.DeleteAllOnSubmit(db.Favorites.Where(p => p.DealerCode == UserHelper.DealerCode && p.PartCode == PartCode && p.Type == favType));
			db.SubmitChanges();
		}

		public static void DeleteFavorite(Favorite fav)
		{
			PartDAO.PendingDeleteFavorite(fav);
			PartDC.SubmitChanges();
		}

		public static void PendingDeleteFavorite(Favorite fav)
		{
			if (fav != null)
			{
				PartDC.Favorites.DeleteOnSubmit(fav);
			}
		}

		[DataObjectMethod(DataObjectMethodType.Insert)]
		public static void InsertFavorite(string PartCode, int Rank, string favType, string partType)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			if (!PartDAO.IsFavoriteMarked(PartCode, partType, favType))
			{
				db.Favorites.InsertOnSubmit(new Favorite
				{
					DealerCode = UserHelper.DealerCode,
					PartCode = PartCode,
					PartType = partType,
					Rank = Rank,
					Type = favType
				});
				db.SubmitChanges();
			}
		}

		public static bool IsFavoriteMarked(string PartCode, string partType, string favType)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return db.Favorites.SingleOrDefault(p => p.Type == favType && p.PartType == partType && p.PartCode == PartCode && p.DealerCode == UserHelper.DealerCode) != null;
		}

		public static Favorite GetFavorite(string partCode, string partType, string dealerCode, string type)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return db.Favorites.SingleOrDefault(f => f.PartCode == partCode && f.PartType == partType && f.Type == type && f.DealerCode == dealerCode);
		}

		public static Favorite GetFavorite(long favoriteId)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return db.Favorites.SingleOrDefault(f => f.FavoriteId == favoriteId);
		}

		public static int GetFavoriteRank(string partCode, string partType, string dealerCode, string type)
		{
			Favorite fav = PartDAO.GetFavorite(partCode, partType, dealerCode, type);
			if (fav == null) return 0;
			return fav.Rank;
		}

		#endregion

		#region Safety Stock

		int safetyCount;
		[DataObjectMethod(DataObjectMethodType.Select)]
		public int GetSafetyCount(int WarehouseId)
		{
			return safetyCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public int GetSafetyCount()
		{
			return safetyCount;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindAllSafety(long WarehouseId, int maximumRows, int startRowIndex)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			var query = from t in db.Parts
						join t0 in db.PartInfos on t.PartCode equals t0.PartCode
						join t1 in db.PartSafeties on t0.PartInfoId equals t1.PartInfoId
						where t0.PartType == "P" && t1.WarehouseId == WarehouseId && t1.SafetyQuantity > t1.CurrentStock && t.DatabaseCode == UserHelper.DatabaseCode
						select new
						{
							t0.PartCode,
							t.EnglishName,
							t.VietnamName,
							t1.SafetyQuantity,
							t1.CurrentStock
						};
			safetyCount = query.Count();
			return query.Skip(startRowIndex).Take(maximumRows);
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public IQueryable FindAllSafety(int maximumRows, int startRowIndex)
		{
			return FindAllSafety(UserHelper.WarehouseId, maximumRows, startRowIndex);
		}

		#endregion

		#region Helper

		public static bool IsPartCodeValid(string PartCode, string PartType, string DealerCode)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			if (PartType == "P")
				return VDMS.Data.TipTop.Part.IsPartExist(PartCode, UserHelper.DatabaseCode);
			//return db.Parts.Where(p => p.PartCode == PartCode/* && p.DatabaseCode == DatabaseCode*/).Count() != 0;
			else
			{
				//var db = DCFactory.GetDataContext<PartDataContext>();
				return db.Accessories.SingleOrDefault(p => p.AccessoryCode == PartCode && p.DealerCode == DealerCode) != null;
			}
		}

		public static bool IsPartCodeValid(string PartCode, string PartType)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			if (PartType == "P")
				return VDMS.Data.TipTop.Part.IsPartExist(PartCode, UserHelper.DatabaseCode);
			//return db.Parts.SingleOrDefault(p => p.PartCode == PartCode && p.DatabaseCode == UserHelper.DatabaseCode) != null;
			else
			{
				//var db = DCFactory.GetDataContext<PartDataContext>();
				return db.Accessories.SingleOrDefault(p => p.AccessoryCode == PartCode && p.DealerCode == UserHelper.DealerCode) != null;
			}
		}

		public static bool IsPartCodeValid(string PartCode)
		{
			return IsPartCodeValid(PartCode, "P");
		}

		public static bool IsPartCodeValidForCC(string PartCode, string partType)
		{
			if (partType == PartType.Part)
			{
				//return IsPartCodeValidGlobal(PartCode);
				return VDMS.Data.TipTop.Part.IsPartExist(PartCode, "all"); // dqminh Jul 10
			}
			else
			{
				return IsPartCodeValid(PartCode, partType);
			}
		}

		public static bool IsPartCodeValidGlobal(string PartCode)
		{
			var db = DCFactory.GetDataContext<PartDataContext>();
			return db.Parts.Where(p => p.PartCode == PartCode).Count() > 0;
		}

		//public static Part GetPart(string partCode)
		//{
		//    return DCFactory.GetDataContext<PartDataContext>().Parts.Single(p => p.PartCode == partCode);
		//}

		//public static void CreatePart(string partCode, string partName, string model, string type, string spec)
		//{
		//    PartDataContext dc = DCFactory.GetDataContext<PartDataContext>();

		//    VDMS.II.Entity.Part part = new VDMS.II.Entity.Part() { PartCode = partCode, EnglishName = partName, VietnamName = partName, Model = model, Type = type, Specification = spec };
		//    dc.Parts.InsertOnSubmit(part);
		//    dc.SubmitChanges();
		//}

		/// <summary>
		/// Do the stock adjust: import or export the special parts
		/// </summary>
		/// <param name="partCode"></param>
		/// <param name="partType">A: accessory  P: SYM part</param>
		/// <param name="dealerCode"></param>
		/// <param name="wareHouseId"></param>
		/// <param name="transDate"></param>
		/// <param name="transCode"></param>
		/// <param name="cost"></param>
		/// <param name="quantity"></param>
		/// <param name="comment"></param>
		/// <param name="invNo"></param>
		/// <param name="vendorId"></param>
		public static bool StockAdjust(string partCode, string partType, string dealerCode, long wareHouseId, long? toWarehouse, DateTime transDate, string transCode, int cost, int quantity, string comment, string invNo, long? vendorId)
		{
			return StockAdjust(partCode, partType, dealerCode, wareHouseId, toWarehouse, transDate, transCode, cost, quantity, comment, invNo, vendorId, false, true);
		}

		/// <summary>
		/// Do the stock adjust: import or export the special parts
		/// </summary>
		/// <param name="partCode"></param>
		/// <param name="partType"></param>
		/// <param name="dealerCode"></param>
		/// <param name="wareHouseId"></param>
		/// <param name="toWarehouse"></param>
		/// <param name="transDate"></param>
		/// <param name="transCode"></param>
		/// <param name="cost"></param>
		/// <param name="quantity"></param>
		/// <param name="comment"></param>
		/// <param name="invNo"></param>
		/// <param name="vendorId"></param>
		/// <param name="allowOverTimeTrans"></param>
		/// <returns></returns>
		public static bool StockAdjust(string partCode, string partType, string dealerCode, long wareHouseId, long? toWarehouse, DateTime transDate, string transCode, int cost, int quantity, string comment, string invNo, long? vendorId, bool allowOverTimeTrans)
		{
			return StockAdjust(partCode, partType, dealerCode, wareHouseId, toWarehouse, transDate, transCode, cost, quantity, comment, invNo, vendorId, allowOverTimeTrans, true);
		}

		/// <summary>
		/// Do the stock adjust: import or export the special parts
		/// </summary>
		/// <param name="partCode"></param>
		/// <param name="partType"></param>
		/// <param name="dealerCode"></param>
		/// <param name="wareHouseId"></param>
		/// <param name="toWarehouse"></param>
		/// <param name="transDate"></param>
		/// <param name="transCode"></param>
		/// <param name="cost"></param>
		/// <param name="quantity"></param>
		/// <param name="comment"></param>
		/// <param name="invNo"></param>
		/// <param name="vendorId"></param>
		/// <param name="allowOverTimeTrans"></param>
		/// <param name="checkClosedInv"></param>
		/// <returns></returns>
		public static bool StockAdjust(string partCode, string partType, string dealerCode, long wareHouseId, long? toWarehouse, DateTime transDate, string transCode, int cost, int quantity, string comment, string invNo, long? vendorId, bool allowOverTimeTrans, bool checkClosedInv)
		{
			if ((transDate > DateTime.Now) && !allowOverTimeTrans) throw new Exception(Resources.Message.DateConditionErr);
			if (checkClosedInv && InventoryDAO.IsInventoryLock(wareHouseId, transDate.Year, transDate.Month))
			{
				throw new Exception(Resources.Message.InventoryLocked);
			}

			string toDealer = "";
			Warehouse desWh = null;
			if (toWarehouse != null)
			{
				desWh = WarehouseDAO.GetWarehouse((long)toWarehouse);
				if (desWh == null) throw new Exception(string.Format("Warehouse not found: {0}!", toWarehouse));
				toDealer = desWh.DealerCode;
			}

			// truncate time component
			transDate = transDate.Date;

			// get/create part info
			var pInfo = PartInfoDAO.AddPartToDealer(partCode, partType, dealerCode);
			if (pInfo == null) throw new Exception(string.Format("PartInfo failed: {0}!", partCode));
			var pInfo2 = ((toDealer != "") && (toDealer != dealerCode)) ? PartInfoDAO.AddPartToDealer(partCode, partType, toDealer) : pInfo;
			if (pInfo2 == null) throw new Exception(string.Format("PartInfo2 failed: {0}!", partCode));

			// change last months quantity for warehouse
			if (!InventoryDAO.UpdateInventory(pInfo.PartInfoId, wareHouseId, transDate, quantity))
				return false;
			// change last months quantity for dealer
			if ((dealerCode != toDealer) && (!InventoryDAO.UpdateInventory(pInfo.PartInfoId, dealerCode, transDate, quantity)))
				return false;

			// same to receive dealer
			if (toWarehouse != null)
			{
				if (!InventoryDAO.UpdateInventory(pInfo.PartInfoId, (long)toWarehouse, transDate, quantity * -1))
					throw new Exception(string.Format("Update inventory failed for {0}!", pInfo.PartCode));
				if ((dealerCode != toDealer) && (!InventoryDAO.UpdateInventory(pInfo.PartInfoId, toDealer, transDate, quantity * -1)))
					throw new Exception(string.Format("Update inventory2 failed for {0}!", pInfo.PartCode));
			}

			// change current stock (create partSafety)
			PartInfoDAO.IncCurrentStock(pInfo, quantity, wareHouseId);
			if (toWarehouse != null) PartInfoDAO.IncCurrentStock(pInfo2, quantity * -1, (long)toWarehouse);

			// making log
			TransactionHistory th = TransactionDAO.CreateTransaction(pInfo, dealerCode, wareHouseId, toWarehouse, transDate, transCode, cost, quantity, comment, invNo, vendorId);
			if (toWarehouse != null)
			{
				TransactionDAO.CreateTransaction(pInfo2, desWh.DealerCode, desWh.WarehouseId, wareHouseId, transDate, transCode, cost, quantity * -1, comment, invNo, vendorId);
			}
			return true;
		}
		#endregion
	}
}