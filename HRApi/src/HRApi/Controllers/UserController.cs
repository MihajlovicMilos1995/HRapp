using HRApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace HRApi.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private HRContext _ctx;
        private UserManager<IdentityUser> _userManager;

        public UserController(HRContext ctx, UserManager<IdentityUser> UserManager)
        {
            _ctx = ctx;
            _userManager = UserManager;
        }

        //  [HttpPost("adduser")]
        //  public IActionResult AddUser()
        //  {
        //      var user = new IdentityUser()
        //      {
        //          UserName = "Ultradumb",
        //          Email = "milos@people.com"
        //      };
        //
        //      var id = _userManager.CreateAsync(user, "sifra").Result;
        //      return new NoContentResult();
        //  }

        [Authorize(Roles = "SuperUser,HrManager")]
        [HttpGet("GetUser")]
        public IEnumerable<RegUser> GetUser()
        {
            return _ctx.RegUsers.ToList();
        }


        [Authorize(Roles = "RegUser,SuperUser,HrManager")]
        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] RegUser regUser)
        {
            if (regUser == null)
            {
                return BadRequest();
            }
            _ctx.Add(regUser);
            _ctx.SaveChanges();

            return Created("api/usercontroller", regUser);
        }


        [Authorize(Roles = "RegUser,SuperUser,HrManager")]
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

            todo.RegUserName = regUser.RegUserName;
            todo.RegUserLastName = regUser.RegUserLastName;
            todo.RegUserCity = regUser.RegUserCity;
            todo.RegUserCountry = regUser.RegUserCountry;
            todo.LocationChange = regUser.LocationChange;
            todo.RegUserPartFull = regUser.RegUserPartFull;
            todo.RegUserKeyword = regUser.RegUserKeyword;

            if (User.Identity.Name == "SuperUser,HrManager")
            {
                todo.WorkXp = regUser.WorkXp;
            }

            _ctx.SaveChanges();

            return Ok();
        }


        [Authorize(Roles = "SuperUser,HrManager")]
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