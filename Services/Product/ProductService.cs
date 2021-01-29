using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NetCoreAPI_Template_v2.Data;
using NetCoreAPI_Template_v2.DTOs.Product;
using NetCoreAPI_Template_v2.Models;
using NetCoreAPI_Template_v2.Models.Product;
using NetCoreAPI_Template_v2.Services.Product;

namespace NetCoreAPI_Template_v2
{
    public class ProductService : IProductService
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _log;
        public ProductService(AppDBContext dbContext, IMapper mapper, ILogger<ProductService> log)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _log = log;
        }

        public async Task<ServiceResponse<GetProductDto>> AddProduct(AddProductDto newProduct)
        {
            var products = await _dbContext.Products.FirstOrDefaultAsync(x=>x.Name == newProduct.Name);
            if (products != null)
            {
                return ResponseResult.Failure<GetProductDto>("product duplicate.");
            }

            var product = new Product
            {
                Name = newProduct.Name,
                Price = newProduct.Price,
                StockCount = newProduct.StockCount,
                ProductGroupId = newProduct.ProductGroupId
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var productToReturn = await _dbContext.Products
            .Include(x => x.ProductGroup)
            .Where(x => x.Id == product.Id)
            .FirstOrDefaultAsync();

            var dto = _mapper.Map<GetProductDto>(productToReturn);
            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProducts()
        {
            var Products = await _dbContext.Products.Include(x=>x.ProductGroup).ToListAsync();
            var dto = _mapper.Map<List<GetProductDto>>(Products);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetProductDto>> GetProductById(int productId)
        {
            var product = await _dbContext.Products.Include(x => x.ProductGroup).FirstOrDefaultAsync(x => x.Id == productId);
            if (product is null)
            {
                return ResponseResult.Failure<GetProductDto>("Product not found.");
            }

            var dto = _mapper.Map<GetProductDto>(product);

            return ResponseResult.Success(dto);
        }
    }
}