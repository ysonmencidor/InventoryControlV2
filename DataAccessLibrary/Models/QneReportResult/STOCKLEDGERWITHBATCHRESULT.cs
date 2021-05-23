using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class STOCKLEDGERWITHBATCHRESULT
    {
        public Guid DocumentId { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentCode { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public Guid StockId { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string StockName2 { get; set; }
        public decimal StockIn { get; set; }
        public decimal StockOut { get; set; }
        public decimal PurchaseCost { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountLocal { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string CostCentreCode { get; set; }
        public string CostCentreName { get; set; }
        public string BaseUOM { get; set; }
        public decimal Qty { get; set; }
        public string DocType { get; set; }
        public Guid StockBatchNumberId { get; set; }
        public string BatchNo { get; set; }
        public DateTime BatchNoExpiryDate { get; set; }
        public DateTime BatchNoManufacturingDate { get; set; }
    }
}
