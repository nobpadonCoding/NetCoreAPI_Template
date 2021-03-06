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
        public async Task<IActionResult> GetEmployeeById(int EmployeeId)
        {
            return Ok(await _comService.GetEmployeeById(EmployeeId));
        }

        [HttpPost("Employee/Add/addEmployee")]
        public async Task<IActionResult> AddEmployee(AddEmployeeDto newEmployee)
        {
            return Ok(await _comService.AddEmployee(newEmployee));
        }

        [HttpPost("Department/Add/addDepartment")]
        public async Task<IActionResult> AddDepartment(AddDepartmentDto newDepartment)
        {
            return Ok(await _comService.AddDepartment(newDepartment));
        }

        [HttpPost("Position/Add/addPosition")]
        public async Task<IActionResult> AddPosition(AddPositionDto newPosition)
        {
            return Ok(await _comService.AddPosition(newPosition));
        }

        [HttpPut("Employee/Update/{editEmployeeId}")]
        public async Task<IActionResult> EditEmployee(EditEmployeeDto editEmployee)
        {
            return Ok(await _comService.EditEmployee(editEmployee));
        }

        [HttpPut("Position/Update/{editPositionId}")]
        public async Task<IActionResult> EditPosition(EditPositionDto editPosition)
        {
            return Ok(await _comService.EditPosition(editPosition));
        }

        [HttpPut("Department/Update/{editDepartmentId}")]
        public async Task<IActionResult> EditDepartment(EditDepartmentDto editDepartment)
        {
            return Ok(await _comService.EditDepartment(editDepartment));
        }

        [HttpDelete("Employee/Delete/{deleteEmployeeId}")]
        public async Task<IActionResult> DeleteEmployee(int deleteEmployeeId)
        {
            return Ok(await _comService.DeleteEmployee(deleteEmployeeId));
        }

        [HttpDelete("Department/Delete/{deleteDepartmentId}")]
        public async Task<IActionResult> DeleteDepartment(int deleteDepartmentId)
        {
            return Ok(await _comService.DeleteDepartment(deleteDepartmentId));
        }

        [HttpDelete("Position/Delete/{deletePositionId}")]
        public async Task<IActionResult> DeletePosition(int deletePositionId)
        {
            return Ok(await _comService.DeletePosition(deletePositionId));
        }

        [HttpGet("Employee/filter")]
        public async Task<IActionResult> GetAllEmployeeFilter([FromQuery] EmployeeFilterDto EmployeeFilter)
        {
            return Ok(await _comService.GetEmployeeFilter(EmployeeFilter));
        }
    }
}