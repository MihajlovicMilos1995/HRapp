using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace HRApi.Controllers
{
    [Authorize("SuperUser")]

    public class BackOfficeUserController : Controller
    {
        private readonly HRContext _context;

        public BackOfficeUserController(HRContext context)
        {
            _context = context;
        }

        // GET: BackOfficeUser
        public async Task<IActionResult> Index()
        {

            return View("~/Views/BackOffice/User/Index.cshtml", await _context.RegUsers.ToListAsync());
        }

        // GET: BackOfficeUser/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regUser = await _context.RegUsers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (regUser == null)
            {
                return NotFound();
            }

            return View("~/Views/BackOffice/User/Details.cshtml", regUser);
        }

        [HttpGet("GetUserByKeyword")]
        public List<RegUser> GetUserByKeyword(string keyword)
        {
            var user = _context.RegUsers.Where
                (u => u.RegUserKeyword
                .Contains(keyword)).ToList();

            return user;
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regUser = await _context.RegUsers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (regUser == null)
            {
                return NotFound();
            }

            return View("~/Views/BackOffice/User/Edit.cshtml", regUser);
        }


        //// POST: BackOfficeUser/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RegUserName,statusOfUser,RegUserLastName,RegUserCity,RegUserCountry,LocationChange,RegUserPartFull,WorkXp,RegUserKeyword,RegUserSex,RegUserDoB,RegUserAdditionalInfo,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] RegUser regUser)
        {
            if (id != regUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(regUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegUserExists(regUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View("~/Views/BackOffice/User/Index.cshtml", regUser);
        }

        [HttpPost("CreateUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,RegUserName,RegUserLastName,RegUserEmail,RegUserPassword,RegUserCity,RegUserCountry,LocationChange,RegUserPartFull,WorkXp,RegUserKeyword,RegUserSex,RegUserDoB,RegUserAdditionalInfo")] RegUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("~/Views/BackOffice/User/Create.cshtml", user);
        }

        // GET: BackOfficeUser/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regUser = await _context.RegUsers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (regUser == null)
            {
                return NotFound();
            }

            return View("~/Views/BackOffice/User/Delete.cshtml", regUser);
        }

        // POST: BackOfficeUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var regUser = await _context.RegUsers.SingleOrDefaultAsync(m => m.Id == id);
            _context.RegUsers.Remove(regUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        private bool RegUserExists(string id)
        {
            return _context.RegUsers.Any(e => e.Id == id);
        }
    }
}
