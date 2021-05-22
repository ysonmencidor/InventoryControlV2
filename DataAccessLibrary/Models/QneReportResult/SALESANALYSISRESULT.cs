using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneReportResult
{
    public class SALESANALYSISRESULT
    {
        public string DocType { get; set; }
        public string DocumentCode { get; set; }
        public DateTime DocumentDate { get; set; }
        public int DocumentPos { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string DocCustomerName { get; set; }
        public string DocSalesPersonCode { get; set; }
        public string DocSalesPersonName { get; set; }
        public string StockCode { get; set; }
        public string IsBundled { get; set; }
        public string StockDescription { get; set; }
        public string DebtorSalesPersonCode { get; set; }
        public string DebtorSalesPersonName { get; set; }
        public string DebtorAreaCode { get; set; }
        public string DebtorAreaName { get; set; }
        public string DebtorCategoryCode { get; set; }
        public string DebtorCategoryName { get; set; }
        public string DebtorControlAccountCode { get; set; }
        public string StockName { get; set; }
        public string StockName2 { get; set; }
        public string StockCategoryCode { get; set; }
        public string StockCategoryName { get; set; }
        public string StockClassCode { get; set; }
        public string StockClassName { get; set; }
        public string StockGroupCode { get; set; }
        public string StockGroupName { get; set; }
        public string StockLocationCode { get; set; }
        public string StockLocationName { get; set; }

        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string DepartmentCode { get; set; }
        public string CostCentreId { get; set; }
        public string CostCentreCode { get; set; }
        public string StockUOM { get; set; }
        public decimal StockWeight { get; set; }
        public string StockWeightUOM { get; set; }
        public decimal StockVolume { get; set; }
        public string StockVolumeUOM { get; set; }


        public decimal StockMinQty { get; set; }
        public decimal StockMaxQty { get; set; }
        public string StockItemType { get; set; }
        public string StockBarCode { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public decimal ItemQty { get; set; }
        public decimal ItemValue { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal ItemDiscountAmount { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
    }
}
