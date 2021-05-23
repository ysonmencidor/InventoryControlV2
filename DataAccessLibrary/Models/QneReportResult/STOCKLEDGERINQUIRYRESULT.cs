using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class STOCKLEDGERINQUIRYRESULT
    {
        public int RowNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentCode { get; set; }
        public string DocType { get; set; }
        public string Description { get; set; }
        public string ItemDescription { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string StockName2 { get; set; }
        public decimal StockIn { get; set; }
        public decimal StockOut { get; set; }
        public decimal PurchaseCost { get; set; }
        public decimal PurchaseCostExcLandingCost { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal AmountLocal { get; set; }
        public decimal NetAmountLocal { get; set; }
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
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public string Remark4 { get; set; }
        public string Remark5 { get; set; }
        public bool IsActive { get; set; }
        public string TransactionTime { get; set; }
        public string BaseUOM { get; set; }
        public decimal Qty { get; set; }
        public decimal RunningBalance { get; set; }
        public Guid DocumentId { get; set; }

    }
}
