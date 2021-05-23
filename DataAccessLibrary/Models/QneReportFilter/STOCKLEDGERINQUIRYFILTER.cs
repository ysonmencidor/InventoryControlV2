using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class STOCKLEDGERINQUIRYFILTER : BaseForm
    {
        public bool IncludeGST { get; set; }
        public bool IncludeZero { get; set; } = true;
        public bool IncludeInactive { get; set; } = false;
        public string StockCodeFrom { get; set; }
        public string StockCodeTo { get; set; }
        public string LocationCode { get; set; }
        public bool IncludeStockTransfer { get; set; } = false;
        public string StockGroup { get; set; }
    }
}
