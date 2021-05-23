using Dapper;
using DataAccessLibrary.Models.QneModels;
using DataAccessLibrary.Models.QneReportFilter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DataAccessLibrary.Models.QneServices
{
    public class QneDataService 
    {
        public async Task<IEnumerable<Stocks>> GetStocks(string companyCode)
        {
            var sqlcon = QNEConnectionString.ChooseConnection(companyCode);
            using (IDbConnection con = new SqlConnection(sqlcon))
            {
                var sql = "SELECT Id,StockCode FROM Stocks";
                var model = await con.QueryAsync<Stocks>(sql, commandType: CommandType.Text);
                return model;
            }
        }
        public async Task<IEnumerable<StockGroups>> GetStockGroup(string companyCode)
        {
            var sqlcon = QNEConnectionString.ChooseConnection(companyCode);
            using (IDbConnection con = new SqlConnection(sqlcon))
            {
                var sql = "SELECT * FROM StockGroups";
                var model = await con.QueryAsync<StockGroups>(sql, commandType: CommandType.Text);
                return model;
            }
        }
        public async Task<IEnumerable<StockLocations>> GetStockLocations(string companyCode)
        {
            var sqlcon = QNEConnectionString.ChooseConnection(companyCode);
            using (IDbConnection con = new SqlConnection(sqlcon))
            {
                var sql = "SELECT * FROM StockLocations";
                var model = await con.QueryAsync<StockLocations>(sql, commandType: CommandType.Text);
                return model;
            }
        }
        public async Task<IEnumerable<Debtors>> GetDebtors(string companyCode)
        {
            var sqlcon = QNEConnectionString.ChooseConnection(companyCode);
            using (IDbConnection con = new SqlConnection(sqlcon))
            {
                var sql = "SELECT Id,CompanyName FROM Debtors ORDER BY CompanyName";
                var model = await con.QueryAsync<Debtors>(sql, commandType: CommandType.Text);
                return model;
            }
        }
        public async Task<IEnumerable<Areas>> GetAreas(string companyCode)
        {
            var sqlcon = QNEConnectionString.ChooseConnection(companyCode);
            using (IDbConnection con = new SqlConnection(sqlcon))
            {
                var sql = "SELECT Id,Description FROM Areas ORDER BY Description";
                var model = await con.QueryAsync<Areas>(sql, commandType: CommandType.Text);
                return model;
            }
        }
        public async Task<IEnumerable<SalesPersons>> GetSalesPersons(string companyCode)
        {
            var sqlcon = QNEConnectionString.ChooseConnection(companyCode);
            using (IDbConnection con = new SqlConnection(sqlcon))
            {
                var sql = "SELECT Id,Name,StaffCode FROM SalesPersons ORDER BY StaffCode";
                var model = await con.QueryAsync<SalesPersons>(sql, commandType: CommandType.Text);
                return model;
            }
        }
        public async Task<IEnumerable<DebtorCategory>> GetDebtorCategory(string companyCode)
        {
            var sqlcon = QNEConnectionString.ChooseConnection(companyCode);
            using (IDbConnection con = new SqlConnection(sqlcon))
            {
                var sql = "SELECT Id,Description FROM DebtorCategory ORDER BY Description";
                var model = await con.QueryAsync<DebtorCategory>(sql, commandType: CommandType.Text);
                return model;
            }
        }
        public async Task<IEnumerable<FGMASTERFILEMODEL>> GetFGMasterFile(string companyCode)
        {
            var sqlcon = QNEConnectionString.ChooseConnection(companyCode);
            using (IDbConnection con = new SqlConnection(sqlcon))
            {
                var sql = @"SELECT B.Id AS CATID,A.StockCode,A.StockName,
                            CONVERT(BIT, (CASE WHEN B.SWINE IS NULL OR B.SWINE = 0 THEN 0 ELSE 1 END)) AS SWINE,
                            CONVERT(BIT, (CASE WHEN B.COMMON IS NULL OR B.COMMON = 0 THEN 0 ELSE 1 END)) AS COMMON,
                            CONVERT(BIT, (CASE WHEN B.POULTRY IS NULL OR B.POULTRY = 0 THEN 0 ELSE 1 END)) AS POULTRY
                            FROM Stocks AS A LEFT JOIN CATEGORIZED_PRODUCTS AS B
                            ON A.StockCode = B.STOCKCODE";
                var model = await con.QueryAsync<FGMASTERFILEMODEL>(sql, commandType: CommandType.Text);
                return model;
            }
        }
        public async Task<FGMASTERFILEMODEL> GetFGByCode(string companyCode,string stockCode)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(companyCode)))
            {
                var sql = @"SELECT B.Id AS CATID,A.StockCode,A.StockName,
                            CONVERT(BIT, (CASE WHEN B.SWINE IS NULL OR B.SWINE = 0 THEN 0 ELSE 1 END)) AS SWINE,
                            CONVERT(BIT, (CASE WHEN B.COMMON IS NULL OR B.COMMON = 0 THEN 0 ELSE 1 END)) AS COMMON,
                            CONVERT(BIT, (CASE WHEN B.POULTRY IS NULL OR B.POULTRY = 0 THEN 0 ELSE 1 END)) AS POULTRY
                            FROM Stocks AS A LEFT JOIN CATEGORIZED_PRODUCTS AS B
                            ON A.StockCode = B.STOCKCODE WHERE A.STOCKCODE = @STCODE";
                DynamicParameters p = new DynamicParameters();
                p.Add("@STCODE", stockCode);
                var model = await con.QuerySingleAsync<FGMASTERFILEMODEL>(sql , p, commandType: CommandType.Text);
                return model;           
            }
        }
        public async Task<int> InsertUpdateFGMASTERFILE(FGMASTERFILEFILTER filter)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(filter.companyCode)))
            {
                string sql = "";
                var p = new DynamicParameters();
                p.Add("@STCODE", filter.StockCode);
                if (filter.CATID != null || filter.CATID > 0)
                {
                    switch (filter.Category)
                    {
                        case 'S':
                            sql = "UPDATE CATEGORIZED_PRODUCTS SET SWINE = @SWINE WHERE STOCKCODE = @STCODE";
                            p.Add("@SWINE", filter.IsChecked);
                            break;
                        case 'C':
                            sql = "UPDATE CATEGORIZED_PRODUCTS SET COMMON = @COMMON WHERE STOCKCODE = @STCODE";
                            p.Add("@COMMON", filter.IsChecked);
                            break;
                        case 'P':
                            sql = "UPDATE CATEGORIZED_PRODUCTS SET POULTRY = @POULTRY WHERE STOCKCODE = @STCODE";
                            p.Add("@POULTRY", filter.IsChecked);
                            break;
                    }
                    return await con.ExecuteAsync(sql, p, commandType: CommandType.Text);
                }
                else
                {
                    sql = @"INSERT INTO CATEGORIZED_PRODUCTS (STOCKCODE,SWINE,POULTRY,COMMON) VALUES (@STCODE,@SWINE,@POULTRY,@COMMON)";
                    switch (filter.Category)
                    {
                        case 'S':
                            p.Add("@SWINE", "1");
                            p.Add("@POULTRY", "0");
                            p.Add("@COMMON", "0");
                            break;
                        case 'C':
                            p.Add("@SWINE", "0");
                            p.Add("@POULTRY", "0");
                            p.Add("@COMMON", "1");
                            break;
                        case 'P':
                            p.Add("@SWINE", "0");
                            p.Add("@POULTRY", "1");
                            p.Add("@COMMON", "0");
                            break;
                    }
                }

                return await con.ExecuteAsync(sql, p, commandType: CommandType.Text);
                //sql = @"UPDATE CATEGORIZED_PRODUCTS SET {0}=@Status WHERE STOCKCODE = @StockCode
                //               IF @@ROWCOUNT=0
                //               INSERT INTO CATEGORIZED_PRODUCTS (STOCKCODE) VALUES(@StockCode);";

            }
        }
        public async Task<IEnumerable<StockIssue>> GetStockIssuesAsync(string companyCode)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(companyCode)))
            {
                const string sql = @"SELECT Id,StockOutCode FROM StockOuts ORDER BY StockOutCode DESC";
                return await con.QueryAsync<StockIssue>(sql, commandType:CommandType.Text);
            }
        }
        public async Task<IEnumerable<stockbatchnumbers>> GetBatchNumByStockId(string companyCode, Guid StockId)
        {
            using (IDbConnection con = new SqlConnection(QNEConnectionString.ChooseConnection(companyCode)))
            {
                const string sql = @"SELECT Id,StockId,BatchNo FROM StockBatchNumbers WHERE StockId = @StockId";
                return await con.QueryAsync<stockbatchnumbers>(sql, new { StockId }, commandType: CommandType.Text);
            }
        }

    }
}
