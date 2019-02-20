﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTable.API.Core.DTOs.FTable
{
    // Things that modify the data to be return
    public class FTableDataModifierDTO
    {
        public int PageSize { get; set; } // Page size to return
        public int currentPage { get; set; } // Page number
    
        public FSearchDTO Search { get; set; }
        public IEnumerable<FOrderDTO> Orders { get; set; }
        public IEnumerable<FFilterDTO> Filters { get; set; }
    }
}