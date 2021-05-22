using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class CUSTOMERSTATEMENTRESULT
    {
        public CUSTOMERSTATEMENTRESULT()
        {
            this.matchedPDCs = new List<MATCHED_PDC>();
        }
        public DateTime? DeliveryOrderDate { get; set; }
        public DateTime? INVOICEDATE { get; set; }

        public string OurDONO { get; set; }
        public string InvoiceCode { get; set; }
        public double TotalAmountLocal { get; set; }
        public double PAIDAMOUNT { get; set; }
        public string DEBTORNAME { get; set; }
        public string TERMS { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string ADDRESS3 { get; set; }
        public string ADDRESS4 { get; set; }

        public int AGING { get; set; }

        public List<MATCHED_PDC> matchedPDCs { get; set; }
    }

    public class MATCHED_PDC
    {
        public MATCHED_PDC() { }
        public double PDC_AMOUNT { get; set; }
        public DateTime? DUEDATE { get; set; }
        public string DETAILS { get; set; }

        public int AGING { get; set; }
    }
}
