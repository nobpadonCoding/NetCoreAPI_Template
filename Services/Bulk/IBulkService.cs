using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreAPI_Template_v2.DTOs;
using NetCoreAPI_Template_v2.DTOs.Bulk;
using NetCoreAPI_Template_v2.Models;

namespace NetCoreAPI_Template_v2.Services
{
    public interface IBulkService
    {
        List<Bulk> BulkInsert();
        List<Bulk> BulkUpdate();
        List<Bulk> BulkDelete();
        List<Bulk> GetBulks();

        Task<ServiceResponseWithPagination<List<Bulk>>> GetBulksWithPagination(PaginationDto pagination);
        Task<ServiceResponseWithPagination<List<Bulk>>> GetBulksFilter(BulkFilterDto filter);
        Task<ServiceResponse<List<Bulk>>> GetBulksByInlineSQL(int bulkId);
        Task<ServiceResponse<List<Bulk>>> GetBulksByStoreProcedure(int bulkId);
    }
}