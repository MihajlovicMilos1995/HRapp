using HRApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using HRApi.Enums;
using AutoMapper;
using HRApi.Models.UserDTO;
using System.Collections;
using AutoMapper.XpressionMapper;

namespace HRApi.Controllers
{
    [Route("[controller]")]
    //[Authorize]
    public class UserController : Controller
    {
        private HRContext _ctx;
        private UserManager<RegUser> _userManager;
        private readonly IMapper _mapper;
        // private RoleManager<RegUser> _roleManager;

        public UserController(HRContext ctx, UserManager<RegUser> UserManager, IMapper mapper)
        {
            _ctx = ctx;
            _userManager = UserManager;
            _mapper = mapper;

            //_roleManager = RoleManager;
        }

        //[Authorize(Roles = "SuperUser, HrManager")]
        [HttpGet("GetUser")]
        public IEnumerable<RegUser> GetUser()
        {

            return _ctx.RegUsers.ToList();
            //var user = _ctx.RegUsers.ToList();

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<RegUser, UserViewModel>();
            //});

            //var mapper = config.CreateMapper();
            //var source = new RegUser();
            //var dest = mapper.Map<RegUser, UserViewModel>(user);
            //IEnumerable<UserViewModel> model = _mapper.Map<List<RegUser>, List<UserViewModel>>(user);

            //Mapper.AssertConfigurationIsValid();

            //return model;

        }

        //[Authorize(Roles = "SuperUser,HrManager")]
        [HttpGet("ListUserByRole")]
        public List<RegUser> ListUserByRole([FromQuery] string role)
        {
            var users = _ctx.Users.Where
                (x => x.Roles.Select
                (y => y.RoleId)
                .Contains(role)).ToList();

            return users;
        }

        //[Authorize(Roles = "SuperUser,HrManager")]
        [HttpGet("ListUserByKeyword")]
        public List<RegUser> ListUserByKeyword([FromQuery] string keyword)
        {
            var user = _ctx.RegUsers.Where
                (u => u.RegUserKeyword
                .Contains(keyword)).ToList();

            return user;
        }

        //[HttpGet("GetUsersInCompanyArea/{companyName}")]
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

        //[Authorize(Roles = "SuperUser, HrManager,RegUser")]
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
            _ctx.SaveChanges();

            return Ok("Edited");
        }

        //[Authorize(Roles="SuperUser,HrManager")]
        [HttpPut("UserAdditionalInfo/{userName}")]
        public IActionResult UserAdditionalInfo(RegUser regUser, string userName, string additionalInfo)
        {
            var user = _ctx.RegUsers.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return NotFound();
            }
            user.RegUserAdditionalInfo = regUser.RegUserAdditionalInfo;
            return Ok("Aditional information added.");
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