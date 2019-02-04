using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTable.WebAPI.FW.DTOs.FTable
{
    public class FTableResultDTO
    {
        // Total rows of data
        public int totalRows { get; set; }
        // Total rows after After Searching/filtering
        public int totalRowsAfterModifications { get; set; }
        // TODO
        // filter Data not sure how to deal with it right now
        public IEnumerable<dynamic> FilterData {get;set;}
        // Data
        public IEnumerable<dynamic> Page { get; set; }
    }
}