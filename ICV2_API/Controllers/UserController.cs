using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICV2_API.Controllers
{
    //[Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserData userData;

        public UserController(IUserData userData)
        {
            this.userData = userData;
        }

        [Route("changePFromDefault")]
        [HttpPost]
        public async Task<IActionResult> changePFromDefault(DefaultPass data)
        {
            int result = await userData.ChangePassword(data);
            if (result > 0)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [Route("SaveUserData")]
        [HttpPost]
        public async Task<IActionResult> SaveUserAsync(UserModel userModel)
        {
            if(userModel != null)
            {
                if (userModel.Id > 0)
                {
                    int UpdateCount = await userData.UpdateUser(userModel);
                    if (UpdateCount > 0)
                    {
                        return Ok(new { data = await GetUserById(userModel.Id), count = UpdateCount, IsUpdate = true });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {

                    var result = await userData.InsertUser(userModel);
                    if (result > 0)
                    {
                        //return Ok(result);
                        return Ok(new { data = await GetUserById(result), count = result, IsUpdate = false });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }

            }
            else
            {
                return NotFound();
            }

        
        }
        [Route("GetRoles")]
        [HttpGet]
        public async Task<List<Roles>> GetUserRoles()
        {
            return await userData.ListOfRoles();
        }

        [Route("GetUsers")]
        [HttpGet]
        public async Task<List<UserModel>> GetUsers()
        {
            return await userData.GetUsers();
        }
        [Route("GetUsersByRole")]
        [HttpGet]
        public async Task<List<UserModel>> GetUsersByRole(string RoleName,string Type)
        {
            return await userData.GetUserByRole(RoleName,Type);
        }
        [Route("GetUserWithoutGroup")]
        [HttpGet]
        public async Task<List<UserModel>> GetUserWithoutGroup()
        {
            return await userData.GetUserWithoutGroup();
        }

        [Route("GetUserById/{UserId}")]
        [HttpGet]
        public async Task<UserModel> GetUserById(int UserId)
        {
            return await userData.GetUserById(UserId);
        }

    }
}
