namespace NetCoreAPI_Template_v2.DTOs.Bulk
{
    public class BulkFilterDto : PaginationDto
    {
        public string BulkName { get; set; }
        public string BulkCode { get; set; }

        //Ordering
        public string OrderingField { get; set; }
        public bool AscendingOrder { get; set; } = true;
    }
}