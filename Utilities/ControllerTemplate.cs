using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;

namespace Utilities
{
    public abstract class ControllerTemplate<T1, T2> : ODataController
        where T1 : ModelTemplate where T2 : DbContext, new()
    {
        private DbSet<T1> DbSet { get; }
        private T2 DbContext { get; }

        protected ControllerTemplate(string setName)
        {
            DbContext = new T2();
            if (!DbContext.HasProperty(setName))
                throw new Exception(typeof(T2) + " does not contain the property " + setName);
            DbSet = DbSet.GetValue(setName) as DbSet<T1>;
        }

        [HttpGet]
        [EnableQuery]
        public virtual IHttpActionResult Get()
        {
            return OnGetAll();
        }

        [HttpGet]
        [EnableQuery]
        public virtual IHttpActionResult Get([FromODataUri] int key)
        {
            return OnGetSingle(key);
        }

        [HttpPost]
        public virtual IHttpActionResult Post(T1 t)
        {
            return OnPost(t);
        }

        [HttpPatch]
        public virtual IHttpActionResult Patch([FromODataUri] int key, Delta<T1> patch)
        {
            return OnPatch(key, patch);
        }

        [HttpDelete]
        public virtual IHttpActionResult Delete([FromODataUri] int key)
        {
            return OnDelete(key);
        }

        protected override void Dispose(bool disposing)
        {
            DbContext.Dispose();
            base.Dispose(disposing);
        }

        protected IHttpActionResult OnGetAll()
        {
            return Ok(DbSet.AsQueryable());
        }

        protected IHttpActionResult OnGetSingle([FromODataUri] int key)
        {
            var entities = DbSet.Where(entity => entity.Key == key);
            return entities.Any() ? (IHttpActionResult) Ok(SingleResult.Create(entities)) : NotFound();
        }

        protected IHttpActionResult OnPost(T1 t)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            DbSet.Add(t);
            DbContext.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected IHttpActionResult OnPatch([FromODataUri] int key, Delta<T1> patch)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entity = DbSet.FirstOrDefault(e => e.Key == key);
            if (entity == null) return NotFound();
            patch.Patch(entity);
            DbContext.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected IHttpActionResult OnDelete([FromODataUri] int key)
        {
            var entity = DeleteOperation(key, DbSet);
            if (entity == null) return NotFound();
            DbSet.Remove(entity);
            DbContext.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected IHttpActionResult OnGetCollection([FromODataUri] int key)
        {
            
        }

        protected abstract T1 DeleteOperation([FromODataUri] int key, DbSet<T1> dbSet);
    }
}