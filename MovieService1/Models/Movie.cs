using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieService1.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public DateTime Year { get; set; }

    }
}