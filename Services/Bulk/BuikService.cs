using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI_Template_v2.Data;
using NetCoreAPI_Template_v2.DTOs;
using NetCoreAPI_Template_v2.Helpers;
using NetCoreAPI_Template_v2.Models;

namespace NetCoreAPI_Template_v2.Services
{
    public class BuikService : IBulkService
    {
        private readonly AppDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;
        public BuikService(AppDBContext dbContext, IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
            _dbContext = dbContext;

        }
        public List<Bulk> BulkDelete()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = _dbContext.Bulk.ToList();
            _dbContext.BulkDelete(bulk);
            return bulk;
        }

        public List<Bulk> BulkInsert()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = GetDataForInsert();
            _dbContext.BulkInsert(bulk);
            return bulk;
        }

        public List<Bulk> BulkUpdate()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = GetDataForUpdate();
            _dbContext.BulkUpdate(bulk);
            return bulk;
        }

        public List<Bulk> GetBulks()
        {
            return _dbContext.Bulk.ToList();
        }

        private static List<Bulk> GetDataForInsert()
        {
            List<Bulk> bulk = new List<Bulk>();
            for (int i = 0; i <= 100; i++)
            {
                bulk.Add(new Bulk() { BulkId = i + 1, BulkName = "Insert BulkName" + i, BulkCode = "InsertBulkCode" + i }); //gen id
            }
            return bulk;
        }

        private static List<Bulk> GetDataForUpdate()
        {
            List<Bulk> bulk = new List<Bulk>();
            for (int i = 0; i <= 100; i++)
            {
                bulk.Add(new Bulk() { BulkId = i + 1, BulkName = "Update BulkName" + i, BulkCode = "UpdateBulkCode" + i }); //gen id
            }
            return bulk;
        }

        public async Task<ServiceResponseWithPagination<List<Bulk>>> GetBulksWithPagination(PaginationDto pagination)
        {
            var queryable = _dbContext.Bulk.AsQueryable();
            var paginationResult = await _httpContext.HttpContext
                .InsertPaginationParametersInResponse(queryable,pagination.RecordsPerPage, pagination.Page);
            var dto = await queryable.Paginate(pagination).ToListAsync();

            return ResponseResultWithPagination.Success(dto, paginationResult);
        }
    }
}