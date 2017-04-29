using HRApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        public UserController(HRContext ctx, UserManager<RegUser> UserManager)
        {
            _ctx = ctx;
            _userManager = UserManager;
        }

        [Authorize(Roles = "SuperUser, HrManager")]
        [HttpGet("GetUser")]
        public IEnumerable<RegUser> GetUser()
        {
            return _ctx.RegUsers.ToList();
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser([FromBody] RegUser User)
        {
            var user = await _userManager.CreateAsync(User, User.PasswordHash);
            return Ok(user);
        }

       


        [Authorize]
        [HttpPut("EditUser/{UserId}")]
        public IActionResult EditUser([FromBody] RegUser regUser, int UserId)
        {
            if (regUser == null)
            {
                return BadRequest();
            }

            var todo = _ctx.RegUsers.Find(regUser);
            if (todo == null)
            {
                return NotFound();
            }

            todo.RegUserLastName = regUser.RegUserLastName;
            todo.RegUserCity = regUser.RegUserCity;
            todo.RegUserCountry = regUser.RegUserCountry;
            todo.LocationChange = regUser.LocationChange;
            todo.RegUserPartFull = regUser.RegUserPartFull;
            todo.RegUserKeyword = regUser.RegUserKeyword;

            if (User.Identity.Name == "SuperUser")
            {
                todo.WorkXp = regUser.WorkXp;
            }

            _ctx.SaveChanges();

            return Ok();
        }

        [Authorize(Roles = "SuperUser, HrManager")]
        [HttpDelete("deleteUser/{UserId}")]
        public IActionResult deleteUser(int UserId)
        {
            var todo = _ctx.RegUsers.Find(UserId);
            if (todo == null)
            {
                return NotFound();
            }
            _ctx.RegUsers.Remove(todo);
            _ctx.SaveChanges();

            return Ok();
        }
    }
}