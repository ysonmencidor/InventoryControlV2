using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.TestModel
{
    public class TestService
    {
        private readonly ISqlDataAccess dataAccess;

        public TestService(ISqlDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        public async Task<IEnumerable<Employee>> ShowEmployee(string companyCode)
        {
            var sqlcon = QNEConnectionString.ChooseConnection(companyCode);
            using (IDbConnection con = new SqlConnection(sqlcon))
            {
                var sql = "SELECT * FROM Employee";
                var model = await con.QueryAsync<Employee>(sql, commandType: CommandType.Text);
                return model;
            }
        }
    }
}
