﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAPI_Template_v2.DTOs
{
    public class PaginationResultDto
    {
        public double TotalAmountRecords { get; set; }
        public double TotalAmountPages { get; set; }
        public double CurrentPage { get; set; }
        public double RecordsPerPage { get; set; }
    }
}