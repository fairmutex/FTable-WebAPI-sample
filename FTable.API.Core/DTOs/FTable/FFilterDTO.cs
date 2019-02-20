using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTable.API.Core.DTOs.FTable
{
    public class FFilterDTO
    {
        public string ColumnName { get; set; }
        public string Type { get; set; }
        public JObject Apply { get; set; }
    }
}