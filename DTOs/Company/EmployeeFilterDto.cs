namespace NetCoreAPI_Template_v2.DTOs.Company
{
    public class EmployeeFilterDto : PaginationDto
    {
        public string EmployeeName { get; set; }
        public string EmployeeDepartment { get; set; }

        //Ordering
        public string OrderingField { get; set; }
        public bool AscendingOrder { get; set; } = true;
    }
}