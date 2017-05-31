using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities;

namespace MovieService1.Models
{
    public class User : ModelTemplate
    {
        public Person person { get; set; }
        public int PermissionKey { get; set; }
    }
}