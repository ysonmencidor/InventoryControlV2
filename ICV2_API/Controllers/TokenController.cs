using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ICV2_API.Controllers
{
    [EnableCors("AllowCors")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUserData userData;

        public TokenController(IConfiguration configuration, IUserData userData)
        {
            _config = configuration;
            this.userData = userData;
        }

        [Route("Token")]
        [HttpPost]
        public async Task<IActionResult> Token(string username,string password)
        {
            if (await IsValidUsernameAndPassword(username, password))
            {
                return new ObjectResult(await GenerateToken(username, password));
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<bool> IsValidUsernameAndPassword(string username,string password)
        {
            var model = new UserModel {
                Username = username,
                Password = password
            };

            if (model != null) {
                var user = await userData.LoginUser(model);
                if (user == null)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<dynamic> GenerateToken(string username,string password)
        {
            var model = new UserModel
            {
                Username = username,
                Password = password
            };

            AuthenticatedUserModel AuthenticatedUser = await userData.LoginUser(model);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.Role, AuthenticatedUser.RoleName),
                new Claim(ClaimTypes.NameIdentifier, AuthenticatedUser.Id.ToString()),
                new Claim("AccessType", AuthenticatedUser.AccessType),
                new Claim("IsPasswordDefault", AuthenticatedUser.IsPasswordDefault.ToString())
            };
            var token = new JwtSecurityToken(
                            new JwtHeader(
                                new SigningCredentials(
                                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),SecurityAlgorithms.HmacSha256)),
                                        new JwtPayload(claims));

            var output = new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = AuthenticatedUser.username
            };

            return output;
        }

    }
}
