using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreAPI_Template_v2.DTOs.Company;
using NetCoreAPI_Template_v2.Models;

namespace NetCoreAPI_Template_v2.Services.Company
{
    public interface ICompanyService
    {
        Task<ServiceResponse<List<GetEmployeeDto>>> GetAllEmployees();
        Task<ServiceResponse<GetEmployeeDto>> GetEmployeeById(int EmployeeId);
        Task<ServiceResponse<GetEmployeeDto>> AddEmployee(AddEmployeeDto newEmployee);
        Task<ServiceResponse<GetPositionDto>> AddPosition(AddPositionDto newPosition);
        Task<ServiceResponse<GetEmployeeDto>> EditEmployee(EditEmployeeDto editEmployee);
        Task<ServiceResponse<GetEmployeeDto>> DeleteEmployee(int deleteEmployeeId);
    }
}