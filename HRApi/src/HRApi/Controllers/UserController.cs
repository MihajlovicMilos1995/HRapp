using HRApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HRApi.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private HRContext _ctx;
        private UserManager<IdentityUser> _userManager;

        public UserController(HRContext ctx,UserManager<IdentityUser> UserManager)
        {
            _ctx = ctx;
            _userManager = UserManager;
        }

        [HttpPost("adduser")]
        public IActionResult AddUser()
        {
            var user = new IdentityUser()
            {
                UserName = "Ultradumb",
                Email = "milos@people.com"
            };

            var id = _userManager.CreateAsync(user, "sifra").Result;
            return new NoContentResult();
        }

        [HttpGet("GetUser")]
        public IEnumerable<RegUser> GetUser()
        {
            return _ctx.RegUsers.ToList();
        }

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
            todo.WorkXp = regUser.WorkXp;
            todo.RegUserKeyword = regUser.RegUserKeyword;

            _ctx.SaveChanges();

            return Ok();
        }

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