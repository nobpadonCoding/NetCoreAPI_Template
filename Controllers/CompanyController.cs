using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI_Template_v2.Services.Company;

namespace NetCoreAPI_Template_v2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _comService;
        public CompanyController(ICompanyService comService)
        {
            _comService = comService;

        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            return Ok(await _comService.GetAllEmployees());
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            return Ok(await _comService.GetEmployeeById(employeeId));
        }
    }
}