using DataAccessLibrary;
using DataAccessLibrary.TestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICV2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestService testData;

        public TestController(TestService testData)
        {
            this.testData = testData;
        }

        [HttpGet]
        public async Task<IActionResult> OnGetAsync(string companyCode)
        {      
            var data = await testData.ShowEmployee(companyCode);

            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
