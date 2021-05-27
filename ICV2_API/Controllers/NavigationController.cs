using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICV2_API.Controllers
{
    [EnableCors("AllowCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationController : ControllerBase
    {
        private readonly MenuService menu;

        public NavigationController(MenuService _menu)
        {
            menu = _menu;
        }

        [Route("GetUserGroupedMenu")]
        [HttpGet]
        public async Task<IEnumerable<MenuInfo>> GetUserGroupedMenu(int UserId)
        {
            return await menu.GetMenuData(UserId);
        }

        [Route("GetNavMenu")]
        [HttpGet]
        public async Task<IEnumerable<MenuInfo>> GetNavMenuAsync()
        {
            return await menu.GetMenuData();
        }
        [Route("GetCompany")]
        [HttpGet]
        public async Task<IEnumerable<Company>> GetCompanyAsync()
        {
            return await menu.GetCompanies();
        }

        [Route("GetGroups")]
        [HttpGet]
        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            return await menu.GetGroups();
        }
        [Route("SaveGroup")]
        [HttpPost]
        public async Task<IActionResult> SaveGroupAsync(Group group)
        {
            int Result = await menu.SaveGroup(group);
            if (Result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("GetGroupById")]
        [HttpGet]
        public async Task<Group> GetGroupById(int id)
        {
            var model = await menu.GetGroupMenuById(id);
            if (model != null)
            {
                return model;
            }
            return new Group();
        }

        [Route("GetAssignedMenuByGroupId")]
        [HttpGet]
        public async Task<IEnumerable<AssignedMenu>> GetAssignedMenuByGroupId(int GroupedId)
        {
            return await menu.GetAssignedMenu(GroupedId);
        }

        [Route("AssignMenu")]
        [HttpPost]
        public async Task<IActionResult> AssignMenu(GroupedMenu groupedMenu)
        {
            int Result = await menu.AssignMenu(groupedMenu);
            if (Result > 0)
            {
                return Ok(Result);
            }
            return BadRequest();
        }
        [Route("UnAssigMenu")]
        [HttpPost]
        public async Task<IActionResult> UnAssignMenu(AssignedMenu assignedMenu)
        {
            int result = await menu.RemoveAssignedMenu(assignedMenu.GroupedMenuId);
            if (result > 0)
            {
                return Ok(await menu.GetMenuDataById(assignedMenu.MenuId));
            }
            return BadRequest();
        }

        [Route("GetUsersOnGroup")]
        [HttpGet]
        public async Task<IActionResult> GetUsersOnGroup(int GroupId)
        {
            var model = await menu.GetUsersOnGroup(GroupId);
            if (model != null)
            {
                return Ok(model);
            }
            return BadRequest();
        }
        [Route("InserUserToGroup")]
        [HttpPost]
        public async Task<IActionResult> InserUserToGroup(MenuAccess access)
        {
            int Result = await menu.InsertUserToGroup(access);
            if (Result > 0)
            {
                return Ok(Result);
            }
            return BadRequest();
        }

        [Route("DeleteUserToGroup")]
        [HttpPost]
        public async Task<IActionResult> DeleteUserToGroup(MenuAccess access)
        {
            int Result = await menu.DeleteUserToGroup(access);
            if (Result > 0)
            {
                return Ok(Result);
            }
            return BadRequest();
        }

        [Route("CountUsersInGroup")]
        [HttpGet]
        public async Task<IActionResult> CountUsersInGroup()
        {
            var model = await menu.CountUsersInGroup();
            if (model != null)
            {
                return Ok(model);
            }
            return BadRequest();
        }

        [Route("GetMenuCustom")]
        [HttpGet]
        public async Task<IActionResult> GetMenuCustom(int UserId)
        {
            var model = await menu.GetMenuCustomByManageUserId(UserId);
            if (model != null)
            {
                return Ok(model);
            }
            return BadRequest();
        }
        [Route("GetAssignedMenuCustomById")]
        [HttpGet]
        public async Task<IActionResult> GetAssignedMenuCustomById(int UserId)
        {
            var model = await menu.GetAssignedMenuCustom(UserId);
            if (model != null)
            {
                return Ok(model);
            }
            return BadRequest();
        }

        [Route("GetMenuCustomByUserId")]
        [HttpGet]
        public async Task<IActionResult> GetMenuCustomById(int UserId)
        {
            var model = await menu.GetMenuCustomByUserId(UserId);
            if (model != null)
            {
                return Ok(model);
            }
            return BadRequest();
        }

        [Route("AssignedCustomMenu")]
        [HttpPost]
        public async Task<IActionResult> AssignedCustomMenu(CustomAccess customAccess)
        {
            int Result = await menu.AssignedCustomMenu(customAccess);
            if(Result > 0)
            {
                return Ok(Result);
            }
            return BadRequest();
        }

        [Route("RemoveCustomMenu")]
        [HttpPost]
        public async Task<IActionResult> RemoveCustomMenu(CustomAccess customAccess)
        {
            int Result = await menu.RemoveAssignedCustom(customAccess);
            if (Result > 0)
            {
                return Ok(Result);
            }
            return BadRequest();
        }
    }
}
