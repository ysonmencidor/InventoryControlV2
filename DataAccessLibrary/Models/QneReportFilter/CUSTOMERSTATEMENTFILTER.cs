using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class CUSTOMERSTATEMENTFILTER : BaseForm
    {
        public string AreaId { get; set; } = "%%";
        public string DebtorCategoryId { get; set; } = "%%";
        public string DebtortId { get; set; } = "%%";
        public string SalesPersonId { get; set; } = "%%";
    }
}
