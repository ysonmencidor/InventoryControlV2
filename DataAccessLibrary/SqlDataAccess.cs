using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly AppConnectionString appConnectionString;

        public SqlDataAccess(AppConnectionString appConnectionString)
        {
            this.appConnectionString = appConnectionString;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            using IDbConnection connection = new SqlConnection(appConnectionString.Value);
            var data = await connection.QueryAsync<T>(sql, parameters);
            return data.ToList();
        }

        public async Task<T> LoadSingleData<T>(string sql, object parameters = null)
        {
            using IDbConnection connection = new SqlConnection(appConnectionString.Value);
            var data = await connection.QuerySingleAsync<T>(sql, parameters);
            return data;
        }

        public async Task<int> SaveData<T>(string sql, T parameters)
        {
            using IDbConnection connection = new SqlConnection(appConnectionString.Value);
            return await connection.ExecuteAsync(sql, parameters);
        }

        public async Task SpSaveData<T>(string sql, T parameters)
        {
            using IDbConnection connection = new SqlConnection(appConnectionString.Value);
            await connection.ExecuteAsync(sql, parameters, commandType:CommandType.StoredProcedure);
        }

        public async Task<int> SaveDataReturnId<T>(string sql, T parameters)
        {
            using IDbConnection connection = new SqlConnection(appConnectionString.Value);
            var result = await connection.ExecuteScalarAsync(sql, parameters);
            int NewId;
            if (result != null)
            {
                NewId = Convert.ToInt32(result);
            }
            else
            {
                NewId = 0;
            }
            return NewId;
        }

    }
}
