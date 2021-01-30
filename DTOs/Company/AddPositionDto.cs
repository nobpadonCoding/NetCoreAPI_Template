using NetCoreAPI_Template_v2.Validations;

namespace NetCoreAPI_Template_v2.DTOs.Company
{
    public class AddPositionDto
    {
        [FirstLetterUpperCaseAttribute]
        public string PositionDescription { get; set; }
    }
}