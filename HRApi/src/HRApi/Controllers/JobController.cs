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

        public JobController(HRContext job, UserManager<RegUser> userManager /*, IHttpContextAccessor httpContextAccessor*/)
        {
            _jobctx = job;
            _userManager = userManager;
            //var userId = httpContextAccessor.HttpContext. .FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet("GetJob")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View("~/VIews/General/Job/Index.cshtml", await _jobctx.Jobs.ToListAsync());
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

        //later
        [HttpPost("ApplyForJob")]
        [AllowAnonymous]
        public async Task<IActionResult> ApplyForJob(int? id, Job jobs, RegUser user, AutoGenHistory history)
        {
            var job = await _jobctx.Jobs
            .SingleOrDefaultAsync(m => m.JobId == id);

            string userId = _userManager.GetUserId(User);
            //if (User.Identity.IsAuthenticated)
            //{
            //    _jobctx.History.Add(job);
            //    _jobctx.History.Add(userId);
            //}
            //else
            //  {
            //    RedirectToAction("Login");
            //  }
            return Ok("You applied for the position");
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

            return View("~/VIews/General/Job/Details.cshtml", job);
        }

        [Authorize(Roles = "SuperUser,HrManager")]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobName,JobDesc,JobCity,JobCountry,JobCategories,JobSalary,JobReqXp,JobPartFull,JobKeyword")] Job job)
        {
            if (ModelState.IsValid)
            {
                _jobctx.Add(job);
                await _jobctx.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("~/VIews/General/Job/Create.cshtml", job);
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

            return View("~/VIews/BackOffice/User/Edit.cshtml", job);
        }


        //// POST: BackOfficeUser/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("JobName,JobDesc,JobCity,JobCountry,JobCategories,JobSalary,JobReqXp,JobPartFull,JobKeyword")] Job job)
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
            return View("~/VIews/BackOffice/User/Index.cshtml", job);
        }

        [Authorize(Roles = "SuperUser,HrManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regUser = await _jobctx.Jobs
                .SingleOrDefaultAsync(j => j.JobId == id);
            if (regUser == null)
            {
                return NotFound();
            }

            return View("~/VIews/General/Job/Delete.cshtml", regUser);
        }

        // POST: BackOfficeUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var regUser = await _jobctx.Jobs.SingleOrDefaultAsync(j => j.JobId == id);
            _jobctx.Jobs.Remove(regUser);
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
