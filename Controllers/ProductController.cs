using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI_Template_v2.Services.Product;

namespace NetCoreAPI_Template_v2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController: ControllerBase
    {
        private readonly IProductService _proService;

        public ProductController(IProductService proService)
        {
            _proService = proService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _proService.GetAllProducts());
        }
    }
}