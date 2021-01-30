using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreAPI_Template_v2.Data;
using NetCoreAPI_Template_v2.DTOs.Company;
using NetCoreAPI_Template_v2.Models;
using NetCoreAPI_Template_v2.Models.Company;

namespace NetCoreAPI_Template_v2.Services.Company
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _log;

        public CompanyService(AppDBContext dbContext, IMapper mapper, ILogger<CompanyService> log)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _log = log;
        }

        public async Task<ServiceResponse<List<GetEmployeeDto>>> GetAllEmployees()
        {
            try
            {
                var Employees = await _dbContext.Employees
               .Include(x => x.Position)
               .Include(x => x.Department).ToListAsync();

                var dto = _mapper.Map<List<GetEmployeeDto>>(Employees);

                _log.LogInformation("GetAllEmployees Success");
                return ResponseResult.Success(dto, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<List<GetEmployeeDto>>(ex.Message);
            }

        }

        public async Task<ServiceResponse<GetEmployeeDto>> GetEmployeeById(int EmployeeId)
        {
            try
            {
                var employee = await _dbContext.Employees
                .Include(x => x.Department)
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.Id == EmployeeId);
                if (employee is null)
                {
                    _log.LogError($"employee id {EmployeeId} not found");
                    return ResponseResult.Failure<GetEmployeeDto>($"employee id {EmployeeId} not found");
                }

                var dto = _mapper.Map<GetEmployeeDto>(employee);

                return ResponseResult.Success(dto, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetEmployeeDto>(ex.Message);
            }

        }

        public async Task<ServiceResponse<GetEmployeeDto>> AddEmployee(AddEmployeeDto newEmployee)
        {
            try
            {
                var position = await _dbContext.Positions.FirstOrDefaultAsync(x => x.Id == newEmployee.PositionId);
                if (position is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>("Position not found");
                }

                var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == newEmployee.DepartmentId);
                if (department is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>("Department not found");
                }

                var employee_new = new Employee
                {
                    Name = newEmployee.Name,
                    LastName = newEmployee.LastName,
                    PositionId = newEmployee.PositionId,
                    DepartmentId = newEmployee.DepartmentId
                };

                _dbContext.Employees.Add(employee_new);
                await _dbContext.SaveChangesAsync();

                var dto = _mapper.Map<GetEmployeeDto>(employee_new);
                _log.LogInformation($"Add employee Success");
                return ResponseResult.Success(dto, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetEmployeeDto>(ex.Message);
            }

        }

        public async Task<ServiceResponse<GetEmployeeDto>> EditEmployee(EditEmployeeDto editEmployee)
        {
            try
            {
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == editEmployee.Id);
                if (employee is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>($"employee id {editEmployee.Id} not found");
                }

                var position = await _dbContext.Positions.FirstOrDefaultAsync(x => x.Id == editEmployee.PositionId);
                if (position is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>("Position not found");
                }

                var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == editEmployee.DepartmentId);
                if (department is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>("Department not found");
                }

                employee.Name = editEmployee.Name;
                employee.LastName = editEmployee.LastName;
                employee.PositionId = editEmployee.PositionId;
                employee.DepartmentId = editEmployee.DepartmentId;

                _dbContext.Employees.Update(employee);
                await _dbContext.SaveChangesAsync();

                var dto = _mapper.Map<GetEmployeeDto>(employee);
                _log.LogInformation($"Add employee Success");
                return ResponseResult.Success(dto, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetEmployeeDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<GetEmployeeDto>> DeleteEmployee(int deleteEmployeeId)
        {
            try
            {
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == deleteEmployeeId);
                if (employee is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>($"employee id {deleteEmployeeId} not found");
                }

                _dbContext.Employees.RemoveRange(employee);
                await _dbContext.SaveChangesAsync();


                var dto = new GetEmployeeDto
                {
                    Id = deleteEmployeeId
                };

                _log.LogInformation("Delete Employee done.");
                return ResponseResult.Success(dto, "success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetEmployeeDto>(ex.Message);
            }
        }
    }
}