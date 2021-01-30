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

        [HttpPost("addDepartment")]
        public async Task<IActionResult> AddDepartment(AddDepartmentDto newDepartment)
        {
            return Ok(await _comService.AddDepartment(newDepartment));
        }

        [HttpPost("addPosition")]
        public async Task<IActionResult> AddPosition(AddPositionDto newPosition)
        {
            return Ok(await _comService.AddPosition(newPosition));
        }

        [HttpPut("Employee/{editEmployeeId}")]
        public async Task<IActionResult> EditEmployee(EditEmployeeDto editemployee)
        {
            return Ok(await _comService.EditEmployee(editemployee));
        }

        [HttpPut("Position/{editPositionId}")]
        public async Task<IActionResult> EditPosition(EditPositionDto editPosition)
        {
            return Ok(await _comService.EditPosition(editPosition));
        }

        [HttpPut("Department/{editDepartmentId}")]
        public async Task<IActionResult> EditDepartment(EditDepartmentDto editDepartment)
        {
            return Ok(await _comService.EditDepartment(editDepartment));
        }

        [HttpDelete("Employee/{deleteEmployeeId}")]
        public async Task<IActionResult> DeleteEmployee(int deleteEmployeeId)
        {
            return Ok(await _comService.DeleteEmployee(deleteEmployeeId));
        }
    }
}