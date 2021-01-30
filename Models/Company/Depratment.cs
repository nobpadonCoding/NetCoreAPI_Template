using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v2.Models.Company
{
    [Table("Depratment")]
    public class Depratment
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}