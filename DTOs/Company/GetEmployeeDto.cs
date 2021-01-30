using NetCoreAPI_Template_v2.Models.Company;

namespace NetCoreAPI_Template_v2.DTOs.Company
{
    public class GetEmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PositionDescription { get; set; }
        public string DepartmentDescription { get; set; }
    }
}