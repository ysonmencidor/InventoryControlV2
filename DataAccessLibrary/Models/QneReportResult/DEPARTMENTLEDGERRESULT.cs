using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class DEPARTMENTLEDGERRESULT
    {
        public string AccountCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string GLAccountCode { get; set; }
        public string GLAccountName { get; set; }
        public string DocumentCode { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        public string ReferenceNo { get; set; }
        public string JournalType { get; set; }
    }
}
