using HRApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace HRApi.Controllers
{
    [Route("api/controller]")]
    public class UserController : Controller
    {
        private HRContext _ctx;

        public UserController(HRContext ctx)
        {
            _ctx = ctx;
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