using Dapper;
using DataAccessLibrary.Models;
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
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess db;
        private readonly AppConnectionString appConnectionString;

        public UserData(ISqlDataAccess db, AppConnectionString appConnectionString)
        {
            this.db = db;
            this.appConnectionString = appConnectionString;
        }

        public async Task<List<UserModel>> GetUsers()
        {
            const string sql = @"SELECT UserAccount.*,Roles.* FROM UserAccount JOIN UserRoles on UserAccount.Id = UserRoles.UserId JOIN Roles on UserRoles.RoleId = Roles.RoleId";

            return await db.LoadData<UserModel, dynamic>(sql, new { });
        }

        public async Task<List<UserModel>> GetUserByRole(string RoleName, string AccessType)
        {
            const string sql = @"SELECT UserAccount.*,Roles.* FROM UserAccount JOIN UserRoles on UserAccount.Id = UserRoles.UserId JOIN Roles on UserRoles.RoleId = Roles.RoleId
                                WHERE Roles.RoleName = @RoleName AND AccessType = @AccessType";

            return await db.LoadData<UserModel, dynamic>(sql, new { RoleName, AccessType });
        }

        public async Task<List<UserModel>> GetUserWithoutGroup()
        {
            const string sql = @"SELECT UserAccount.*,Roles.* FROM UserAccount JOIN UserRoles on UserAccount.Id = UserRoles.UserId JOIN Roles on UserRoles.RoleId = Roles.RoleId
                                WHERE Roles.RoleName = 'User' AND AccessType = 'Grouped' AND UserAccount.Id NOT IN (SELECT UserId FROM MenuAccess)";
            return await db.LoadData<UserModel, dynamic>(sql, new { });
        }

        public async Task<UserModel> GetUserById(int UserId)
        {
            const string sql = @"SELECT UserAccount.*,Roles.* FROM UserAccount JOIN UserRoles on UserAccount.Id = UserRoles.UserId JOIN Roles on UserRoles.RoleId = Roles.RoleId WHERE UserAccount.Id = @UserId";
            return await db.LoadSingleData<UserModel>(sql, new { UserId });
        }
        public async Task<int> InsertUser(UserModel userModel)
        {
            using (IDbConnection con = new SqlConnection(appConnectionString.Value))
            {
                //con.Open();
                //using (var transaction = con.BeginTransaction())
                //{
                //}
                //string sql = $@"INSERT INTO [dbo].[UserAccount]
                //     ([Username]
                //     ,[Password]
                //     ,[Firstname]
                //     ,[Middlename]
                //     ,[Lastname])
                //      VALUES
                //     (@Username
                //     ,@Password
                //     ,@Firstname
                //     ,@Middlename
                //     ,@Lastname); 
                //     select @Id = @@IDENTITY
                //     INSERT INTO UserRoles values (@Id,@RoleId)";

                //  string test = $@"BEGIN TRANSACTION
                //      INSERT INTO UserAccount
                //             (Username,Password,Firstname,Middlename,Lastname)
                //              VALUES
                //             (@Username,@Password,@Firstname,@Middlename,@Lastname)
                //      DECLARE @UserId INT
                //      SET @UserId = SCOPE_IDENTITY() --last assigned id
                //      INSERT INTO UserRoles values (@UserId,@RoleId)
                //      COMMIT";

                var p = new DynamicParameters();
                p.Add("@UserId", 0, DbType.Int32, ParameterDirection.Output);
                p.Add("@Username", userModel.Username);
                p.Add("@Password", userModel.Password);
                p.Add("@Name", userModel.Name);
                p.Add("@AceessType", userModel.AccessType);
                p.Add("@IsActive", userModel.IsActive);
                p.Add("@RoleId", userModel.RoleId);

                //transaction.Commit();

                await con.ExecuteAsync("SP_SaveNewUser", p, commandType: CommandType.StoredProcedure);
                //await con.QueryAsync<int>(test, p, transaction: transaction); 
                int newID = p.Get<int>("@UserId");
                return newID;
            }


            //await db.SaveData(sql, p);

            //int newIdentity = p.Get<int>("@Id");

            //const string sqlInsertUserRole = "INSERT INTO UserRoles values (@UserId,@RoleId)";
            //await db.SaveData(sqlInsertUserRole, new { @UserId = newIdentity, @RoleId = userModel.RoleId });

        }

        public async Task<int> UpdateUser(UserModel userModel)
        {
            using (IDbConnection con = new SqlConnection(appConnectionString.Value))
            {
                const string SpUpdate = "SP_UpdateUser";
                var p = new DynamicParameters();
                p.Add("@UserId", userModel.Id);
                p.Add("@Username", userModel.Username);
                p.Add("@Name", userModel.Name); ;
                p.Add("@AceessType", userModel.AccessType);
                p.Add("@RoleId", userModel.RoleId);
                p.Add("@IsACtive", userModel.IsActive);

                int affectedRows = await con.ExecuteAsync(SpUpdate, p, commandType: CommandType.StoredProcedure);
                return affectedRows;
            }
        }

        public async Task<AuthenticatedUserModel> LoginUser(UserModel userModel)
        {
            string sql = @"select username,RoleName,UserAccount.Id,UserAccount.AccessType,UserAccount.IsPasswordDefault from UserAccount 
                            join UserRoles on UserAccount.Id = UserRoles.UserId
                            join Roles on UserRoles.RoleId = Roles.RoleId 
                            where IsActive = 'True' and UserAccount.Username = @Username and UserAccount.Password = @Password";
            var model = await db.LoadSingleData<AuthenticatedUserModel>(sql, new { userModel.Username, userModel.Password });

            if(model != null)
            {
                return model;
            }
            return null;
        }

        public async Task<int> ChangePassword(DefaultPass data)
        {
            string sql = @"UPDATE UserAccount SET Password = @Pass,IsPasswordDefault = @Default WHERE Username = @Uname";
            var p = new DynamicParameters();
            p.Add("@Pass", data.RepeatPassword);
            p.Add("@Default", "False");
            p.Add("@Uname", data.Username);
            int affectedRows = await db.SaveData(sql, p);
            return affectedRows;
        }

        public Task<List<Roles>> ListOfRoles()
        {
            string sql = @"select RoleId,RoleName from Roles";
            return db.LoadData<Roles, dynamic>(sql, new { });
        }

        public async Task<int> ResetUserPass(string Username)
        {
            string sql = @"UPDATE UserAccount SET Password = @Pass ,IsPasswordDefault = @Default WHERE Username = @Uname";
            var p = new DynamicParameters();
            p.Add("@Pass", "Defaultpassword");
            p.Add("@Default", "True");
            p.Add("@Uname", Username);
            int affectedRows = await db.SaveData(sql, p);
            return affectedRows;
        }
    }
}
