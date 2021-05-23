using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class STOCKISSUEDETAILSRESULT
    {
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string BatchNo { get; set; }
        public double? Qty { get; set; }
        public string UOMCode { get; set; }
        public double? Price { get; set; }
        public double? Amount { get; set; }
        public double TAX { get; set; }
    }
}
