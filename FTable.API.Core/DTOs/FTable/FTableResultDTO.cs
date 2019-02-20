using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTable.API.Core.DTOs.FTable
{

    // Data to be Returned
    public class FTableResultDTO
    {
        // Total rows of data
        public int totalRows { get; set; }
        // Total rows after After Searching/filtering
        public int totalRowsAfterModifications { get; set; }
        public IEnumerable<dynamic> FilterData {get;set;}
        public IEnumerable<dynamic> Page { get; set; }
    }
}