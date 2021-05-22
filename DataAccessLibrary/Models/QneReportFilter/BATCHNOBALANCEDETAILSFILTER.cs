using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class BATCHNOBALANCEDETAILSFILTER : BaseForm
    {
        public string StockGroup { get; set; } = "%%";
        public string Location { get; set; } = "%%";
        public bool IncludeZeroBalance { get; set; } = true;
    }
}
