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


        //[HttpGet("GetCompany")]
        //public IEnumerable<Company> GetCompany()
        //{
        //    return _compctx.Companies.ToList();
        //}

        [Authorize(Roles = ("SuperUser,HrManager"))]
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
