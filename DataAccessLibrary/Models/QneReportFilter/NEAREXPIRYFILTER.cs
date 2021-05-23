using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class NEAREXPIRYFILTER
    {
        public string CompanyCode { get; set; }
        public DateTime AsOfDate { get; set; } = DateTime.Now;
        public string StockCodes { get; set; }
        public string LocationCode { get; set; }
    }
}
