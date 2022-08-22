using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DemoWebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace DemoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        db1045Context db = new db1045Context();

        [HttpGet]
        [Route("listdept")]
        public IActionResult GetDept()
        {
            // EF common syntax
            //var data = db.Depts.ToList(); 

            // Linq Syntax
            //var data = from dept in db.Depts select dept;

            // suppose you have 20 cols, but we dont want to return all 20 columns
            var data = from dept in db.Depts select new { Id = dept.Id, Name = dept.Name, Location = dept.Location };

            return Ok(data);
        }

        [HttpGet]
        [Route("listdept/{id}")]
        public IActionResult GetDept(int? id)
        {
            if (id == null) {
                return BadRequest("Id is NUll");
            }

            var data = from dept in db.Depts where dept.Id == id select new { Id = dept.Id, Name = dept.Name, Location = dept.Location };
            return Ok(data);
        }
    }
}
