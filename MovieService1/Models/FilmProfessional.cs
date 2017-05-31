using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities;

namespace MovieService1.Models
{
    public class FilmProfessional : Person
    {
        

        public ICollection<Movie> ActingCredits { get; set; }
        public ICollection<Movie> DirectingCredits { get; set; }
        //public double NetWorth { get; set; }

        //creates a boolean array with same number of values as values in the Jobs enum
        public bool[] jobs { get; } = new bool[Enum.GetNames(typeof(Jobs)).Length];


        
    }
}