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
    [Authorize(Roles = "SuperUser")]
    public class BackOfficeJobController : Controller
    {
        private readonly HRContext _context;

        public BackOfficeJobController(HRContext context)
        {
            _context = context;
        }

        [Authorize (Roles ="SuperUser,HrManager")]
        [HttpGet("GetJobByCompany")]
        public IActionResult GetJobByCompany(string companyName, bool includeJob = false)
        {

            var company = _context.Companies.Include(c => c.Jobs)
                    .Where(c => c.CompanyName == companyName).FirstOrDefault();

            if (company == null)
            {
                return NotFound();
            }
            if (includeJob)
            {
                return Ok(company);
            }

            var p = _context.Companies.Where(c => c.CompanyName == companyName).FirstOrDefault();
            if (company == null)
            {
                return NotFound();
            }

            return Ok(p);
        }
    }
}
