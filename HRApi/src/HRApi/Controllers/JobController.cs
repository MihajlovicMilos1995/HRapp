using HRApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRApi.Controllers
{
    [Route("[controller]")]
    public class JobController : Controller
    {
        private HRContext _jobctx;

        public JobController(HRContext job)
        {
            _jobctx = job;
        }

        [AllowAnonymous]
        [HttpGet("GetJob")]
        public IEnumerable<Job> GetJob()
        {
            return _jobctx.Jobs.ToList();
        }

        [HttpPost("CreateJob")]
        public IActionResult CreateJob([FromBody] Job jobs)
        {
            if (jobs== null)
            {
                    return BadRequest();
            }

            _jobctx.Add(jobs);
            _jobctx.SaveChanges();

            return Created("api/usercontroller", jobs);
        }
                

        [HttpPut("EditJob/{JobId}")]
        public IActionResult EditJob([FromBody] Job jobs, int JobId)
        {
            if (jobs == null)
            {
                return BadRequest();
            }

            var todo = _jobctx.Jobs.Find(jobs);
            if (todo == null)
            {
                return NotFound();
            }

            todo.Name = jobs.Name;
            todo.JobDesc = jobs.JobDesc;
            todo.JobCity = jobs.JobCity;
            todo.JobCountry = jobs.JobCountry;
            todo.JobPartFull = jobs.JobPartFull;
            todo.JobKeyword = jobs.JobKeyword;
           
            _jobctx.SaveChanges();

            return Ok();
        }

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
        

    }
}
