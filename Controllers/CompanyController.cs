using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI_Template_v2.DTOs.Company;
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

        [HttpGet("{EmployeeId}")]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            return Ok(await _comService.GetEmployeeById(employeeId));
        }

        [HttpPost("addEmployee")]
        public async Task<IActionResult> AddEmployee(AddEmployeeDto newemployee)
        {
            return Ok(await _comService.AddEmployee(newemployee));
        }

        [HttpPost("addPosition")]
        public async Task<IActionResult> AddPosition(AddPositionDto newPosition)
        {
            return Ok(await _comService.AddPosition(newPosition));
        }

        [HttpPut("{editEmployeeId}")]
        public async Task<IActionResult> EditEmployee(EditEmployeeDto editemployee)
        {
            return Ok(await _comService.EditEmployee(editemployee));
        }

        [HttpDelete("{deleteEmployeeId}")]
        public async Task<IActionResult> DeleteEmployee(int deleteEmployeeId)
        {
            return Ok(await _comService.DeleteEmployee(deleteEmployeeId));
        }
    }
}