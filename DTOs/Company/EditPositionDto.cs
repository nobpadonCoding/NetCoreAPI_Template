using System.ComponentModel.DataAnnotations;
using NetCoreAPI_Template_v2.Validations;

namespace NetCoreAPI_Template_v2.DTOs.Company
{
    public class EditPositionDto
    {
        [Required]
        public int Id { get; set; }

        [FirstLetterUpperCaseAttribute]
        public string PositionDescription { get; set; }
    }
}