using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using HRApi.Enums;

namespace HRApi.Controllers
{
    [Authorize(Roles = "SuperUser")]
    [Route("{BOC}")]
    public class BackEndController : Controller
    {
        private readonly HRContext _context;
        public BackEndController(HRContext context)
        {
            _context = context;    
        }

        [HttpGet("status/{status}")]
        public List<RegUser> GetJobsByStatus(UserStatus status)
        {

            var users = _context.Users
                   .Where(c => c.StatusOfUser == status);

            return users.ToList();
        }

        // GET: BackEnd
        public async Task<IActionResult> Index()
        {
            return View("~/Views/BackOffice/Company/Index.cshtml",await _context.Companies.ToListAsync());
        }

        // GET: BackEnd/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _context.Companies
                .FirstOrDefault(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View("~/Views/BackOffice/Company/Details.cshtml", company);
        }

        // GET: BackEnd/Create
        public IActionResult Create()
        {
            return View("~/Views/BackOffice/Company/Create.cshtml");
        }

        // POST: BackEnd/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("CreateCompany")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyName,CompanyDesc,CompanyCity,CompanyCountry,CompanyPhone,CompanyEmail,CompanyWebSite")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("~/Views/BackOffice/Company/Index.cshtml",company);
        }

        // GET: BackEnd/Edit/5
        [HttpGet("EditCompany")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _context.Companies.FirstOrDefault(c => c.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }
            return View("~/Views/BackOffice/Company/Edit.cshtml",company);
        }

        // POST: BackEnd/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id , [Bind("CompanyName,CompanyDesc,CompanyCity,CompanyCountry,CompanyPhone,CompanyEmail,CompanyWebSite")] Company company)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
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
            return View("~/Views/BackOffice/Company/Index.cshtml",company);
        }

        // GET: BackEnd/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .SingleOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View("~/Views/BackOffice/Company/Delete.cshtml",company);
        }

        // POST: BackEnd/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.SingleOrDefaultAsync(m => m.CompanyId == id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
