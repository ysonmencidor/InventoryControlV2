using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class BaseForm
    {
        public DateTime? DateFrom { get; set; } = DateTime.Now.AddYears(-10);

        public DateTime? DateTo { get; set; } = DateTime.Now;

        public string CompanyCode { get; set; }

    }
}
