using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Utilities;

namespace MovieService1.Models
{
    public class Movie : ModelTemplate
    {
        //public int Id { get; set; }
        public string Title { get; set; }
        //public string Description { get; set; }
        //public double Rating { get; set; }
        public DateTime Year { get; set; }
        public Genre Genre { get; set; }
        public int RunTime { get; set; }
        public int Sales { get; set; }


        private ICollection<FilmProfessional> _cast;
        private FilmProfessional _director;

        //overiding set and get for the movies cast
        public ICollection<FilmProfessional> Cast
        {
            get{ return _cast; }
            set
            {
                _cast = new HashSet<FilmProfessional>();
                foreach(var filmProfessional in value)
                {
                    _cast.Add(filmProfessional);
                }
            }
        }

        //overiding set and get for the movies director
        public FilmProfessional Director
        {
            get { return _director;}
            set { _director = value;}
        }

        //adds an actor to the cast of the movie and adds that movie
        //to the actors credits
        public void AddActor(FilmProfessional fp)
        {
            if(fp == null)
            {
                fp = new FilmProfessional();
            }
            Cast.Add(fp); 

            if(fp.jobs[(int)Jobs.Actor] == false)
            {
                fp.jobs[(int)Jobs.Actor] = true;
            }
            fp.ActingCredits.Add(this);

        }

        //adds a director to the movie and adds that movie to the 
        //directors credits
        public void AddDirector(FilmProfessional fp)
        {
            if(fp == null)
            {
                fp = new FilmProfessional();

            }
            Director = fp;

            if(fp.jobs[(int)Jobs.Director] == false)
            {
                fp.jobs[(int)Jobs.Director] = true;
            }
            fp.DirectingCredits.Add(this);

        }

    }
}