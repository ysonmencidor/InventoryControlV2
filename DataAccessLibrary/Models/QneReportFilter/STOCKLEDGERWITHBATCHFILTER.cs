using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class STOCKLEDGERWITHBATCHFILTER : BaseForm
    {
        public bool IncludeZero { get; set; } = true;
        public bool IncludeInactive { get; set; } = false;
        public bool IncludeStockTransfer { get; set; } = false;
        public string StockCodes { get; set; }
        public string LocationCode { get; set; } = "--ALL--";
        public string BatchId { get; set; } = "%%";
        public string StockGroup { get; set; } = "%%";
    }
}
