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

        public async Task<IEnumerable<MATERIALCONSUMPTIONRESULT>> GenerateMaterialConsumptions(MATERIALCONSUMPTIONFILTER filter)
        {
  
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode)))
            {
                string query = @"Select (SELECT TOP 1 StockAssembly.AssemblyCode
		                        FROM
			                        StockAssembly
		                        WHERE
			                        StockAssembly.Id = A.StockAssemblyId
	                        ) AS ASSEMBLYCODE,
                            E.GroupCode,B.StockCode AS STOCKCODE,B.StockName AS DESCRIPTION,C.UOMCode AS UOM,A.Qty AS QTY,A.UnitCost AS COST,
                            A.Qty * A.UnitCost AS AMOUNT,
                            D.BatchNo from StockAssemblyDetail A
                            LEFT JOIN Stocks B
                            ON A.StockId = B.Id
                            LEFT JOIN UOMs C
                            ON A.UOMId = C.Id
                            LEFT JOIN StockBatchNumbers D
                            ON A.StockBatchNumberId = D.Id
                            LEFT JOIN StockGroups E
                            ON E.Id = B.GroupId
                            WHERE A.StockId = @StockId
                            AND A.StockAssemblyId IN (SELECT Id FROM StockAssembly WHERE AssemblyCode like @DocumentCode)";

                string query2 = @"Select (SELECT TOP 1 StockAssembly.AssemblyCode
		                        FROM
			                        StockAssembly
		                        WHERE
			                        StockAssembly.Id = A.StockAssemblyId
	                        ) AS ASSEMBLYCODE,
                            E.GroupCode,B.StockCode AS STOCKCODE,B.StockName AS DESCRIPTION,C.UOMCode AS UOM,A.Qty AS QTY,A.UnitCost AS COST,
                            A.Qty * A.UnitCost AS AMOUNT,
                            D.BatchNo from StockAssemblyDetail A
                            LEFT JOIN Stocks B
                            ON A.StockId = B.Id
                            LEFT JOIN UOMs C
                            ON A.UOMId = C.Id
                            LEFT JOIN StockBatchNumbers D
                            ON A.StockBatchNumberId = D.Id
                            LEFT JOIN StockGroups E
                            ON E.Id = B.GroupId
                            WHERE A.StockId LIKE '%%'
                            AND A.StockAssemblyId IN (SELECT Id FROM StockAssembly WHERE AssemblyCode like '%%')";
                ////var p = new DynamicParameters();
                ////p.Add("@DocumentCode", filter.AssemblyCode);
                ////p.Add("@StockId", filter.StockId);
                var data = await con.QueryAsync<MATERIALCONSUMPTIONRESULT>(query2, commandType: CommandType.Text);
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

        public async Task<IEnumerable<STOCKISSUEDETAILSRESULT>> GenerateStockissuesDetail(STOCKISSUEFILTER filter)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode)))
            {
                string sql = @"select B.StockCode,B.StockName,D.BatchNo,A.Qty,C.UOMCode,A.Price,A.Amount from StockOutDetails AS A
                                LEFT JOIN Stocks AS B
                                ON A.StockId = B.Id
                                LEFT JOIN UOMs AS C
                                ON C.StockId = A.StockId
                                LEFT JOIN StockBatchNumbers AS D
                                ON A.StockBatchNumberId = D.Id
                                WHERE A.StockOutId = @StockIssueId";
                var res = await con.QueryAsync<STOCKISSUEDETAILSRESULT>(sql, new { filter.StockIssueId });
                return res;
            }
        }

        public async Task<IEnumerable<NEAREXPIRYRESULT>> GenerateNearExpiry(NEAREXPIRYFILTER filter)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode)))
            {
                string sql = "SELECT * FROM dbo.FN_NearExpiryItems(@AsofDate,@StockCodes,@Location)";
                var p = new DynamicParameters();
                p.Add("@AsofDate", filter.AsOfDate);
                p.Add("@StockCodes", filter.StockCodes);
                p.Add("@Location", filter.LocationCode);
                var res = await con.QueryAsync<NEAREXPIRYRESULT>(sql, p);
                return res;
            }
        }
        
        public async Task<IEnumerable<STOCKLEDGERINQUIRYRESULT>> GenerateStockLedgerInq(STOCKLEDGERINQUIRYFILTER filter)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode)))
            {
                string sql = "SELECT * FROM dbo.FN_StockLedgerInquiry(@DateFrom,@DateTo,@IncludeGST,@IncludeZero,@IncludeInactive,@StockCodeFrom,@StockCodeTo,@Location,@IncludeStockTransfer)";
                var p = new DynamicParameters();
                p.Add("@DateFrom", filter.DateFrom);
                p.Add("@DateTo", filter.DateTo);
                p.Add("@IncludeGST", filter.IncludeGST);
                p.Add("@IncludeZero", filter.IncludeZero);
                p.Add("@IncludeInactive", filter.IncludeInactive);
                p.Add("@StockCodeFrom", filter.StockCodeFrom);
                p.Add("@StockCodeTo", filter.StockCodeTo);
                p.Add("@Location", filter.LocationCode);
                p.Add("@IncludeStockTransfer", filter.IncludeStockTransfer);
                var res = await con.QueryAsync<STOCKLEDGERINQUIRYRESULT>(sql, p);
                return res;
            }
        }

        public async Task<IEnumerable<STOCKLEDGERWITHBATCHRESULT>> GenerateStockLedgerWBatch(STOCKLEDGERWITHBATCHFILTER filter)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode)))
            {
                string sql = @"SELECT A.*,B.BatchNoExpiryDate,B.BatchNoManufacturingDate FROM dbo.FN_StockLedgerWithBatch(@DateFrom,@DateTo,@IncludeZero,@IncludeInactive,@IncludeStockTransfer,@StockCodes,@Location) AS A                          
                              JOIN StockBatchNumbers AS B
                              ON A.StockBatchNumberId = B.Id
                              WHERE A.StockBatchNumberId LIKE @BatchId AND A.GroupCode LIKE @GroupCode";
                var p = new DynamicParameters();
                p.Add("@DateFrom", filter.DateFrom);
                p.Add("@DateTo", filter.DateTo);
                p.Add("@IncludeZero", filter.IncludeZero);
                p.Add("@IncludeInactive", filter.IncludeInactive);
                p.Add("@IncludeStockTransfer", filter.IncludeStockTransfer);
                p.Add("@StockCodes", filter.StockCodes);
                p.Add("@BatchId", filter.BatchId);
                p.Add("@Location", filter.LocationCode);
                p.Add("@GroupCode", filter.StockGroup);
                var model = await con.QueryAsync<STOCKLEDGERWITHBATCHRESULT>(sql, p);
                return model;
                //if(filter.BatchId != "%%")
                //{
                //    model.Where(x => x.StockBatchNumberId == Guid.Parse(filter.BatchId));
                //}
                //return model.ToList();
            }
        }

        public async Task<List<INVOICEMATCHLISTING>> GenerateInvoiceMatchListing(INVOICEMATCHLISTINGFILTER filter)
        {
            List<INVOICEMATCHLISTING> InvoiceMatchListing = new List<INVOICEMATCHLISTING>();
            using IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode));

            string sql = @"SELECT
                        INV.InvoiceCode,
                        INV.InvoiceDate,
                        INV.ReferenceNo,
                        DEBT.CompanyName AS DEBTORNAME,
                        CUR.CurrencyCode,
                        INV.TotalAmountLocal,
                        SALES.StaffCode AS SALESPERSONCODE,
                        AREA.AreaCode AS DEBTORAREACODE,
                        DEBTORCAT.CategoryCode AS DEBTORCATEGORYCODE
                        FROM Invoices INV
                        INNER JOIN Debtors DEBT
                        ON INV.DebtorId = DEBT.Id
                        LEFT JOIN Currencies CUR
                        ON CUR.Id = INV.CurrencyId
                        LEFT JOIN SalesPersons SALES
                        ON SALES.Id = INV.SalesPersonId
                        LEFT JOIN Areas AREA
                        ON AREA.Id = DEBT.AreaId
                        LEFT JOIN DebtorCategory DEBTORCAT
                        ON DEBTORCAT.Id = DEBT.CategoryId
                        WHERE INV.IsCancelled <> 0 AND 
                        INV.DebtorId LIKE @DEBTORCODE
                        AND INV.SalesPersonId LIKE @SALESPERSONCODE
                        AND DEBT.AreaId LIKE @AREACODE
                        AND DEBT.CategoryId LIKE @DEBTORCATID
                        AND INV.InvoiceDate BETWEEN @DATEFROM AND @DATETO
                        ORDER BY INV.InvoiceCode";
            var fromDate = filter.DateFrom == null ? Convert.ToDateTime("1900-01-01") : filter.DateFrom;
            var toDate = filter.DateTo == null ? DateTime.Now : filter.DateTo;
            var p = new DynamicParameters();
            p.Add("@DEBTORCODE", filter.DebtortId);
            p.Add("@SALESPERSONCODE", filter.SalesPersonId);
            p.Add("@AREACODE", filter.AreaId);
            p.Add("@DEBTORCATID", filter.DebtorCategoryId);
            p.Add("@DATEFROM", fromDate);
            p.Add("@DATETO", toDate);
            var data = await con.QueryAsync<INVOICEMATCHLISTING>(sql, p);

            var InvoiceForMatching = data.ToList();
            string SQL_INVOICEMATCH_QUERY = @"SELECT 
                            ARM.PayForCode AS InvNo,
                            ARM.ARCode AS DocNo,
                            CAST('OR' AS varchar(20)) AS PayForType,
                            ARR.ClearedDate AS DocDate,
                            ARR.Description,
                            ARR.TotalAmount AS OrAmt
                            FROM ARMatched ARM
                            JOIN Receipts ARR ON
                            ARR.ReceiptCode = ARM.ARCode
                            WHERE
                            PayForType = 'INV'
                            AND Amount <> 0
                            AND ARM.PayForCode = @INVOICECODE
                            UNION
                            SELECT
                            ARM.PayForCode AS InvNo,
                            ARM.ARCode AS DocNo,
                            CAST('CN' AS varchar(20)) AS PayForType,
                            ARCN.CNDate AS DocDate,
                            ARCN.Description,
                            ARCN.TotalAmount AS OrAmt
                            FROM ARMatched ARM
                            JOIN ARCN 
                            ON ARCN.CNCode = ARM.ARCode
                            WHERE PayForType = 'INV'
                            AND Amount <> 0
                            AND ARM.PayForCode = @INVOICECODE
                            UNION
                            SELECT 
                            ARM.PayForCode AS InvNo,
                            ARM.ARCode AS DocNo,
                            CAST('CN' AS varchar(20)) AS PayForType,
                            J.JournalDate AS DocDate,
                            JD.Description,
                            JD.Credit AS OrAmt
                            FROM ARMatched ARM
                            JOIN Journals J
                            ON J.JournalCode = ARM.ARCode
                            JOIN JournalDetails JD
                            ON JD.JournalId = J.Id
                            WHERE
                            PayForType = 'INV'
                            AND Amount <> 0
                            AND JD.Pos = ARM.OptimisticLockField
                            AND ARM.PayForCode = @INVOICECODE";

            for (int i = 0; i < InvoiceForMatching.Count; i++)
            {
                var INV = InvoiceForMatching[i];
                string INVOICECODE = InvoiceForMatching[i].INVOICECODE;
                var param = new DynamicParameters();
                param.Add("@INVOICECODE", INVOICECODE);
                var reader = con.ExecuteReader(SQL_INVOICEMATCH_QUERY, param);

                while (reader.Read())
                {
                    INV.matchedDetailList.Add(new MATCHEDDETAILS()
                    {
                        DOCDATE = !String.IsNullOrEmpty(reader["DOCDATE"].ToString().Trim()) ? Convert.ToDateTime(reader["DOCDATE"].ToString().Trim()) : (DateTime?)null,
                        DOCNO = reader["DOCNO"].ToString().Trim(),
                        AMOUNT = Convert.ToDouble(reader["AMOUNT"].ToString().Trim())
                    });
                }

                double matchedAmountTotal = 0;
                if (INV.matchedDetailList.Count == 0)
                {


                    DateTime invoiceDate = Convert.ToDateTime(INV.INVOICEDATE);
                    DateTime dateToday = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    var days = Convert.ToInt32(dateToday.Subtract(invoiceDate).TotalDays);


                    INV.AGING = days;
                }
                else
                {
                    foreach (var m in INV.matchedDetailList)
                    {
                        DateTime invoiceDate = Convert.ToDateTime(INV.INVOICEDATE);
                        DateTime matchedDate = Convert.ToDateTime(m.DOCDATE);

                        var days = Convert.ToInt32(matchedDate.Subtract(invoiceDate).TotalDays);


                        m.AGING = days;
                        matchedAmountTotal += m.AMOUNT;
                    }

                }
                INV.BALANCE = INV.LOCALTOTALAMOUNT - matchedAmountTotal;

            }
                return InvoiceMatchListing;
        }

        public async Task<List<DEPARTMENTLEDGERRESULT>> GenerateDepartmentLedger(DEPARTMENTLEDGERFILTER filter)
        {
            using IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode));
            List<DEPARTMENTLEDGERRESULT> result = new List<DEPARTMENTLEDGERRESULT>();
            if (filter.DepartmentAccountCodes.Count > 0)
            {
                if (filter.GLAccountCodes.Count > 0)
                {
                    string sql = @"SELECT C.AccountCode,A.TransactionDate,A.GLAccountCode,A.GLAccountName,A.DocumentCode,A.Description,A.Balance,A.ReferenceNo,A.JournalType FROM VW_GLTransactions A
                        INNER JOIN GLAccounts B
                        ON A.GLAccountCode = B.GLAccountCode
                        INNER JOIN Accounts C
                        ON C.Id = B.AccountId
                        WHERE A.GLAccountCode IN @gls";
                    string[] GLAccountCodes = filter.GLAccountCodes.ToArray();
                    var res = await con.QueryAsync<DEPARTMENTLEDGERRESULT>(sql, new { gls = GLAccountCodes });
                    return res.ToList();
                }
                else
                {
                    string sql = @"SELECT C.AccountCode,A.TransactionDate,A.GLAccountCode,A.GLAccountName,A.DocumentCode,A.Description,A.Balance,A.ReferenceNo,A.JournalType FROM VW_GLTransactions A
                        INNER JOIN GLAccounts B
                        ON A.GLAccountCode = B.GLAccountCode
                        INNER JOIN Accounts C
                        ON C.Id = B.AccountId
                        WHERE C.AccountCode IN @gls";
                    string[] DepartmentAccountCodes = filter.DepartmentAccountCodes.ToArray();
                    var res = await con.QueryAsync<DEPARTMENTLEDGERRESULT>(sql, new { gls = DepartmentAccountCodes });
                    return res.ToList();
                }
            }
            return result;
        }

        //public async Task<IEnumerable<STOCKLEDGERWITHBATCHRESULT>> GenerateStockLedgerWBatch(STOCKLEDGERWITHBATCHFILTER filter)
        //{
        //    using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.CompanyCode)))
        //    {
        //        string sql = @"SELECT A.*,B.BatchNoExpiryDate,B.BatchNoManufacturingDate FROM dbo.FN_StockLedgerWithBatch('2010-01-01','2021-12-12',0,0,0,null,'--ALL--') AS A                          
        //                      JOIN StockBatchNumbers AS B
        //                      ON A.StockBatchNumberId = B.Id";
        //        var res = await con.QueryAsync<STOCKLEDGERWITHBATCHRESULT>(sql);
        //        return res;
        //    }
        //}
    }
}
