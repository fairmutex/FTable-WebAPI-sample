using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTable.WebAPI.FW.DTOs.FTable
{
    public class FSearchDTO
    {
        public string Value { get; set; }
        public bool IsRegex { get; set; }
        public bool IsInverse { get; set; }
    }
}