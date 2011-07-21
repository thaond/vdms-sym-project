using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using VDMS.Helper;
using VDMS.II.BasicData;
using VDMS.I.Entity;
using VDMS.II.Linq;
using VDMS.I.Linq;

namespace VDMS.I.Vehicle
{

    public class VehicleInfo
    {
        public string EngineNumber { get; set; }
        public string ItemType { get; set; }
        public string ItemName { get; set; }
        public string DealerCode { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public int Status { get; set; }
        public string Branch { get; set; }
        public DateTime ImportDate { get; set; }
        
    }

    [DataObject]
    public class SessionVehicleDAO<T> where T : VehicleInfo, new()
    {
        public static void Init(string keyy)
        {
            key = keyy;
        }

        protected static string key;

        public static List<T> Vehicles
        {
            get
            {
                List<T> vehicles = HttpContext.Current.Session[key] as List<T>;
                if (vehicles == null)
                {
                    vehicles = new List<T>();
                    HttpContext.Current.Session[key] = vehicles;
                }
                return vehicles;
            }
        }

        public static void AddVehicle(string engineNo)
        {
            if (Vehicles.SingleOrDefault(v => v.EngineNumber == engineNo) == null)
            {
                var dc = DCFactory.GetDataContext<VehicleDataContext>();
                var q = dc.ItemInstances.SingleOrDefault(i => i.EngineNumber == engineNo);
                if (q != null)
                {

                    Vehicles.Add(new T 
                                { 
                                    Branch = q.BranchCode, 
                                    ColorCode = q.Item.ColorCode, 
                                    DealerCode = UserHelper.DealerCode, 
                                    ColorName = q.Item.ColorName, 
                                    EngineNumber = engineNo, 
                                    ItemName = q.Item.ItemName, 
                                    ItemType = q.Item.ItemType, 
                                    Status = q.Status 
                                });
                }
            }
        }

        public static void AddVehicle(T newItem)
        {
            {
                Vehicles.Add(newItem);
            }
        }

        public static void RemoveVehicle(int index)
        {
            if (index < Vehicles.Count)
                Vehicles.RemoveAt(index);
        }

        public static void RemoveVehicle(string engineNo)
        {
            var removingVehicle = Vehicles.SingleOrDefault(v => v.EngineNumber == engineNo);
            if (removingVehicle != null)
            {
                Vehicles.Remove(removingVehicle);
            }
        }

        public static T GetVehicle(string engineNo)
        {
            return Vehicles.SingleOrDefault(v => v.EngineNumber == engineNo);
        }

        public static void UpdateVehicle(int index, Action<T> act)
        {
            act(Vehicles.Where((v, i) => i == index).SingleOrDefault());
        }

        public static void UpdateVehicle(string engineNo, Action<T> act)
        {
            act(Vehicles.Where(v => v.EngineNumber == engineNo).SingleOrDefault());
        }

        public static void GetVehicleInfo()
        {
            var dc = DCFactory.GetDataContext<VehicleDataContext>();
            var query = dc.ItemInstances.Where(instance => Vehicles.Select(v => v.EngineNumber).Contains(instance.EngineNumber)).ToList();

            foreach (T vehicle in Vehicles)
            {
                var q = query.SingleOrDefault(i => i.EngineNumber == vehicle.EngineNumber);
                if (q != null)
                {
                    vehicle.ColorName = q.Color;
                    vehicle.DealerCode = q.DealerCode;
                    vehicle.ItemType = q.Item.ItemType;
                    vehicle.Status = q.Status;
                }
            }
        }
        
        public static void SaveSession()
        {
            HttpContext.Current.Session[key] = Vehicles;
            
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Remove(key);
        }

    }
}