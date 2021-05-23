using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class NEAREXPIRYRESULT
    {
        public Guid StockId { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public int AgeDays { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string BaseUOM { get; set; }
        public decimal Qty { get; set; }
        public decimal BalQty { get; set; }
        public Guid StockBatchNumberId { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ManufacturingDate { get; set; }
    }
}
