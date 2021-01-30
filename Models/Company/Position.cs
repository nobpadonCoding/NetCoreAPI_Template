using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v2.Models.Company
{
    [Table("Position")]
    public class Position
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Employee> Employees { get; set; }
    }
}