using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreAPI_Template_v2.DTOs;
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
    }
}