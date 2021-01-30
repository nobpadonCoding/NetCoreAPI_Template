using System.ComponentModel.DataAnnotations;
using NetCoreAPI_Template_v2.Validations;

namespace NetCoreAPI_Template_v2.DTOs.Company
{
    public class EditDepartmentDto
    {
        [Required]
        public int Id { get; set; }

        [FirstLetterUpperCaseAttribute]
        public string DepartmentDescription { get; set; }
    }
}