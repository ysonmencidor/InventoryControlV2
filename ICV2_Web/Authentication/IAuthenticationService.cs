using DataAccessLibrary.Models;
using ICV2_Web.Models;
using System.Threading.Tasks;

namespace ICV2_Web.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> Login(UserModel userForAuthentication);
        Task LogOut();
    }
}