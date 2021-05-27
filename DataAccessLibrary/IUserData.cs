using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IUserData
    {
        Task<List<UserModel>> GetUsers();
        Task<int> InsertUser(UserModel userModel);
        Task<AuthenticatedUserModel> LoginUser(UserModel userModel);
        Task<List<Roles>> ListOfRoles();
        Task<UserModel> GetUserById(int UserId);
        Task<int> UpdateUser(UserModel userModel);
        Task<List<UserModel>> GetUserByRole(string RoleName, string AccessType);
        Task<List<UserModel>> GetUserWithoutGroup();
        Task<int> ChangePassword(DefaultPass data);
        Task<int> ResetUserPass(string Username);
    }
}