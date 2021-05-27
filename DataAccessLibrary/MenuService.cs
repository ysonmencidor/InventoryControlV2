using Dapper;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class MenuService
    {
        private readonly ISqlDataAccess db;

        public MenuService(ISqlDataAccess _db)
        {
            db = _db;
        }

        public async Task<IEnumerable<MenuInfo>> GetMenuData()
        {
            const string query = @"SELECT * FROM Menu";
            return await db.LoadData<MenuInfo, dynamic>(query, new { });
        }

        public async Task<IEnumerable<MenuInfo>> GetMenuCustomByManageUserId(int UserId)
        {
            string query = @"SELECT * FROM Menu WHERE ParentMenuId > 0 AND  MenuId NOT IN(SELECT MenuId from CustomAccess WHERE UserId = @UserId)";
            return await db.LoadData<MenuInfo, dynamic>(query, new { UserId });
        }

        public async Task<IEnumerable<MenuInfo>> GetAssignedMenuCustom(int UserId)
        {
            string query = @"SELECT * FROM Menu WHERE MenuId IN(SELECT MenuId from CustomAccess WHERE UserId = @UserId)";
            return await db.LoadData<MenuInfo, dynamic>(query, new { UserId });
        }

        public async Task<IEnumerable<CustomAccess>> GetMenuCustomByUserId(int UserId)
        {
            string query = @"SELECT * from CustomAccess WHERE UserId = @UserId";
            return await db.LoadData<CustomAccess, dynamic>(query, new { UserId });
        }

        public async Task<IEnumerable<MenuInfo>> GetMenuData(int Id)
        {
            string query = @"SELECT A.* FROM Menu AS A
                            JOIN GroupedMenu AS B
                            ON A.MenuId = B.MenuId
                            JOIN MenuAccess AS C
                            ON C.GroupId = B.GroupId
                            JOIN UserAccount AS D
                            ON D.Id = C.UserId
                            WHERE D.Id = @Id";
            var data = await db.LoadData<MenuInfo, dynamic>(query, new { Id });
            List<MenuInfo> result = new List<MenuInfo>();


            foreach(var item in data)
            {
                string querySub = @"SELECT * FROM Menu WHERE ParentMenuId = @MenuId";
                var p = new DynamicParameters();
                p.Add("@MenuId", item.MenuId);
                var subResult = await db.LoadData<MenuInfo, dynamic>(querySub, p);

                result.Add(item);
                foreach (var itemSub in subResult)
                {
                    result.Add(itemSub);
                }            
            }

            return result;
        }

        public async Task<MenuInfo> GetMenuDataById(int MenuId)
        {
            const string query = @"SELECT * FROM Menu WHERE MenuId = @MenuId";
            return await db.LoadSingleData<MenuInfo>(query, new { MenuId });
        }
        public async Task<IEnumerable<Company>> GetCompanies()
        {
            const string query = @"SELECT * FROM Company";
            return await db.LoadData<Company, dynamic>(query, new { });
        }

        public async Task<IEnumerable<Group>> GetGroups()
        {
            const string query = @"SELECT * FROM GroupMenu order by Name asc";
            return await db.LoadData<Group, dynamic>(query, new { });
        }

        public async Task<int> SaveGroup(Group group)
        {
            string query;
            var p = new DynamicParameters();
            if (group.GroupId > 0)
            {
                query = "UPDATE GroupMenu SET NAME = @Name,IsActive = @IsActive WHERE GroupId = @groupId";
                p.Add("@groupId", group.GroupId);
            }
            else
            {
                query = "INSERT INTO GroupMenu VALUES(@Name,@IsActive)";
            }
            p.Add("@Name", group.Name);
            p.Add("@IsActive", group.IsActive);
            return await db.SaveData(query, p);
        }

        public async Task<Group> GetGroupMenuById(int GroupId)
        {
            const string query = @"SELECT * FROM GroupMenu WHERE GroupId = @GroupId";
            return await db.LoadSingleData<Group>(query, new { GroupId });
        }

        public async Task<int> AssignMenu(GroupedMenu groupedMenu)
        {
            string query = @"INSERT INTO GroupedMenu OUTPUT inserted.GroupedMenuId VALUES (@GroupId,@MenuId)";
            var p = new DynamicParameters();
            p.Add("@GroupId", groupedMenu.GroupId);
            p.Add("@MenuId", groupedMenu.MenuId);
            return await db.SaveDataReturnId(query, p);
        }

        public async Task<int> RemoveAssignedMenu(int GroupedMenuId)
        {
            string query = @"DELETE FROM GroupedMenu where GroupedMenuId = @GroupedMenuId";
            return await db.SaveData(query, new { GroupedMenuId });
        }

        public async Task<IEnumerable<AssignedMenu>> GetAssignedMenu(int GroupId)
        {
            string query = @"SELECT A.GroupedMenuId,B.MenuName,B.MenuId FROM GroupedMenu AS A
                            JOIN Menu AS B
                            ON A.MenuId = B.MenuId
                            WHERE B.ParentMenuId = 0 AND A.GroupId = @GroupId";
            return await db.LoadData<AssignedMenu, dynamic>(query, new { GroupId });
        }

        public async Task<IEnumerable<UsersInGroup>> CountUsersInGroup()
        {
            string query = @"SELECT GroupId,COUNT(DISTINCT UserId) AS Users
                            FROM MenuAccess 
                            GROUP BY GroupId";
            return await db.LoadData<UsersInGroup, dynamic>(query, new { });
        }
       public async Task<IEnumerable<MenuAccess>> GetUsersOnGroup(int GroupId)
        {
            string query = @"SELECT A.*,B.Name FROM MenuAccess AS A
                            LEFT JOIN UserAccount AS B
                            ON A.UserId = b.Id
                            WHERE A.GroupId = @GroupId";
            return await db.LoadData<MenuAccess, dynamic>(query, new { GroupId });
        }

        public async Task<int> InsertUserToGroup(MenuAccess menuAccess)
        {
            string query = @"INSERT INTO MenuAccess OUTPUT inserted.MenuAccessId VALUES (@GroupId,@UserId)";
            return await db.SaveDataReturnId(query, new { menuAccess.GroupId, menuAccess.UserId });
        }

        public async Task<int> DeleteUserToGroup(MenuAccess menuAccess)
        {
            string query = @"DELETE FROM MenuAccess WHERE MenuAccessId = @MenuAccessId";
            return await db.SaveData(query, new { menuAccess.MenuAccessId });
        }

        public async Task<int> AssignedCustomMenu(CustomAccess customAccess)
        {
            string query = @"INSERT INTO CustomAccess OUTPUT inserted.CustomId VALUES (@UserId,@MenuId)";
            return await db.SaveDataReturnId(query, new { customAccess.UserId , customAccess.MenuId });
        }

        public async Task<int> RemoveAssignedCustom(CustomAccess customAccess)
        {
            string query = @"DELETE FROM CustomAccess WHERE CustomId = @CustomId";
            return await db.SaveData(query, new { customAccess.CustomId });
        }
       
    }
}
