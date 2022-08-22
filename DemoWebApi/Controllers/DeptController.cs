using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DemoWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DemoWebApi.ViewModel;
using System;

namespace DemoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        db1045Context db = new db1045Context();

        [HttpGet]
        [Route("/")]
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

            //var data = (from dept in db.Depts where dept.Id == id select new { Id = dept.Id, Name = dept.Name, Location = dept.Location }).FirstOrDefault();
            var data = db.Depts.Where(dept => dept.Id == id).Select(dept => new { Id = dept.Id, Name = dept.Name, Location = dept.Location }).FirstOrDefault();

            if (data == null) return NotFound($" Department {id} is not present");
            return Ok(data);
        }


        // http://localhost:20457/api/dept/ListCity?city=pune
        [HttpGet]
        [Route("listCity")]
        public IActionResult GetCity([FromQuery] string city)
        {
            if (city == null) return BadRequest("City is NUll");
            var data = db.Depts.Where(dept => dept.Location == city).Select(dept => new { Id = dept.Id, Name = dept.Name, Location = dept.Location });
            // data is array
            if (data.Count() == 0) return NotFound($"There are no departments in {city} city!");
            return Ok(data);
        }

        [HttpGet]
        [Route("ShowDept")]
        public IActionResult GetDeptInfo() {
            var data = db.DeptInfo_VMs.FromSqlInterpolated<DeptInfoVM>($"DeptInfo");
            return Ok(data);
        }

        [HttpPost]
        [Route("AddDept")]
        public IActionResult PostDept(Dept dept) {
                
            if (ModelState.IsValid) {
                try
                {
                    //db.Depts.Add(dept);
                    //db.SaveChanges();

                    db.Database.ExecuteSqlInterpolated($"spAddRecordsToDept {dept.Id}, {dept.Name}, {dept.Location}");

                    return Created("Record Successfully Added", dept);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.InnerException.Message);
                }
            }
            return BadRequest("Something went wrong!");
        }
    }
}
