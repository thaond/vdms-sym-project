using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using CrystalDecisions.CrystalReports.Engine;

namespace VDMS.Common.Utils
{
    public class ReportFactory
    {
        static ReportFactory()
        {
            JobsLimit = 70;
        }

        protected static Queue reportQueue = new Queue();

        public static int JobsLimit { get; set; }

        protected static ReportDocument CreateReport()
        {
            ReportDocument report = new ReportDocument();
            reportQueue.Enqueue(report);
            return report;
        }

        public static ReportDocument GetReport()
        {
            //75 is default print job limit. Now use max value is 70 :)
            while (reportQueue.Count >= JobsLimit)
            {
                ((ReportDocument)reportQueue.Dequeue()).Dispose();
            }
            return CreateReport();
        }
    }
}
