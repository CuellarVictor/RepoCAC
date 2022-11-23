using AuditCAC.Dal.Entities;
using AuditCAC.Domain;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Route("api/test")]
    [ApiController]
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestController : ControllerBase
    {
        private readonly IDataRepository<Test> _dataRepository;
        //private readonly IProceduresRepository<CoberturaModel> _proceduresRepository;

        public TestController(IDataRepository<Test> dataRepository) //, IProceduresRepository<CoberturaModel> proceduresRepository
        {
            this._dataRepository = dataRepository;
            //this._proceduresRepository = proceduresRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Test> tests = _dataRepository.GetAll();
            return Ok(tests);

            //_dataRepository.Reset();
            //return Ok();
        }
        // GET: api/Employee/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(long id)
        {
            Test test = _dataRepository.Get(id);
            if (test == null)
            {
                return NotFound("The Test record couldn't be found.");
            }
            return Ok(test);
        }
        // POST: api/Employee
        [HttpPost]
        public IActionResult Post([FromBody] Test test)
        {
            if (test == null)
            {
                return BadRequest("Test is null.");
            }
            _dataRepository.Add(test);
            return CreatedAtRoute(
                  "Get",
                  new { Id = test.ID },
                  test);
        }
        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Test test)
        {
            if (test == null)
            {
                return BadRequest("Test is null.");
            }
            Test testToUpdate = _dataRepository.Get(id);
            if (testToUpdate == null)
            {
                return NotFound("The Test record couldn't be found.");
            }
            _dataRepository.Update(testToUpdate, test);
            return NoContent();
        }
        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Test test = _dataRepository.Get(id);
            if (test == null)
            {
                return NotFound("The Test record couldn't be found.");
            }
            _dataRepository.Delete(test);
            return NoContent();
        }
    }
}
