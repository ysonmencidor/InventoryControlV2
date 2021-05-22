using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICV2_API.Controllers
{
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
            if(Result > 0)
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
        public async Task<IEnumerable<MenuAccess>> GetUsersOnGroup(int GroupId)
        {
            return await menu.GetUsersOnGroup(GroupId);
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
    }
}
