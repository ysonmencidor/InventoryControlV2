using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class GLACCOUNTSFILTER
    {
        public string companyCode { get; set; }
        public List<Guid> AccountIds { get; set; }
    }
}
