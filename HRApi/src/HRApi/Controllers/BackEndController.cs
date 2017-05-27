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

namespace HRApi.Controllers
{
    [Authorize(Roles = "SuperUser")]
    [Route("{controller}")]
    public class BackEndController : Controller
    {
        private readonly HRContext _context;
        public BackEndController(HRContext context)
        {
            _context = context;    
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser()
        {
            return View(_context.RegUsers.ToList());
        }

        [HttpGet("GetUserByKeyword")]
        public List<RegUser> GetUserByKeyword(string keyword)
        {
            var user = _context.RegUsers.Where
                (u => u.RegUserKeyword
                .Contains(keyword)).ToList();

            return user;
        }

        // GET: BackEnd
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        // GET: BackEnd/Details/5
        public IActionResult Details(string companyName)
        {
            if (companyName == null)
            {
                return NotFound();
            }

            var company = _context.Companies
                .FirstOrDefault(m => m.CompanyName == companyName);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: BackEnd/Create
        public IActionResult Create()
        {
            return View();
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
            return View(company);
        }

        // GET: BackEnd/Edit/5
        public IActionResult Edit(string companyName)
        {
            if (companyName == null)
            {
                return NotFound();
            }

            var company = _context.Companies.FirstOrDefault(c => c.CompanyName == companyName);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: BackEnd/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string companyName , [Bind("CompanyName,CompanyDesc,CompanyCity,CompanyCountry,CompanyPhone,CompanyEmail,CompanyWebSite")] Company company)
        {
            if (companyName == null)
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
            return View(company);
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

            return View(company);
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
