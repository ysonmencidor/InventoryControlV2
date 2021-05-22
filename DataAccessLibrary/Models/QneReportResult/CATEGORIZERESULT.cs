using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class CATEGORIZE_FINAL
    {
        public IEnumerable<CATEGORIZERESULT> CATEGORIZERESULTCREDITS { get; set; }
        public IEnumerable<CATEGORIZERESULT> CATEGORIZERESULTS { get; set; }
        public IEnumerable<CATEGORIZE_SUMMARY_AGENT> SUMMARY_FOR_AGENTS { get; set; }
        //public IEnumerable<CATEGORIZE_SUMMARY_AGENT> cATEGORIZE_SUMMARY_AGENTs { get; set; }
        //public IEnumerable<CATEGORIZE_CN_AGENT> cATEGORIZE_CN_AGENTs { get; set; }
    }

    public class CATEGORIZERESULT
    {
        public string SALESPERSONCODE { get; set; }
        public string STOCKCODE { get; set; }
        public string DESCRIPTION { get; set; }
        public double QTY { get; set; }
        public string UOM { get; set; }
        public double AMOUNT { get; set; }
        public double AMOUNTA { get; set; }
        public bool SWINE { get; set; }
        public bool POULTRY { get; set; }
        public bool COMMON { get; set; }
        public string CATEGORY { get; set; }
        public string DEBTOR { get; set; }
        public double UNITPRICE { get; set; }
        public DateTime DELIVERYORDERDATE
        {
            get; set;
        }
        public string DONUMBER { get; set; }
        public string CREDITNOTECODE { get; set; }
        
        public string SALESINVOICECODE { get; set; }
        public DateTime SALESINVOICEDATE
        {
            get;set;
        }
    }
    public class CATEGORIZE_SUMMARY_AGENT
    {
        public string SALESPERSONCODE { get; set; }
        public double AMOUNTA { get; set; }
        public double SWINEA { get; set; }
        public double POULTRYA { get; set; }
        public double COMMONA { get; set; }
    }

    public class CATEGORIZE_CN_AGENT
    {
        public string SALESPERSONCODE { get; set; }
        public string SWINE { get; set; }
        public string POULTRY { get; set; }
        public string COMMON { get; set; }
        public double TOTAL { get; set; }
        public string CREDITNODECODES { get; set; }
    }
}
