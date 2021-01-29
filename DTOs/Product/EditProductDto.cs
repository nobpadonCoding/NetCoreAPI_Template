using System.ComponentModel.DataAnnotations;
using NetCoreAPI_Template_v2.Validations;

namespace NetCoreAPI_Template_v2.DTOs.Product
{
    public class EditProductDto
    {
        [Required]
        public int Id { get; set; }

        [FirstLetterUpperCaseAttribute]
        public string Name { get; set; }
        public int Price { get; set; }
        public int StockCount { get; set; }
        
        [Required]
        public int ProductGroupId { get; set; }
    }
}