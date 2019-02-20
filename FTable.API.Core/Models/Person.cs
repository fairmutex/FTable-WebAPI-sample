
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FTable.API.Core.Models
{
    public class Person
    {
        // [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Status { get; set; } = "Enabled";
        public string Picture { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}