﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IATITester.IATILib;
using IATITester.IATILib.IATIVersion1;
using IATITester.IATILib.IATIVersion2;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IATITester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Okay");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}