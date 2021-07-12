using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class DEPARTMENTLEDGERFILTER : BaseForm
    {
        public List<string> DepartmentAccountCodes { get; set; }

        public List<string> GLAccountCodes { get; set; }

    }
}
