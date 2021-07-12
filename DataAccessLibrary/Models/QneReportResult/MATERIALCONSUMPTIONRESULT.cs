using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class MATERIALCONSUMPTIONRESULT
    {
        
        public string ASSEMBLYCODE { get; set; }
        public string GroupCode { get; set; }
        public string STOCKCODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string UOM { get; set; }
        public double QTY { get; set; }
        public double COST { get; set; }
        public double AMOUNT { get; set; }
        //public string ReferenceNo { get; set; }
        //public DateTime TransactionDate { get; set; }
        public string BatchNo { get; set; }
    }
}
