using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using MovieService1.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace MovieService1.Controllers
{//Stephanie's original version
    public class MovieController: ODataController
    {
        //this controller uses the MoviesContext class to access
        //the database using Entity Frameworks
        MovieContext db = new MovieContext();
        private bool MovieExists(int key)
        {
            return db.Movies.Any(p => p.Key == key);
        }
        //this is the starting point for the controller.
        protected  override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //The [EnableQuery] attribute enables clients to modify
        //the query, by using query options such as $filter,
        //$sort, and $page.
        [EnableQuery]
        public IQueryable<Movie> Get()
        {
            return db.Movies;
        }
        [EnableQuery]
        public SingleResult<Movie> Get([FromODataUri] int key)
        {
            //The parameterless version of the Get method returns
            //the entire Movies collection. The Get method with a 
            //key parameter looks up a product by its key (in this 
            //case, the Id property).

            IQueryable<Movie> result = db.Movies.Where(p => p.Key == key);
            return SingleResult.Create(result);
        }


        //This is letting the client add a movie
        public async Task<IHttpActionResult> Post(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Movies.Add(movie);
            await db.SaveChangesAsync();
            return Created(movie);
        }



        //This is letting the client update an entry 
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Movie> movie)
        {
            //Delta<T> tracks the changes
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Movies.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            movie.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (MovieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }
        //this lets a client delete an entry
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var movie = await db.Movies.FindAsync(key);
            if (movie == null)
            {
                return NotFound();
            }
            db.Movies.Remove(movie);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}