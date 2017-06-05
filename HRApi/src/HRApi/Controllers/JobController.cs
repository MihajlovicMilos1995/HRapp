using HRApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRApi.Controllers
{
    [Route("[controller]")]
    public class JobController : Controller
    {
        private HRContext _jobctx;
        private readonly UserManager<RegUser> _userManager;

        public JobController(HRContext job, UserManager<RegUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _jobctx = job;
            _userManager = userManager;
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var userId = httpContextAccessor.HttpContext. .FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet("GetJob")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View("~/Views/General/Job/Index.cshtml", await _jobctx.Jobs.ToListAsync());
        }

        [HttpGet("GetJobJson")]
        public IEnumerable<Job> GetJobJson()
        {
            return _jobctx.Jobs.ToList();
        }
        [HttpGet("GetJobsByCity")]
        [AllowAnonymous]
        public List<Job> GetJobsByCity(string city)
        {
            if (city != null)
            {
                var jobs = _jobctx.Jobs.Where
                    (x => x.JobCity
                   .Contains(city)).ToList();

                return jobs;
            }
            return _jobctx.Jobs.ToList();
        }

        [HttpGet("GetJobsByCountry")]
        [AllowAnonymous]
        public List<Job> GetJobsByCountry([FromQuery]string country)
        {
            if (country != null)
            {
                var jobs = _jobctx.Jobs.Where
                    (j => j.JobCountry
                .Contains(country)).ToList();
                return jobs;
            }
            return _jobctx.Jobs.ToList();
        }

        [HttpGet("GetJobsByKeyword")]
        [AllowAnonymous]
        public List<Job> GetJobsByKeyword([FromQuery]string keyword)
        {
            if (keyword != null)
            {
                var jobs = _jobctx.Jobs.Where
                    (j => j.JobKeyword
                .Contains(keyword)).ToList();

                return jobs;
            }
            return _jobctx.Jobs.ToList();
        }

        [HttpGet("GetJobsByKeyword")]
        [AllowAnonymous]
        public List<Job> GetJobsByTime([FromQuery]string time)
        {
            if (time != null)
            {
                var jobs = _jobctx.Jobs.Where
                    (j => j.JobPartFull
                    .ToString().Contains(time)).ToList();

                return jobs;
            }
            return _jobctx.Jobs.ToList();
        }


        [HttpPost("ApplyForJob")]
        //[Authorize("HrManager")]
     //   public async Task<IActionResult> ApplyForJob(int id, Job jobs, RegUser users, AutoGenHistory history)
     //   {
     //       var job = await _jobctx.Jobs
     //       .SingleOrDefaultAsync(m => m.JobId == id);
     //
     //       var user = await _userManager.GetUserAsync(HttpContext.User);
     //
     //       //   if (User.Identity.IsAuthenticated)
     //       //   {
     //       //       var temp = new TempPosition();
     //       //       temp.job = job;
     //       //       temp. = user;
     //       //       _jobctx.Temp.Add(temp);
     //       //       _jobctx.SaveChanges();
     //       //}
     //       else
     //       {
     //           RedirectToAction("Login", "Account");
     //       }
     //       return Ok("You applied for the position");
     //   }

        [HttpGet("Accept")]
        public IActionResult Accept(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobapp = _jobctx.History.FirstOrDefault(j => j.Job.JobId == id);

            if (User.IsInRole("HrManager"))
            {

            }
            return Ok("User is accepted");
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _jobctx.Jobs
                .SingleOrDefaultAsync(m => m.JobId == id);
            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
            //return View("~/Views/General/Job/Details.cshtml", job);
        }

        [Authorize(Roles = "SuperUser,HrManager")]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/General/Job/Create.cshtml");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobId,JobName,JobDesc,JobCity,JobCountry,JobCategories,JobSalary,JobReqXp,JobPartFull,JobKeyword")] Job job)
        {
            if (ModelState.IsValid)
            {
                _jobctx.Add(job);
                await _jobctx.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("~/Views/General/Job/Index.cshtml", job);
        }

        [HttpPost("CreateJson")]
        public IActionResult CreateJson([Bind("JobId,JobName,JobDesc,JobCity,JobCountry,JobCategories,JobSalary,JobReqXp,JobPartFull,JobKeyword")] Job job)
        {
            _jobctx.Jobs.Add(job);
            _jobctx.SaveChanges();
            return Ok(job);
        }

        [HttpPut("EditJson")]
        public IActionResult EditJson([Bind("JobId,JobName,JobDesc,JobCity,JobCountry,JobCategories,JobSalary,JobReqXp,JobPartFull,JobKeyword")] Job job, int id)
        {
            if (job == null)
            {
                return BadRequest();
            }

            var todo = _jobctx.Jobs.FirstOrDefault(j => j.JobId == id);
            if (User.Identity.IsAuthenticated)
            {
                todo.JobName = job.JobName;
                todo.JobDesc = job.JobDesc;
                todo.JobCity = job.JobCity;
                todo.JobCountry = job.JobCountry;
                todo.JobKeyword = job.JobKeyword;
                todo.JobPartFull = job.JobPartFull;
                todo.JobReqXp = job.JobReqXp;
                todo.JobSalary = job.JobSalary;
                todo.JobCategories = job.JobCategories;

            }
            _jobctx.SaveChanges();
            return Ok("Edited");
        }
        [Authorize(Roles = "SuperUser,HrManager")]
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _jobctx.Jobs
                .SingleOrDefaultAsync(m => m.JobId == id);
            if (job == null)
            {
                return NotFound();
            }

            return View("~/Views/General/Job/Edit.cshtml", job);
        }


        //// POST: BackOfficeUser/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobName,JobDesc,JobCity,JobCountry,JobCategories,JobSalary,JobReqXp,JobPartFull,JobKeyword")] Job job)
        {
            if (id != job.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _jobctx.Update(job);
                    await _jobctx.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (JobExists(job.JobId))
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
            return View("~/Views/General/Job/Index.cshtml", job);
        }

        [HttpDelete("DeleteJson")]
        public IActionResult DeleteJson(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _jobctx.Jobs.FirstOrDefault(c => c.JobId == id);
            if (company == null)
            {
                return NotFound();
            }
            else
            {
                _jobctx.Remove(company);
                _jobctx.SaveChanges();
            }

            return Ok("Deleted");
        }

        [HttpGet("Delete")]
        [Authorize(Roles = "SuperUser,HrManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _jobctx.Jobs
                .SingleOrDefaultAsync(j => j.JobId == id);
            if (job == null)
            {
                return NotFound();
            }

            return View("~/Views/General/Job/Delete.cshtml", job);
        }

        // POST: BackOfficeUser/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _jobctx.Jobs.SingleOrDefaultAsync(j => j.JobId == id);
            _jobctx.Jobs.Remove(job);
            await _jobctx.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet("SSP")]
        public IActionResult SearchAndSort([FromQuery]string searchString, [FromQuery] string sortBy, [FromQuery] int page, [FromQuery] int jobsPerPage = 3)
        {
            var job = from j in _jobctx.Jobs
                      select j;

            if (searchString != null)
            {
                job = job.Where(j => j.JobName.Contains(searchString)
                                        || j.JobKeyword.Contains(searchString));
            }

            if (sortBy == "Des")
            {
                job = job.OrderByDescending(j => j.JobName);
            }
            else if (sortBy == "Asc")
            {
                job = job.OrderBy(j => j.JobName);
            }

            if (page > 0)
            {
                job = job.Skip((page - 1) * jobsPerPage).Take(jobsPerPage);
            }

            return Ok(job);
        }
        private bool JobExists(int id)
        {
            return _jobctx.Jobs.Any(e => e.JobId == id);
        }
    }
}
