using System.Collections.Generic;
using System.Linq;
using System.Data;
using VDMS.II.Entity;
using VDMS.II.Linq;
using System;

namespace VDMS.Helper
{
    public class DealerHelper
    {
        static List<Dealer> dealers;
        static object _lock = new object();
        static Dictionary<string, string> CodeToName = new Dictionary<string, string>();
        static Dictionary<string, string> CodeToAddress = new Dictionary<string, string>();

        static DealerHelper()
        {
            Init();
        }

        public static IList<Dealer> Dealers
        {
            get
            {
                return dealers;
            }
        }

        public static void Unload()
        {
            dealers.Clear();
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        public static void Init()
        {
            lock (_lock)
            {
                var db = DCFactory.GetDataContext<PartDataContext>();
                dealers = db.Dealers.ToList();
                dealers.ForEach(d =>
                {
                    d.Dealers.Load();
                    d.Warehouses.Load();
                });

                // from VDMS-I
                lock (CodeToName)
                {
                    CodeToName.Clear();
                    CodeToAddress.Clear();

                    DataSet ds = VDMS.Data.TipTop.Dealer.GetListDealerALL();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        try
                        {
                            CodeToName.Add(row["BranchCode"] as string, row["BranchName"] as string);
                            CodeToAddress.Add(row["BranchCode"] as string, row["Address"] as string);
                        }
                        catch { }
                    }
                }
            }
        }

        public static string GetName(string Code)
        {
            if (string.IsNullOrEmpty(Code)) return Code;
            var d = dealers.SingleOrDefault(p => p.DealerCode == Code);
            if (d == null) return Code;
            return string.IsNullOrEmpty(d.DealerName) ? Code : d.DealerName;
        }
        
        public static string GetNameI(string Code)
        {
            if (string.IsNullOrEmpty(Code)) return Code;
            if (CodeToName.ContainsKey(Code)) return CodeToName[Code];
            return Code;
        }

        public static string GetAddress(string Code)
        {
            if (string.IsNullOrEmpty(Code)) return Code;
            var d = dealers.SingleOrDefault(p => p.DealerCode == Code);
            if (d == null) return Code;
            return string.IsNullOrEmpty(d.Address) ? Code : d.Address;
        }

        public static string GetAddressI(string Code)
        {
            if (string.IsNullOrEmpty(Code)) return Code;
            if (CodeToAddress.ContainsKey(Code)) return CodeToAddress[Code];
            return Code;
        }

        public static bool GetQuotationCFStatus(string Code)
        {   
            if (string.IsNullOrEmpty(Code)) return false;
            var d = dealers.SingleOrDefault(p => p.DealerCode == Code);
            if (d != null && d.Quo_CF_Status != null)
                return d.Quo_CF_Status.Value;
            else
            {
                if (d.Quo_CF_Status == null)
                    return true;
                else
                    return false;
            }   
        }
        public static string GetAreaCode(string dealercode)
        {
            if (string.IsNullOrEmpty(dealercode)) return string.Empty;
            var d = dealers.SingleOrDefault(p => p.DealerCode == dealercode);
            if (d != null)
                return d.AreaCode;
            else
                return string.Empty;
        }

        public static string GetDatabaseCode(string dealercode)
        {
            if (string.IsNullOrEmpty(dealercode)) return string.Empty;
            var d = dealers.SingleOrDefault(p => p.DealerCode == dealercode);
            if (d != null)
                return d.DatabaseCode;
            else
                return string.Empty;
        }

        public static List<string> AutoVehicleDealerlist()
        {
            var query = dealers.Where(d => d.AutoInStockVehicleStatus == true).Select(v => v.DealerCode).ToList();
            return query;

        }

        public static int GetAutoVehicleDealerSpan(string dealercode)
        {
            if (string.IsNullOrEmpty(dealercode)) return 0;
            var db = DCFactory.GetDataContext<PartDataContext>();
            var d =  db.Dealers.SingleOrDefault(p => p.DealerCode == dealercode);
            if (d != null)
                return d.AutoInStockVehicleSpan;
            else
                return 0;
        }

        public static DateTime GetAutoVehicleStartDate(string dealercode)
        {
            var db = DCFactory.GetDataContext<PartDataContext>();
            var d = db.Dealers.SingleOrDefault(p => p.DealerCode == dealercode);
            if (d != null)
                return d.AutoinstockVehicleStartdate;
            else
                return DateTime.MinValue;
        }

    }
}