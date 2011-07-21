using System;
using System.Threading;
using VDMS.Common.Utils;
using VDMS.Data.TipTop;
using VDMS.Helper;

namespace VDMS
{
	public class SyncPartSchedulerI : IDisposable
	{
		/// <summary>
		/// Determines the status of the Scheduler
		/// </summary>        
		public bool Cancelled { get; set; }
		//public bool Paused { get; set; }
		public bool OnSyncNew { get; set; }
		public bool OnSyncPrice { get; set; }
		public bool ForceSyncNew { get; set; }
		public bool ForceSyncPrice { get; set; }
		public DateTime? LastSync { get; set; }
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
			LastSync = null;
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
				OnSyncNew = OnSyncPrice = ForceSyncNew = ForceSyncPrice = false;
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
				bool ontime = (VDMSSetting.CurrentSetting.AutoSyncPartDaysI > 0)
					//&& (VDMSSetting.CurrentSetting.AutoSyncPartHourI > 0)
								&& VDMSSetting.CurrentSetting.AllowAutoSyncPartI
								&& (DateTime.Now.Hour == VDMSSetting.CurrentSetting.AutoSyncPartHourI)
								&& ((LastSync == null) || (LastSync != DateTime.Now.Date))
								&& (DateTime.Now.Subtract(VDMSSetting.CurrentSetting.AutoSyncPartFromDateI).Days % VDMSSetting.CurrentSetting.AutoSyncPartDaysI == 0);
				if (ontime
					||
					ForceSyncNew || ForceSyncPrice
				   )
				{
					LogMessage(string.Format("Starting synchronize {0} ----------", (this.ForceSyncNew || this.ForceSyncPrice) ? ", forced by user" : "Automaticaly"));

					if (ontime || ForceSyncNew) this.DoSyncNewParts();
					if (ontime || ForceSyncPrice) this.DoSyncPartsPrice();

					LogMessage("Synchronization finished ----------");
					LogEndMessage(" ");

					LastSync = DateTime.Now.Date;
					ForceSyncNew = false;
					ForceSyncPrice = false;
				}
				// *** Put in 
				this.WaitHandle.WaitOne(this.CheckFrequency * 1000, true);
			}
			LogMessage("Shutting down service <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
			LogEndMessage(" ");
		}
	
		public bool DoSyncNewParts()
		{
			int rows = 0;
			if (this.OnSyncNew) return false;
			this.OnSyncNew = true;

			LogBeginMessage("Inserting new parts...  ");
			try
			{
                rows = Part.SyncNewVDMSIParts(VDMSSetting.CurrentSetting.DefaultLabourOnInsertPartI);
				LogEndMessage(string.Format("done! {0} rows inserted", rows));
			}
			catch (Exception ex)
			{
				LogEndMessage(string.Format("failed: {0}", DataFormat.TraceExceptionMessage(ex)));
			}
			this.OnSyncNew = false;
			return true;
		}

		public bool DoSyncPartsPrice()
		{
			int rows = 0;
			if (this.OnSyncPrice) return false;
			this.OnSyncPrice = true;

			LogBeginMessage("Updating price...  ");
			try
			{
				rows = Part.SyncVDMSIPartsPrice();
				LogEndMessage(string.Format("done! {0} rows updated", rows));
			}
			catch (Exception ex)
			{
				LogEndMessage(string.Format("failed: {0}", DataFormat.TraceExceptionMessage(ex)));
			}

			this.OnSyncPrice = false;
			return true;
		}

		void LogBeginMessage(string s)
		{
			FileHelper.WriteToFileText("SyncPartScheduler.log", string.Concat(DateTime.Now, ": ", s), true);
		}

		void LogEndMessage(string s)
		{
			FileHelper.WriteLineToFileText("SyncPartScheduler.log", s, true);
		}

		void LogMessage(string s)
		{
			FileHelper.WriteLineToFileText("SyncPartScheduler.log", string.Concat(DateTime.Now, ": ", s), true);
		}

		#region IDisposable Members
		public void Dispose()
		{
			this.Stop();
		}
		#endregion
	}
}