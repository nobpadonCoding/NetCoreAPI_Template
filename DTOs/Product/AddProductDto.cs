using System.ComponentModel.DataAnnotations;
using NetCoreAPI_Template_v2.Validations;

namespace NetCoreAPI_Template_v2.DTOs.Product
{
    public class AddProductDto
    {
        [FirstLetterUpperCaseAttribute]
        [Required]
        public string Name { get; set; }
        public int Price { get; set; }
        public int StockCount { get; set; }
        public int ProductGroupId { get; set; }
    }
}