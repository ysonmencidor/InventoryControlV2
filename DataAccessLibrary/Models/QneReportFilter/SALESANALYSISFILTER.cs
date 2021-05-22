using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class SALESANALYSISFILTER : BaseForm
    {
        public string SalesPersonCode { get; set; }
        public string DebtorCode { get; set; }
        public bool IncludeGST { get; set; } = false;
        public bool IncludeDN { get; set; } = false;
        public bool includeCN { get; set; } = false;
        public bool includeCS { get; set; } = false;
        public bool stockControlOnly { get; set; } = false;
    }
}
