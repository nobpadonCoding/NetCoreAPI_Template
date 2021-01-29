using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v2.Models.Product
{
    [Table("Product")]
    public class Product
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int StockCount { get; set; }
        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
    }
}