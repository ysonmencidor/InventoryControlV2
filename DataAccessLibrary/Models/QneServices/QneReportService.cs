using Dapper;
using DataAccessLibrary.Models.QneReportFilter;
using DataAccessLibrary.Models.QneReportResult;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.QneServices
{
    public class QneReportService
    {
        public async Task<IEnumerable<BATCHNOBALANCEDETAILSRESULT>> GenerateBatchNoBalDetails(BATCHNOBALANCEDETAILSFILTER filter)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode)))
            {
                string sql = @"SELECT A.*,B.StockName FROM 
                                    dbo.FN_BatchNoBalanceDetails(@DateFrom,@DateTo,@UnsalablePeriod,@IncludeZeroBalance) AS A
                                    LEFT JOIN Stocks AS B
                                    ON A.StockId=B.Id
                                    WHERE A.StockGroupCode LIKE @STOCKGROUP AND A.LocationCode LIKE @LOCATIONCODE";

                var fromDate = filter.DateFrom == null ? Convert.ToDateTime("1900-01-01") : filter.DateFrom;
                var toDate = filter.DateTo == null ? DateTime.Now : filter.DateTo;

                var p = new DynamicParameters();
                int IncludeZeroBal = filter.IncludeZeroBalance == true ? 1 : 0;
                p.Add("@STOCKGROUP", filter.StockGroup);
                p.Add("@LOCATIONCODE", filter.Location);
                p.Add("@DateFrom", fromDate);
                p.Add("@DateTo", toDate);
                p.Add("@UnsalablePeriod", 6);
                p.Add("@IncludeZeroBalance", IncludeZeroBal);
                var data = await con.QueryAsync<BATCHNOBALANCEDETAILSRESULT>(sql, p);
                return data;
            }
        }

        public async Task<List<CUSTOMERSTATEMENTRESULT>> GenerateCustomerStatement(CUSTOMERSTATEMENTFILTER filter)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode)))
            {
                string sql = @"SELECT
                                DR.DeliveryOrderDate,
                                SI.OurDONO,
                                INV.InvoiceCode,
                                INV.TotalAmountLocal,
                                (SELECT SUM(ARMatched.Amount) FROM ARMatched WHERE ARMatched.PayForCode = INV.InvoiceCode)
                                AS PAIDAMOUNT,
                                DEBTOR.CompanyCode,
                                DEBTOR.CompanyName AS DEBTORNAME,
                                Agent.Name,
                                Areas.AreaCode AS DEBTORARECODE,
                                Areas.Description AS DEBTORAREADESCRIPTION,
                                CATEGORY.CategoryCode AS DEBTORCATEGORYCODE,
                                INV.InvoiceDate AS INVOICEDATE,
                                Terms.Description as TERMS,
                                DEBTOR.Address1,
                                DEBTOR.Address2,
                                DEBTOR.Address3,
                                DEBTOR.Address4
                                FROM Invoices AS INV 
                                INNER JOIN Debtors as DEBTOR
                                ON INV.DebtorId = DEBTOR.Id
                                LEFT JOIN DebtorCategory AS CATEGORY
                                ON DEBTOR.CategoryId = CATEGORY.Id
                                LEFT JOIN Areas
                                ON DEBTOR.AreaId = Areas.Id
                                LEFT JOIN SalesPersons AS Agent
                                ON INV.SalesPersonId = Agent.Id
                                LEFT JOIN Terms AS Terms
                                ON INV.TermId = Terms.Id
                                LEFT JOIN SalesInvoices as SI
                                ON SI.SalesInvoiceCode = INV.InvoiceCode
                                LEFT JOIN DeliveryOrders as DR
                                ON DR.DeliveryOrderCode = SI.OurDONO
                                WHERE INV.DebtorId LIKE @DEBTORCODE
                                AND INV.SalesPersonId LIKE @SALESPERSONCODE
                                AND DEBTOR.AreaId LIKE @AREACODE
                                AND DEBTOR.CategoryId LIKE @DEBTORCATID
                                AND INV.InvoiceDate BETWEEN @DATEFROM AND @DATETO
                                AND INV.IsCancelled <> 1
                                AND ((SELECT ISNULL(SUM(ARMATCHED.Amount),0) FROM ARMATCHED 
                                WHERE ARMatched.PayForCode = INV.InvoiceCode) < INV.TotalAmountLocal
                                OR(SELECT 
	                                SUM(ARMatched.Amount)
	                                FROM ARMatched 
	                                JOIN Receipts 
	                                ON ARMatched.ARCode = Receipts.ReceiptCode
                                WHERE ARMatched.PayForCode = INV.InvoiceCode AND Receipts.IsPostDatedCheque <> 1)
                                > 0
                                );";
                var fromDate = filter.DateFrom == null ? Convert.ToDateTime("1900-01-01") : filter.DateFrom;
                var toDate = filter.DateTo == null ? DateTime.Now : filter.DateTo;
                var p = new DynamicParameters();
                p.Add("@DEBTORCODE", filter.DebtortId);
                p.Add("@SALESPERSONCODE", filter.SalesPersonId);
                p.Add("@AREACODE", filter.AreaId);
                p.Add("@DEBTORCATID", filter.DebtorCategoryId);
                p.Add("@DATEFROM", fromDate);
                p.Add("@DATETO", toDate);
                var data = await con.QueryAsync<CUSTOMERSTATEMENTRESULT>(sql, p);


                string PDC_MATCHED_QUERY = @"SELECT ARR.TotalAmountLocal,
		                                                  ARR.ChequeDate,
		                                                  ARR.ReferenceNo 
                                            FROM ARMATCHED AS ARM
                                            LEFT JOIN RECEIPTS ARR
                                            ON ARM.ARCode = ARR.ReceiptCode
                                            WHERE ARM.PayForCode = @INVOICECODE AND ARR.IsPostDatedCheque = '1'";
                var CUSTOMERSTATEMENTASLIST = data.ToList();
                for (int i = 0; i < CUSTOMERSTATEMENTASLIST.Count; i++)
                {
                    var inv = CUSTOMERSTATEMENTASLIST[i];
                    string INVOICECODE = CUSTOMERSTATEMENTASLIST[i].InvoiceCode;
                    var param = new DynamicParameters();
                    param.Add("@INVOICECODE", INVOICECODE);
                    var reader = con.ExecuteReader(PDC_MATCHED_QUERY, param);

                    while (reader.Read())
                    {
                        inv.matchedPDCs.Add(new MATCHED_PDC()
                        {
                            DETAILS = reader["ReferenceNo"].ToString().Trim(),
                            PDC_AMOUNT = !String.IsNullOrEmpty(reader["TotalAmountLocal"].ToString().Trim()) ? Convert.ToDouble(reader["TotalAmountLocal"].ToString().Trim()) : 0,
                            DUEDATE = !String.IsNullOrEmpty(reader["ChequeDate"].ToString().Trim()) ? Convert.ToDateTime(reader["ChequeDate"].ToString().Trim()) : (DateTime?)null
                        });
                    }

                    if (inv.matchedPDCs.Count == 0)
                    {
                        DateTime invoiceDate = Convert.ToDateTime(inv.INVOICEDATE);
                        DateTime dateToday = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                        var days = Convert.ToInt32(dateToday.Subtract(invoiceDate).TotalDays);


                        inv.AGING = days;
                    }
                    else
                    {
                        foreach (var m in inv.matchedPDCs)
                        {
                            DateTime invoiceDate = Convert.ToDateTime(inv.INVOICEDATE);
                            DateTime matchedDate = Convert.ToDateTime(m.DUEDATE);
                            var days = Convert.ToInt32(matchedDate.Subtract(invoiceDate).TotalDays);
                            m.AGING = days;
                            //  matchedAmountTotal += m.MATCHEDAMOUNT;
                        }

                    }

                }

                CUSTOMERSTATEMENTASLIST = CUSTOMERSTATEMENTASLIST.Where(i => (i.PAIDAMOUNT < i.TotalAmountLocal) || (i.matchedPDCs.Count > 0)).ToList();

                return CUSTOMERSTATEMENTASLIST;


            }
        }

        public async Task<IEnumerable<SALESANALYSISRESULT>> GenerateSalesAnalysis(SALESANALYSISFILTER filter)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode)))
            {
                string sql = "select * from dbo.FN_SalesAnalysis(@DateFrom,@DateTo,@IncludeGST,@IncludeDN,@includeCN,@includeCS,@stockControlOnly)";
                var fromDate = filter.DateFrom == null ? Convert.ToDateTime("1900-01-01") : filter.DateFrom;
                var toDate = filter.DateTo == null ? DateTime.Now : filter.DateTo;

                var p = new DynamicParameters();
                p.Add("@DateFrom", fromDate);
                p.Add("@DateTo", toDate);
                p.Add("@IncludeGST",filter.IncludeGST);
                p.Add("@IncludeDN",filter.IncludeDN);
                p.Add("@includeCN",filter.includeCN);
                p.Add("@includeCS",filter.includeCS);
                p.Add("@stockControlOnly", filter.stockControlOnly);
                var res = await con.QueryAsync<SALESANALYSISRESULT>(sql,p);
                return res;
            }
        }

        public async Task<CATEGORIZE_FINAL> GenerateCategorizeReport(CATEGORIZEFILTER filter)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode)))
            {

                CATEGORIZE_FINAL FINAL_RESULT = new CATEGORIZE_FINAL();
                string category = filter.category;

                string whereClause = category == "%%" ? "A.StockId IS NOT NULL " : category == "1" ? "SWINE = '1' AND POULTRY = '0' AND COMMON = '0'" : category == "2" ? "SWINE = '0' AND POULTRY = '1' AND COMMON = '0'" : category == "3" ? "SWINE = '0' AND POULTRY = '0' AND COMMON = '1'" : "A.StockId IS NOT NULL";
                //Console.WriteLine(whereClause);
                var p = new DynamicParameters();
                p.Add("@SalesPerson", filter.salesPersonCode);
                p.Add("@dateFrom", filter.DateFrom);
                p.Add("@dateTo", filter.DateTo);
                string queryStringCredit = "";
                string queryStringNonCredit = "";


                if (filter.WithVat)
                {
                    queryStringCredit = @"SELECT G.StaffCode AS SALESPERSONCODE,B.CNDate AS DELIVERYORDERDATE,B.CNCode AS CREDITNOTECODE,
		                    B.DEBTORNAME AS DEBTOR,d.StockName AS DESCRIPTION,
                            CONVERT(BIT, (CASE WHEN SWINE IS NULL OR SWINE = 0 THEN 0 ELSE 1 END)) AS SWINE,
                            CONVERT(BIT, (CASE WHEN COMMON IS NULL OR COMMON = 0 THEN 0 ELSE 1 END)) AS COMMON,
                            CONVERT(BIT, (CASE WHEN POULTRY IS NULL OR POULTRY = 0 THEN 0 ELSE 1 END)) AS POULTRY,
		                    A.QTY,F.UOMCode AS UOM,A.UNITPRICE,A.AMOUNT-A.TaxAmount AS AMOUNT FROM SalesCNDetails A
                                    LEFT JOIN SalesCN B
                                    ON A.SalesCNId = B.Id
                                    LEFT JOIN StockTransactions C
                                    ON c.DocumentId = a.Id AND c.DocType = 'SalesCN' AND B.CNCode = C.DocumentCode
                                    LEFT JOIN Stocks D
                                    ON D.Id = A.StockId 
				                    LEFT JOIN CATEGORIZED_PRODUCTS E
                                    ON E.STOCKCODE = D.StockCode
				                    LEFT JOIN UOMs F
				                    ON F.Id = A.UOMId
				                    LEFT JOIN SalesPersons G
				                    ON G.id = B.SalesPersonId
                                    WHERE " + whereClause + " AND G.StaffCode LIKE @SalesPerson AND B.CNDate BETWEEN @dateFrom AND @dateTo ORDER BY B.CNDate";

                    queryStringNonCredit = @"SELECT F.StaffCode AS SALESPERSONCODE,B.SALESINVOICEDATE,B.SALESINVOICECODE,B.DEBTORNAME AS DEBTOR,A.DESCRIPTION,
                                                CONVERT(BIT, (CASE WHEN SWINE IS NULL OR SWINE = 0 THEN 0 ELSE 1 END)) AS SWINE,
                                                CONVERT(BIT, (CASE WHEN COMMON IS NULL OR COMMON = 0 THEN 0 ELSE 1 END)) AS COMMON,
                                                CONVERT(BIT, (CASE WHEN POULTRY IS NULL OR POULTRY = 0 THEN 0 ELSE 1 END)) AS POULTRY,
                                                A.QTY,E.UOMCode AS UOM,A.UNITPRICE,A.AMOUNT-A.TaxAmount AS AMOUNT FROM SalesInvoiceDetails A 
                                                LEFT JOIN SalesInvoices B
                                                ON A.SalesInvoiceId = B.Id				
				                                LEFT JOIN Stocks AS D
				                                ON D.Id = A.StockId
                                                LEFT JOIN CATEGORIZED_PRODUCTS C
                                                ON C.STOCKCODE = D.StockCode 
				                                LEFT JOIN UOMs AS E
				                                ON E.Id = A.UOMId
				                                LEFT JOIN SalesPersons AS F
				                                ON B.SalesPersonId = F.Id
                                                WHERE " + whereClause + " AND F.StaffCode LIKE @SalesPerson" +
                                                " AND B.SALESINVOICEDATE BETWEEN @dateFrom AND @dateTo ORDER BY B.SALESINVOICEDATE ASC";

                }
                else
                {
                    queryStringCredit = @"SELECT G.StaffCode AS SALESPERSONCODE,B.CNDate AS DELIVERYORDERDATE,B.CNCode AS CREDITNOTECODE,
		                    B.DEBTORNAME AS DEBTOR,d.StockName AS DESCRIPTION,
                            CONVERT(BIT, (CASE WHEN SWINE IS NULL OR SWINE = 0 THEN 0 ELSE 1 END)) AS SWINE,
                            CONVERT(BIT, (CASE WHEN COMMON IS NULL OR COMMON = 0 THEN 0 ELSE 1 END)) AS COMMON,
                            CONVERT(BIT, (CASE WHEN POULTRY IS NULL OR POULTRY = 0 THEN 0 ELSE 1 END)) AS POULTRY,
		                    A.QTY,F.UOMCode AS UOM,A.UNITPRICE,A.AMOUNT FROM SalesCNDetails A
                                    LEFT JOIN SalesCN B
                                    ON A.SalesCNId = B.Id
                                    LEFT JOIN StockTransactions C
                                    ON c.DocumentId = a.Id AND c.DocType = 'SalesCN' AND B.CNCode = C.DocumentCode
                                    LEFT JOIN Stocks D
                                    ON D.Id = A.StockId 
				                    LEFT JOIN CATEGORIZED_PRODUCTS E
                                    ON E.STOCKCODE = D.StockCode
				                    LEFT JOIN UOMs F
				                    ON F.Id = A.UOMId
				                    LEFT JOIN SalesPersons G
				                    ON G.id = B.SalesPersonId
                                    WHERE " + whereClause + " AND G.StaffCode LIKE @SalesPerson AND B.CNDate BETWEEN @dateFrom AND @dateTo ORDER BY B.CNDate";

                    queryStringNonCredit = @"SELECT F.StaffCode AS SALESPERSONCODE,B.SALESINVOICEDATE,B.SALESINVOICECODE,B.DEBTORNAME AS DEBTOR,A.DESCRIPTION,
                                                CONVERT(BIT, (CASE WHEN SWINE IS NULL OR SWINE = 0 THEN 0 ELSE 1 END)) AS SWINE,
                                                CONVERT(BIT, (CASE WHEN COMMON IS NULL OR COMMON = 0 THEN 0 ELSE 1 END)) AS COMMON,
                                                CONVERT(BIT, (CASE WHEN POULTRY IS NULL OR POULTRY = 0 THEN 0 ELSE 1 END)) AS POULTRY,
                                                A.QTY,E.UOMCode AS UOM,A.UNITPRICE,A.AMOUNT FROM SalesInvoiceDetails A 
                                                LEFT JOIN SalesInvoices B
                                                ON A.SalesInvoiceId = B.Id				
				                                LEFT JOIN Stocks AS D
				                                ON D.Id = A.StockId
                                                LEFT JOIN CATEGORIZED_PRODUCTS C
                                                ON C.STOCKCODE = D.StockCode 
				                                LEFT JOIN UOMs AS E
				                                ON E.Id = A.UOMId
				                                LEFT JOIN SalesPersons AS F
				                                ON B.SalesPersonId = F.Id
                                                WHERE " + whereClause + " AND F.StaffCode LIKE @SalesPerson" +
                                                " AND B.SALESINVOICEDATE BETWEEN @dateFrom AND @dateTo ORDER BY B.SALESINVOICEDATE ASC";

                }


                FINAL_RESULT.CATEGORIZERESULTCREDITS = await con.QueryAsync<CATEGORIZERESULT>(queryStringCredit, p, commandType: CommandType.Text);
                FINAL_RESULT.CATEGORIZERESULTS = await con.QueryAsync<CATEGORIZERESULT>(queryStringNonCredit, p, commandType: CommandType.Text);

                var SUMMARY_AGENT = FINAL_RESULT.CATEGORIZERESULTS.GroupBy(X => X.SALESPERSONCODE).Select(g=>g.First()).ToList();

                List<CATEGORIZE_SUMMARY_AGENT> list = new List<CATEGORIZE_SUMMARY_AGENT>();

                foreach (var item in SUMMARY_AGENT)
                {
                    list.Add(new CATEGORIZE_SUMMARY_AGENT { 
                        SALESPERSONCODE = item.SALESPERSONCODE
                    });
                }
                FINAL_RESULT.SUMMARY_FOR_AGENTS = list;

                return FINAL_RESULT;
            }
        }

    }
}
