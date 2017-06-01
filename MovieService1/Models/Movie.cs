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
        public string Cast { get; set; }
        public string Director { get; set; }
  
        public double Rating { get; set; }
        public int RunTime { get; set; }
        public int Sales { get; set; }
       
    }
    public class FilmProfessional
    {
        public string ActingCredits { get; set; }
        public string DirectingCredits { get; set; }
        public string jobs  { get; set; }
    }
}