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
                _log.LogInformation($"Edit employee Success");
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

        public async Task<ServiceResponse<GetPositionDto>> AddPosition(AddPositionDto newPosition)
        {
            try
            {
                var position = await _dbContext.Positions.FirstOrDefaultAsync(x => x.Description == newPosition.PositionDescription);
                if (position != null)
                {
                    return ResponseResult.Failure<GetPositionDto>("Position duplicate.");
                }
                var position_new = new Position
                {
                    Description = newPosition.PositionDescription
                };

                _dbContext.Positions.Add(position_new);
                await _dbContext.SaveChangesAsync();

                var position_return = await _dbContext.Positions.FirstOrDefaultAsync(x => x.Description == newPosition.PositionDescription);

                var dto = _mapper.Map<GetPositionDto>(position_return);
                _log.LogInformation("Add Position Success.");
                return ResponseResult.Success(dto, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetPositionDto>(ex.Message);
            }

        }

        public async Task<ServiceResponse<GetDepartmentDto>> AddDepartment(AddDepartmentDto newDepartment)
        {
            try
            {
                var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Description == newDepartment.DepartmentDescription);
                if (department != null)
                {
                    return ResponseResult.Failure<GetDepartmentDto>("Department duplicate.");
                }
                var department_new = new Department
                {
                    Description = newDepartment.DepartmentDescription
                };

                _dbContext.Departments.Add(department_new);
                await _dbContext.SaveChangesAsync();

                var department_return = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Description == newDepartment.DepartmentDescription);

                var dto = _mapper.Map<GetDepartmentDto>(department_return);
                _log.LogInformation("Add Department Success.");
                return ResponseResult.Success(dto, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetDepartmentDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<GetPositionDto>> EditPosition(EditPositionDto editPosition)
        {
            try
            {
                var position = await _dbContext.Positions.FirstOrDefaultAsync(x => x.Id == editPosition.Id);
                if (position is null)
                {
                    return ResponseResult.Failure<GetPositionDto>($"Position id {editPosition.Id} not found");
                }

                position.Description = editPosition.PositionDescription;

                _dbContext.Positions.Update(position);
                await _dbContext.SaveChangesAsync();

                var dto = _mapper.Map<GetPositionDto>(position);
                _log.LogInformation($"Edit position Success");
                return ResponseResult.Success(dto, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetPositionDto>(ex.Message);
            }

        }

        public async Task<ServiceResponse<GetDepartmentDto>> EditDepartment(EditDepartmentDto editDepartment)
        {
            try
            {
                var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == editDepartment.Id);
                if (department is null)
                {
                    return ResponseResult.Failure<GetDepartmentDto>($"Position id {editDepartment.Id} not found");
                }

                department.Description = editDepartment.DepartmentDescription;

                _dbContext.Departments.Update(department);
                await _dbContext.SaveChangesAsync();

                var dto = _mapper.Map<GetDepartmentDto>(department);
                _log.LogInformation($"Edit position Success");
                return ResponseResult.Success(dto, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetDepartmentDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<GetDepartmentDto>> DeleteDepartment(int deleteDepartmentId)
        {
            try
            {
                var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == deleteDepartmentId);
                if (department is null)
                {
                    return ResponseResult.Failure<GetDepartmentDto>($"department id {deleteDepartmentId} not found");
                }

                var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.DepartmentId == deleteDepartmentId);
                if (employee != null)
                {
                    var department_Active = _mapper.Map<GetDepartmentDto>(department);
                    return ResponseResult.Failure<GetDepartmentDto>($"Department {department_Active.Description} employee is Active");
                }

                var department_return = _mapper.Map<GetDepartmentDto>(department);

                _dbContext.Departments.RemoveRange(department);
                await _dbContext.SaveChangesAsync();

                _log.LogInformation($"Delete Department id {deleteDepartmentId} done.");
                return ResponseResult.Success(department_return, "success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetDepartmentDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<GetPositionDto>> DeletePosition(int deletePositionId)
        {
            try
            {
                var position = await _dbContext.Positions.FirstOrDefaultAsync(x => x.Id == deletePositionId);
                if (position is null)
                {
                    return ResponseResult.Failure<GetPositionDto>($"Position id {deletePositionId} not found");
                }

                var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.PositionId == deletePositionId);
                if (employee != null)
                {
                    var Position_Active = _mapper.Map<GetPositionDto>(position);
                    return ResponseResult.Failure<GetPositionDto>($"Position {Position_Active.Description} employee is Active");
                }

                var Positions_return = _mapper.Map<GetPositionDto>(position);

                _dbContext.Positions.RemoveRange(position);
                await _dbContext.SaveChangesAsync();

                _log.LogInformation($"Delete position id {deletePositionId} done.");
                return ResponseResult.Success(Positions_return, "success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetPositionDto>(ex.Message);
            }
        }
    }
}