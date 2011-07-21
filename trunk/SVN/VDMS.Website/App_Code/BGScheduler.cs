using System;
using System.Threading;
using VDMS.Helper;
using VDMS.II.PartManagement.Order;
using System.Web;

namespace VDMS
{
    public class BGScheduler : IDisposable
	{
		/// <summary>
		/// Determines the status fo the Scheduler
		/// </summary>        
		public bool Cancelled { get; set; }
        //public HttpContext HTTPContext { get; set; }

		/// <summary>
		/// The frequency of checks against hte POP3 box are 
		/// performed in Seconds.
		/// </summary>
		private int CheckFrequency = 680;
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
			Thread t = new Thread(Run);
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

		/// <summary>
		/// Runs the actual processing loop by checking the mail box
		/// </summary>
		private void Run()
		{
			// *** Start out  waiting
			this.WaitHandle.WaitOne(this.CheckFrequency * 1000, true);
			while (!Cancelled)
			{
                VDMS.I.Vehicle.OrderDAO.SysInit();
				// *** Put in 
				this.WaitHandle.WaitOne(this.CheckFrequency * 1000, true);
			}
		}

		void LogMessage(string s)
		{
			FileHelper.WriteLineToFileText("xScheduler.log", string.Concat(DateTime.Now, ": ", s), true);
		}

		#region IDisposable Members
		public void Dispose()
		{
			this.Stop();
		}
		#endregion
	}
}