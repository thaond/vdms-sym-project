using System;
using System.Linq;
using System.Threading;
using VDMS.Helper;
using VDMS.II.PartManagement.Order;
using System.Web;
using VDMS.I.Linq;
using VDMS.II.Linq;
using VDMS.I.Vehicle;
using VDMS.I.Entity;
using VDMS.Common.Utils;

namespace VDMS
{
    public class CloseSchedulerI : IDisposable
    {
        /// <summary>
        /// Determines the status of the Scheduler
        /// </summary>        
        public bool Cancelled { get; set; }
        //public bool Paused { get; set; }
        public bool Closing { get; set; }
        public bool ForceClose { get; set; }
        public DateTime? LastClosed { get; set; }
        //public HttpContext HTTPContext { get; set; }

        private Thread t;

        /// <summary>
        /// The frequency of checks against hte POP3 box are 
        /// performed in Seconds.
        /// </summary>
        private int CheckFrequency = 180;
        AutoResetEvent WaitHandle = new AutoResetEvent(false);
        object SyncLock = new Object();

        /// <summary>
        /// Starts the background thread processing       
        /// </summary>
        /// <param name="CheckFrequency">Frequency that checks are performed in seconds</param>
        public void Start(int checkFrequency)
        {
            this.CheckFrequency = checkFrequency;
            this.Cancelled = false;
            LastClosed = null;
            t = new Thread(Run);
            t.Start();
        }

        /// <summary>
        /// Causes the processing to stop. If the operation is still
        /// active it will stop after the current message processing
        /// completes
        /// </summary>
        public void Stop()
        {
            lock (this.SyncLock)
            {
                if (Cancelled) return;
                this.Cancelled = true;
                this.WaitHandle.Set();
            }
        }
        public void Abort()
        {
            lock (this.SyncLock)
            {
                t.Abort();
                Closing = ForceClose = false;
                Start(CheckFrequency);
            }
        }

        /// <summary>
        /// Runs the actual processing loop by checking the mail box
        /// </summary>
        private void Run()
        {
            LogEndMessage(" ");
            LogMessage("Starting Service >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            // *** Start out  waiting
            this.WaitHandle.WaitOne(this.CheckFrequency * 1000, true);
            while (!Cancelled)
            {
                if (
                    //!Paused
                    //&&
                    ((LastClosed == null) || (LastClosed != DateTime.Now.Date))
                    &&
                    VDMSSetting.CurrentSetting.AllowAutoCloseInvI
                    &&
                    ((DateTime.Now.Day == VDMSSetting.CurrentSetting.AutoCloseInvDayI) && (DateTime.Now.Hour == VDMSSetting.CurrentSetting.AutoCloseInvTimeI))
                    ||
                    ForceClose
                   )
                {
                    this.DoCloseAll(DateTime.Now.AddMonths(-1));
                    LastClosed = DateTime.Now.Date;
                    ForceClose = false;
                }
                // *** Put in 
                this.WaitHandle.WaitOne(this.CheckFrequency * 1000, true);
            }
            LogMessage("Shutting down service <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            LogEndMessage(" ");
        }

        public bool DoCloseAll(DateTime defaultCloseDate)
        {
            if (this.Closing) return false;
            this.Closing = true;
            VehicleDataContext vdc = new VehicleDataContext();
            PartDataContext pdc = new PartDataContext();

            DateTime crrMonth = DataFormat.DateOfFirstDayInMonth(DateTime.Now);
            LogMessage(string.Format("Starting Close {0} ----------", this.ForceClose ? ", forced by user" : "Automaticaly"));
            foreach (var d in pdc.Dealers)
            {
                bool hasW = false;
                LogMessage(string.Format("Closing dealer {0}: ", d.DealerCode));
                try
                {
                    // close warehouses
                    foreach (var w in d.ActiveWarehouses.Where(w => w.Type == VDMS.II.Entity.WarehouseType.Vehicle))
                    {
                        LogBeginMessage(string.Format("     Close warehouse {0}: ", w.Code));
                        try
                        {
                            hasW = true;
                            SaleInventoryLock wlck = InventoryHelper.GetInventoryLock(w.DealerCode, w.Code);
                            if (wlck == null)
                            {
                                InventoryHelper.DoCloseW(w.Code, w.DealerCode, defaultCloseDate.Month, defaultCloseDate.Year, vdc);
                            }
                            else
                            {
                                DateTime lastWLock = new DateTime((int)wlck.Year, (int)wlck.Month, 1);
                                lastWLock = lastWLock.AddMonths(1);
                                while (lastWLock < crrMonth)
                                {
                                    InventoryHelper.DoCloseW(w.Code, w.DealerCode, lastWLock.Month, lastWLock.Year, vdc);
                                    lastWLock = lastWLock.AddMonths(1);
                                }
                            }
                            LogEndMessage("Done!");
                        }
                        catch (Exception ex)
                        {
                            LogEndMessage(string.Format("Failed: {0}", DataFormat.TraceExceptionMessage(ex)));
                        }
                    }
                    // close Dealers
                    if (hasW)
                    {
                        SaleInventoryLock dlck = InventoryHelper.GetInventoryLock(d.DealerCode, 0);
                        if (dlck == null)
                        {
                            InventoryHelper.DoCloseD(d.DealerCode, defaultCloseDate.Month, defaultCloseDate.Year, vdc);
                        }
                        else
                        {
                            DateTime lastDLock = new DateTime((int)dlck.Year, (int)dlck.Month, 1);
                            lastDLock = lastDLock.AddMonths(1);
                            while (lastDLock < crrMonth)
                            {
                                InventoryHelper.DoCloseD(d.DealerCode, lastDLock.Month, lastDLock.Year, vdc);
                                lastDLock = lastDLock.AddMonths(1);
                            }
                        }
                        LogMessage(string.Format("Close dealer {0} done!", d.DealerCode));
                    }
                    else
                    {
                        LogMessage(string.Format("{0} has no warehouses!", d.DealerCode));
                    }
                }
                catch (Exception ex)
                {
                    LogMessage(string.Format("Close dealer {0} failed: {1}", d.DealerCode, DataFormat.TraceExceptionMessage(ex)));
                }
                LogEndMessage(" ");
            }
            LogMessage("AutoClose finished ----------");
            LogEndMessage(" ");

            vdc.Dispose();
            pdc.Dispose();
            this.Closing = false;
            return true;
        }

        void LogBeginMessage(string s)
        {
            FileHelper.WriteToFileText("CloseScheduler.log", string.Concat(DateTime.Now, ": ", s), true);
        }
        void LogEndMessage(string s)
        {
            FileHelper.WriteLineToFileText("CloseScheduler.log", s, true);
        }
        void LogMessage(string s)
        {
            FileHelper.WriteLineToFileText("CloseScheduler.log", string.Concat(DateTime.Now, ": ", s), true);
        }
        #region IDisposable Members
        public void Dispose()
        {
            this.Stop();
        }
        #endregion
    }
}