using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI_Template_v2.Models.Company
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Employee> Employees { get; set; }

    }
}