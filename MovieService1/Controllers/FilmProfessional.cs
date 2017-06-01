using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieService1.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace MovieService1.Controllers
{
    public class FilmProfessional : ODataController
    {
        MovieContext db = new MovieContext();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // Lets you query by Actor
        //[EnableQuery]
            //public SingleResult<FilmProfessional> GetActor([FromODataUri] int key)
            //{
            //    var result = db.FilmProfessional.Where(m => m.Id == key).Select(m => m.cast);

            //    )
            //    return SingleResult.Create(result);
            //}
    }
}