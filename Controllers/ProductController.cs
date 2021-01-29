using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI_Template_v2.DTOs.Product;
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

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            return Ok(await _proService.GetProductById(productId));
        }

        [HttpPost("addproduct")]
        public async Task<IActionResult> AddProduct(AddProductDto newProduct)
        {
            return Ok(await _proService.AddProduct(newProduct));
        }

        [HttpPut("editproduct")]
        public async Task<IActionResult> EditProduct(EditProductDto editProduct)
        {
            return Ok(await _proService.EditProduct(editProduct));
        }
    }
}