using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class INVOICEMATCHLISTING
    {
        public INVOICEMATCHLISTING()
        {
            this.matchedDetailList = new List<MATCHEDDETAILS>();
        }

        public string INVOICECODE { get; set; }
        public DateTime? INVOICEDATE { get; set; }
        public string INVOICEREFNO { get; set; }
        public string DEBTORACCOUNT { get; set; }
        public string DEBTORNAME { get; set; }
        public string FCURRENCYCODE { get; set; }
        public double LOCALTOTALAMOUNT { get; set; }
        //public string ARCODE { get; set; }
        //public double MATCHEDAMOUNT { get; set; }
        // public DateTime? MATCHEDDATE { get; set; }
        public string SALESPERSONCODE { get; set; }
        public string DEBTORAREACODE { get; set; }
        public string DEBTORCATEGORYCODE { get; set; }

        public List<MATCHEDDETAILS> matchedDetailList { get; set; }

        public double BALANCE { get; set; }
        public int AGING { get; set; }
    }

    public class MATCHEDDETAILS
    {
        public string DOCNO { get; set; }
        public DateTime? DOCDATE { get; set; }
        public double AMOUNT { get; set; }
        public string DESCRIPTION { get; set; }
        public string PAYFORTYPE { get; set; }

        //manually computed
        public int AGING { get; set; }

    }
}
