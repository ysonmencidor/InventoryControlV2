using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class FGMASTERFILEFILTER
    {
        public string StockCode { get; set; }
        public bool IsChecked { get; set; }
        public Char Category { get; set; }
        public string companyCode { get; set; }
        public int? CATID { get; set; }
    }
}
