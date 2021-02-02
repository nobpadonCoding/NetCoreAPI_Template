using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI_Template_v2.DTOs;
using NetCoreAPI_Template_v2.DTOs.Bulk;
using NetCoreAPI_Template_v2.Services;

namespace NetCoreAPI_Template_v2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ODataRoutePrefix("Bulk")]
    public class BulkController : ControllerBase
    {
        private readonly IBulkService _bulkService;
        public BulkController(IBulkService bulkService)
        {
            _bulkService = bulkService;

        }

        [HttpGet("all")]
        [EnableQuery]
        public IActionResult GetBulk()
        {
            return Ok(_bulkService.GetBulks());
        }
        [HttpPost("insert")]
        public IActionResult BulkInsert()
        {
            return Ok(_bulkService.BulkInsert());
        }

        [HttpPut("update")]
        public IActionResult BulkUpdate()
        {
            return Ok(_bulkService.BulkUpdate());
        }

        [HttpDelete("delete")]
        public IActionResult BulkDelete()
        {
            return Ok(_bulkService.BulkDelete());
        }

        [HttpGet("bulk/pagination")]
        public async Task<IActionResult> GetBulksWithPagination([FromQuery] PaginationDto pagination)
        {
            return Ok(await _bulkService.GetBulksWithPagination(pagination));
        }

        [HttpGet("bulk/filter")]
        public async Task<IActionResult> GetBulksFilter([FromQuery] BulkFilterDto filter)
        {
            return Ok(await _bulkService.GetBulksFilter(filter));
        }

        [HttpGet("bulk/inline")]
        public IActionResult GetBulkByInlineSQL(int bulkId)
        {
            return Ok(_bulkService.GetBulksByInlineSQL(bulkId));
        }

        [HttpGet("bulk/storeprocedure")]
        public IActionResult GetBulksByStoreProcedure(int bulkId)
        {
            return Ok(_bulkService.GetBulksByStoreProcedure(bulkId));
        }
    }
}