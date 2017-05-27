using HRApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRApi.Controllers
{
    [Route("[controller]")]
    public class JobController : Controller
    {
        private HRContext _jobctx;

        public JobController(HRContext job /*, IHttpContextAccessor httpContextAccessor*/)
        {
            _jobctx = job;
            //var userId = httpContextAccessor.HttpContext. .FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [AllowAnonymous]
        [HttpGet("GetJob")]
        public IEnumerable<Job> GetJob()
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

        //later
        [HttpPost("ApplyForJob")]
        [AllowAnonymous]
        public IActionResult ApplyForJob(string jobName)
        {

            var job = _jobctx.Jobs.Select
                (j => j.JobName.Contains(jobName));
            if (User.Identity.IsAuthenticated)
            {
                
            }
            return Ok("You applied for the position");

        }

        [Authorize(Roles = "SuperUser,HrManager")]
        [HttpPost("CreateJob")]
        public IActionResult CreateJob([FromBody] Job jobs)
        {
            if (jobs == null)
            {
                return BadRequest();
            }

            _jobctx.Add(jobs);
            _jobctx.SaveChanges();

            return Created("api/usercontroller", jobs);
        }

        [Authorize(Roles = "SuperUser,HrManager")]
        [HttpPut("EditJob/{jobName}")]
        public IActionResult EditJob([FromBody] Job jobs, string jobName)
        {
            if (jobName == null)
            {
                return BadRequest();
            }

            var todo = _jobctx.Jobs.Find(jobName);
            if (todo == null)
            {
                return NotFound();
            }

            todo.JobName = jobs.JobName;
            todo.JobDesc = jobs.JobDesc;
            todo.JobCity = jobs.JobCity;
            todo.JobCountry = jobs.JobCountry;
            todo.JobCategories = jobs.JobCategories;
            todo.JobSalary = jobs.JobSalary;
            todo.JobReqXp = jobs.JobReqXp;
            todo.JobPartFull = jobs.JobPartFull;
            todo.JobKeyword = jobs.JobKeyword;

            _jobctx.SaveChanges();

            return Ok();
        }

        [Authorize(Roles = "SuperUser,HrManager")]
        [HttpDelete("DeleteJob/{JobId}")]
        public IActionResult DeleteJob(int JobId)
        {
            var todo = _jobctx.Jobs.Find(JobId);
            if (todo == null)
            {
                return NotFound();
            }
            _jobctx.Jobs.Remove(todo);
            _jobctx.SaveChanges();

            return Ok();
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
    }
}
