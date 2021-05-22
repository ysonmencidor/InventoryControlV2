using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class BATCHNOBALANCEDETAILSRESULT
    {

        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string UOMCode { get; set; }
        public string Batchno { get; set; }
        public string LocationCode { get; set; }
        public double OnHandQty { get; set; }
        public double BFQty { get; set; }
        public double InQty { get; set; }
        public double OutQty { get; set; }
        public double OutdatedQty { get; set; }
        public string StockCategoryName { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
