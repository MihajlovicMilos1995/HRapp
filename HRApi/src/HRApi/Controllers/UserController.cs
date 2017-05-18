using HRApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using HRApi.Enums;

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
        public List<RegUser> GetUser()
        {
            return _ctx.RegUsers.ToList();
        }

        [Authorize(Roles = "SuperUser,HrManager")]
        [HttpGet("ListUserByRole")]
        public List<RegUser> ListUserByRole([FromQuery] string role)
        {
            var users = _ctx.Users.Where
                (x => x.Roles.Select
                (y => y.RoleId)
                .Contains(role)).ToList();

            return users;
        }

        [Authorize(Roles = "SuperUser,HrManager")]
        [HttpGet("ListUserByKeyword")]
        public List<RegUser> ListUserByKeyword([FromQuery] string keyword)
        {
            var user = _ctx.RegUsers.Where
                (u => u.RegUserKeyword
                .Contains(keyword)).ToList();

            return user;
        }

        [HttpGet("GetUsersInCompanyArea/{companyName}")]
        [Authorize(Roles = ("SuperUser,HrManager"))]
        public List<RegUser> GetUsersInCompanyArea([FromQuery] string companyName)
        {
            var comp = _ctx.Companies.Find(companyName);

            string companyArea = comp.CompanyCity;

            var user = _ctx.RegUsers.Where
                (u => u.RegUserCity.Contains(companyArea))
                .ToList();

            return user;
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
                todo.RegUserSex = regUser.RegUserSex;
                todo.RegUserDoB = regUser.RegUserDoB;
                todo.RegUserLastName = regUser.RegUserLastName;
                todo.RegUserCity = regUser.RegUserCity;
                todo.RegUserCountry = regUser.RegUserCountry;
                todo.LocationChange = regUser.LocationChange;
                todo.RegUserPartFull = regUser.RegUserPartFull;
                todo.RegUserKeyword = regUser.RegUserKeyword;
                todo.StatusOfUser = regUser.StatusOfUser;
            }

            if (User.IsInRole("SuperUser")
                && User.IsInRole("HrManager"))
            {
                todo.RegUserAdditionalInfo = regUser.RegUserAdditionalInfo;
            }

            _ctx.SaveChanges();

            return Ok("Edited");
        }

        [Authorize(Roles = "SuperUser, HrManager")]
        [HttpDelete("DeleteUser/{userName}")]
        public IActionResult DeleteUser([FromQuery]string userName)
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

        [HttpGet("status/{status}")]
        public List<RegUser> GetJobsByStatus(Status status)
        {

            var users = _ctx.Users
                   .Where(c => c.StatusOfUser == status);

                return users.ToList();


        }
    }
}