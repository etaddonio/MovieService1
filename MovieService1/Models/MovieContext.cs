using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MovieService1.Models
{
    public class MovieContext :DbContext
    {
        public MovieContext()
        
            : base("name=MovieContext")
        {

        }
        public DbSet<Movie> Movies { get; set; }
        
    }
}