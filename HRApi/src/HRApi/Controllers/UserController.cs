using HRApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace HRApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private HRContext _ctx;
        private UserManager<RegUser> _userManager;
        // private RoleManager<RegUser> _roleManager;

        public UserController(HRContext ctx, UserManager<RegUser> UserManager)
        {
            _ctx = ctx;
            _userManager = UserManager;

            //_roleManager = RoleManager;
        }

        [Authorize(Roles = "SuperUser, HrManager")]
        [HttpGet("GetUser")]
        public IEnumerable<RegUser> GetUser()
        {
            return _ctx.RegUsers.ToList();
        }

        [HttpGet("ListUserByType")]
        public void ListUserByType(RegUser user)
        {

        }

        [Authorize(Roles = "SuperUser, HrManager,RegUser")]
        [HttpPut("EditUser/{userName}")]
        public IActionResult EditUser([FromBody] RegUser regUser, string userName)
        {
            if (regUser == null)
            {
                return BadRequest();
            }

            var todo = _ctx.RegUsers.FirstOrDefault(u => u.UserName == userName);
            if (todo == null)
            {
                return NotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                todo.UserName = regUser.UserName;
                todo.RegUserName = regUser.RegUserName;
                todo.RegUserLastName = regUser.RegUserLastName;
                todo.RegUserCity = regUser.RegUserCity;
                todo.RegUserCountry = regUser.RegUserCountry;
                todo.LocationChange = regUser.LocationChange;
                todo.RegUserPartFull = regUser.RegUserPartFull;
                todo.RegUserKeyword = regUser.RegUserKeyword;
            }
            _ctx.SaveChanges();

            return Ok("Edited");
        }

        [Authorize(Roles = "SuperUser, HrManager")]
        [HttpDelete("deleteUser/{userName}")]
        public IActionResult deleteUser(string userName)
        {
            var todo = _ctx.RegUsers.FirstOrDefault(u => u.UserName == userName);
            if (todo == null)
            {
                return NotFound();
            }
            _ctx.RegUsers.Remove(todo);
            _ctx.SaveChanges();

            return Ok("Deleted");
        }

        [HttpGet("SSP")]
        public IActionResult SearchAndSort([FromQuery]string searchString, [FromQuery] string sortBy, [FromQuery] int page, [FromQuery] int regUser = 3)
        {
            var User = from u in _ctx.RegUsers
                       select u;

            if (searchString != null)
            {
                User = User.Where(u => u.RegUserName.Contains(searchString)
                                        || u.RegUserKeyword.Contains(searchString));
            }

            if (sortBy == "Des")
            {
                User = User.OrderByDescending(u => u.RegUserName);
            }
            else if (sortBy == "Asc")
            {
                User = User.OrderBy(u => u.RegUserName);
            }

            if (page > 0)
            {
                User = User.Skip((page - 1) * regUser).Take(regUser);
            }

            return Ok(User);
        }
    }
}