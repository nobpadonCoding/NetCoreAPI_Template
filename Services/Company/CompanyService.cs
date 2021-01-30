using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreAPI_Template_v2.Data;
using NetCoreAPI_Template_v2.DTOs.Company;
using NetCoreAPI_Template_v2.Models;

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

        public async Task<ServiceResponse<GetEmployeeDto>> GetEmployeeById(int employeeId)
        {
            try
            {
                var employee = await _dbContext.Employees
                .Include(x => x.Department)
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.Id == employeeId);
                if (employee is null)
                {
                    _log.LogError($"employee id {employeeId} not found");
                    return ResponseResult.Failure<GetEmployeeDto>($"employee id {employeeId} not found");
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
    }
}