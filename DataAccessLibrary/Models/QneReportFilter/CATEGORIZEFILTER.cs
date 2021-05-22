using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class CATEGORIZEFILTER : BaseForm
    {
        public string salesPersonCode { get; set; } = "%%";

        public string category { get; set; } = "%%";

        public bool WithVat { get; set; } = false;
    }
}
