using HRApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRApi.Controllers
{
    [Route("[controller]")]
    public class CompanyController : Controller
    {
        private HRContext _compctx;

        public CompanyController(HRContext company)
        {
            _compctx = company;
        }


        [HttpGet("GetCompany")]
        public IEnumerable<Company> GetCompanies()
        {
            return _compctx.Companies.ToList();
        }

        //[Authorize(Roles = ("SuperUser,HrManager"))]
        [HttpGet("{companyId}")]
        public IActionResult GetCompany(int companyId, bool includeJob = false)
        {
            var company = _compctx.Companies.Include(c => c.Jobs)
                    .Where(c => c.CompanyId == companyId).FirstOrDefault();

            if (company == null)
            {
                return NotFound();
            }

            if (includeJob)
            {
                return Ok(company);
            }
            var p = _compctx.Companies.Where(c => c.CompanyId == companyId).FirstOrDefault();
            if (company == null)
            {
                return NotFound();
            }

            return Ok(p);
        }

        [HttpPost("Create")]
        public IActionResult Create([Bind("CompanyName,CompanyDesc,CompanyCity,CompanyCountry,CompanyPhone,CompanyEmail,CompanyWebSite")]Company company)
        {
            var newcompany = _compctx.Companies.Add(company);
            _compctx.SaveChanges();
            return Ok(company);
        }

        [HttpPut("Edit")]
        public IActionResult Edit([Bind("CompanyName,CompanyDesc,CompanyCity,CompanyCountry,CompanyPhone,CompanyEmail,CompanyWebSite")] Company company, int id)
        {
            if (company == null)
            {
                return BadRequest();
            }

            var todo = _compctx.Companies.FirstOrDefault(j => j.CompanyId == id);
            if (User.Identity.IsAuthenticated)
            {
                todo.CompanyName = company.CompanyName;
                todo.CompanyDesc = company.CompanyDesc;
                todo.CompanyCity = company.CompanyCity;
                todo.CompanyCountry = company.CompanyCountry;
                todo.CompanyPhone = company.CompanyPhone;
                todo.CompanyEmail = company.CompanyEmail;
                todo.CompanyWebSite = company.CompanyWebSite;
            }
            _compctx.SaveChanges();
            return Ok("Edited");
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _compctx.Companies.FirstOrDefault(c => c.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }
            else
            {
                _compctx.Remove(company);
                _compctx.SaveChanges();
            }

            return Ok("Deleted");
        }

        [HttpGet("SSP")]
        public IActionResult SearchAndSort([FromQuery]string searchString, [FromQuery] string sortBy, [FromQuery] int page, [FromQuery] int companiesPerPage = 3)
        {
            var company = from c in _compctx.Companies
                          select c;

            if (searchString != null)
            {
                company = company.Where(c => c.CompanyName.Contains(searchString)
                                        || c.CompanyCity.Contains(searchString));
            }

            if (sortBy == "Des")
            {
                company = company.OrderByDescending(c => c.CompanyName);
            }
            else if (sortBy == "Asc")
            {
                company = company.OrderBy(c => c.CompanyName);
            }

            if (page > 0)
            {
                company = company.Skip((page - 1) * companiesPerPage).Take(companiesPerPage);
            }

            return Ok(company);
        }
    }
}
