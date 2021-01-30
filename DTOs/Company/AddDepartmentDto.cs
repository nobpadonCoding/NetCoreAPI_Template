using NetCoreAPI_Template_v2.Validations;

namespace NetCoreAPI_Template_v2.DTOs.Company
{
    public class AddDepartmentDto
    {
         [FirstLetterUpperCaseAttribute]
        public string DepartmentDescription { get; set; }
    }
}