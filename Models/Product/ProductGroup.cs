using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v2.Models.Product
{
    [Table("ProductGroup")]
    public class ProductGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // public List<Product> Products { get; set; }
    }
}