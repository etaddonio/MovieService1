using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Utilities;

namespace MovieService1.Models
{
    public class Person : ModelTemplate
    {
        [Required]
        string name { get; set; }
        int age { get; set; }
        Gender gender { get; set; }

    }
}