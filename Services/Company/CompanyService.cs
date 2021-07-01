using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreAPI_Template_v2.Data;
using NetCoreAPI_Template_v2.DTOs.Company;
using NetCoreAPI_Template_v2.Helpers;
using NetCoreAPI_Template_v2.Models;
using NetCoreAPI_Template_v2.Models.Company;
using System.Linq.Dynamic.Core;

namespace NetCoreAPI_Template_v2.Services.Company
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _log;
        private readonly IHttpContextAccessor _httpContext;

        public CompanyService(AppDBContext dbContext, IMapper mapper, ILogger<CompanyService> log, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _log = log;
            _httpContext = httpContext;
        }

        public async Task<ServiceResponse<List<GetEmployeeDto>>> GetAllEmployees()
        {
            try
            {
                var Employees = await _dbContext.Employees
               .Include(x => x.Position)
               .Include(x => x.Department).ToListAsync();

                //mapper Dto and return
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
                //check employee
                if (employee is null)
                {
                    _log.LogError($"employee id {EmployeeId} not found");
                    return ResponseResult.Failure<GetEmployeeDto>($"employee id {EmployeeId} not found");
                }

                //mapper Dto and return
                var dto = _mapper.Map<GetEmployeeDto>(employee);

                _log.LogInformation("GetEmployeesById Success");
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
                //Check Position
                if (position is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>("Position not found");
                }

                var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == newEmployee.DepartmentId);
                //Check Department
                if (department is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>("Department not found");
                }

                //assign value
                var employee_new = new Employee
                {
                    Name = newEmployee.Name,
                    LastName = newEmployee.LastName,
                    PositionId = newEmployee.PositionId,
                    DepartmentId = newEmployee.DepartmentId
                };

                //insert database
                _dbContext.Employees.Add(employee_new);
                await _dbContext.SaveChangesAsync();

                //mapper Dto and return
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
                //Check Employee
                if (employee is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>($"employee id {editEmployee.Id} not found");
                }

                var position = await _dbContext.Positions.FirstOrDefaultAsync(x => x.Id == editEmployee.PositionId);
                //Check Position
                if (position is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>("Position not found");
                }

                var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == editEmployee.DepartmentId);
                //Check Department
                if (department is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>("Department not found");
                }

                //assign value
                employee.Name = editEmployee.Name;
                employee.LastName = editEmployee.LastName;
                employee.PositionId = editEmployee.PositionId;
                employee.DepartmentId = editEmployee.DepartmentId;

                 //insert database
                _dbContext.Employees.Update(employee);
                await _dbContext.SaveChangesAsync();

                //mapper Dto and return
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
                var employee = await _dbContext.Employees
                    .Include(x=>x.Position)
                    .Include(x=>x.Department)
                    .FirstOrDefaultAsync(x => x.Id == deleteEmployeeId);
                //check Employee
                if (employee is null)
                {
                    return ResponseResult.Failure<GetEmployeeDto>($"employee id {deleteEmployeeId} not found");
                }

                //mapper Dto and return
                var employee_return = _mapper.Map<GetEmployeeDto>(employee);

                //remove database
                _dbContext.Employees.RemoveRange(employee);
                await _dbContext.SaveChangesAsync();

                _log.LogInformation("Delete Employee done.");
                return ResponseResult.Success(employee_return, "success");
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
                //check Position duplicate
                if (!(position is null))
                {
                    return ResponseResult.Failure<GetPositionDto>("Position duplicate.");
                }
                //assiign value
                var position_new = new Position
                {
                    Description = newPosition.PositionDescription
                };

                //insert database
                _dbContext.Positions.Add(position_new);
                await _dbContext.SaveChangesAsync();

                var position_return = await _dbContext.Positions.FirstOrDefaultAsync(x => x.Description == newPosition.PositionDescription);

                //mapper Dto and return
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
                //check department duplicate
                if (!(department is null))
                {
                    return ResponseResult.Failure<GetDepartmentDto>("Department duplicate.");
                }

                //asign value
                var department_new = new Department
                {
                    Description = newDepartment.DepartmentDescription
                };

                //insert database
                _dbContext.Departments.Add(department_new);
                await _dbContext.SaveChangesAsync();

                var department_return = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Description == newDepartment.DepartmentDescription);
                //mapper Dto and return
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
                //check position
                if (position is null)
                {
                    return ResponseResult.Failure<GetPositionDto>($"Position id {editPosition.Id} not found");
                }

                //assign value
                position.Description = editPosition.PositionDescription;

                //update database
                _dbContext.Positions.Update(position);
                await _dbContext.SaveChangesAsync();

                //mapper Dto and return
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
                //check department
                var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == editDepartment.Id);
                if (department is null)
                {
                    return ResponseResult.Failure<GetDepartmentDto>($"Position id {editDepartment.Id} not found");
                }

                //assign value
                department.Description = editDepartment.DepartmentDescription;

                //update database
                _dbContext.Departments.Update(department);
                await _dbContext.SaveChangesAsync();

                //mapper Dto and return
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
                //caheck department
                var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.Id == deleteDepartmentId);
                if (department is null)
                {
                    return ResponseResult.Failure<GetDepartmentDto>($"department id {deleteDepartmentId} not found");
                }

                //check department is use?
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.DepartmentId == deleteDepartmentId);
                if (!(employee is null))
                {
                    var department_Active = _mapper.Map<GetDepartmentDto>(department);
                    return ResponseResult.Failure<GetDepartmentDto>($"Department {department_Active.Description} employee is Active");
                }

                //mapper Dto and return
                var department_return = _mapper.Map<GetDepartmentDto>(department);

                //remove database
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
                //check position
                if (position is null)
                {
                    return ResponseResult.Failure<GetPositionDto>($"Position id {deletePositionId} not found");
                }

                var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.PositionId == deletePositionId);
                //check position is use
                if (!(employee is null))
                {
                    var Position_Active = _mapper.Map<GetPositionDto>(position);
                    return ResponseResult.Failure<GetPositionDto>($"Position {Position_Active.Description} employee is Active");
                }

                //mapper Dto and return
                var Positions_return = _mapper.Map<GetPositionDto>(position);

                //remove database
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

        public async Task<ServiceResponse<List<Employee>>> GetEmployeeFilter(EmployeeFilterDto EmployeeFilter)
        {
            var queryable = _dbContext.Employees
                .Include(x => x.Position)
                .Include(x => x.Department).AsQueryable();

            //Filter
            if (!string.IsNullOrWhiteSpace(EmployeeFilter.EmployeeName))
            {
                queryable = queryable.Where(x => x.Name.Contains(EmployeeFilter.EmployeeName));
            }

            // if (!string.IsNullOrWhiteSpace(EmployeeFilter.EmployeeDepartment))
            // {
            //     queryable = queryable.Where(x => x.Department.Contains(EmployeeFilter.EmployeeDepartment));
            // }

            //Ordering
            if (!string.IsNullOrWhiteSpace(EmployeeFilter.OrderingField))
            {
                try
                {
                    queryable = queryable.OrderBy($"{EmployeeFilter.OrderingField} {(EmployeeFilter.AscendingOrder ? "ascending" : "descending")}");
                }
                catch
                {
                    return ResponseResultWithPagination.Failure<List<Employee>>($"Could not order by field: {EmployeeFilter.OrderingField}");
                }
            }

            var paginationResult = await _httpContext.HttpContext
                .InsertPaginationParametersInResponse(queryable, EmployeeFilter.RecordsPerPage, EmployeeFilter.Page);

            var dto = await queryable.Paginate(EmployeeFilter).ToListAsync();

            return ResponseResultWithPagination.Success(dto, paginationResult);
        }
    }
}