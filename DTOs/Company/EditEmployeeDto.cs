using System.ComponentModel.DataAnnotations;
using NetCoreAPI_Template_v2.Validations;

namespace NetCoreAPI_Template_v2.DTOs.Company
{
    public class EditEmployeeDto
    {
        [Required]
        public int Id { get; set; }

        [FirstLetterUpperCaseAttribute]
        [Required]
        public string Name { get; set; }

        [FirstLetterUpperCaseAttribute]
        [Required]
        public string LastName { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}