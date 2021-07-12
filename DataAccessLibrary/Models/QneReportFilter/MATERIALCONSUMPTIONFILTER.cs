using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportFilter
{
    public class MATERIALCONSUMPTIONFILTER : BaseForm
    { 
        public Guid StockId { get; set; }
        public string AssemblyCode { get; set; }
    }
}
