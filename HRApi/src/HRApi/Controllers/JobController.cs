using HRApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRApi.Controllers
{
    [Route("api/[controller]")]
    public class JobController : Controller
    {
        private HRContext _ctx;

        [HttpGet("GetJob")]
        public IEnumerable<Jobs> GetJob()
        {
            return _ctx.Jobs.ToList();
        }

        [HttpPost("CreateJob")]
        public IActionResult CreateJob([FromBody] Jobs jobs)
        {
            if (jobs== null)
            {
                    return BadRequest();
            }

            _ctx.Add(jobs);
            _ctx.SaveChanges();

            return Created("api/usercontroller", jobs);
        }

        [HttpPut("EditJob/{JobId}")]
        public IActionResult EditJob([FromBody] Jobs jobs, int JobId)
        {
            if (jobs == null)
            {
                return BadRequest();
            }

            var todo = _ctx.Jobs.Find(jobs);
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
           
            _ctx.SaveChanges();

            return Ok();
        }

        [HttpDelete("DeleteJob/{JobId}")]
        public IActionResult DeleteJob(int JobId)
        {
            var todo = _ctx.Jobs.Find(JobId);
            if (todo == null)
            {
                return NotFound();
            }
            _ctx.Jobs.Remove(todo);
            _ctx.SaveChanges();

            return Ok();
        }
        

    }
}
