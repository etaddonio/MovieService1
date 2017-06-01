using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;

namespace ODataHelper
{
    public abstract class ControllerBase<T1, T2> : ODataController where T1 : ModelBase where T2 : DbContext
    {
        /// <summary>
        /// Database Set of this Controller.
        /// </summary>
        private DbSet<T1> DbSet { get; set; }

        /// <summary>
        /// The database context that contains the given database set.
        /// </summary>
        private T2 DbContext { get; set; }

        /// <summary>
        /// Has this controller been initialized?
        /// </summary>
        private bool Initialized { get; set; }

        /// <summary>
        /// Collection of GetPropertyFucntion delegates.
        /// </summary>
        private Dictionary<string, GetPropertyFucntion> PropertyFucntions { get; set; }

        /// <summary>
        /// Gets a property of the given entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="dbContext">Database Context</param>
        protected delegate IHttpActionResult GetPropertyFucntion(T1 entity, T2 dbContext);

        /// <summary>
        /// Initalizes the controller.
        /// </summary>
        /// <param name="dbSet">Database Set</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="getCollectionProperties">Get Property Fucntions</param>
        protected void Initialize(DbSet<T1> dbSet, T2 dbContext, params GetPropertyFucntion[] getCollectionProperties)
        {
            if (Initialized) return;
            Initialized = true;
            DbSet = dbSet;
            DbContext = dbContext;
            PropertyFucntions = new Dictionary<string, GetPropertyFucntion>();
            foreach (var getCollectionProperty in getCollectionProperties)
            {
                PropertyFucntions[getCollectionProperty.Method.Name] = getCollectionProperty;
            }
        }

        /// <summary>
        /// Gets all of the entities in the database set.
        /// </summary>
        /// <returns>Entities</returns>
        [HttpGet]
        [EnableQuery]
        public virtual IHttpActionResult Get()
        {
            return OnGetAll();
        }

        /// <summary>
        /// Gets a single entity with the given key from the database set, if it exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Entity</returns>
        [HttpGet]
        [EnableQuery]
        public virtual IHttpActionResult Get([FromODataUri] int key)
        {
            return OnGetSingle(key);
        }

        /// <summary>
        /// Adds the given entity to the database set.
        /// </summary>
        /// <param name="t">Entity</param>
        /// <returns>Response</returns>
        [HttpPost]
        public virtual IHttpActionResult Post(T1 t)
        {
            return OnPost(t);
        }

        /// <summary>
        /// Updates the entity with the given key, if it exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="patch">Partial Entity</param>
        /// <returns>Response</returns>
        [HttpPatch]
        public virtual IHttpActionResult Patch([FromODataUri] int key, Delta<T1> patch)
        {
            return OnPatch(key, patch);
        }

        /// <summary>
        /// Replaces the entity with the given key, if it exists, with the given entity.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="t">Entity</param>
        /// <returns>Response</returns>
        [HttpPut]
        public virtual IHttpActionResult Put([FromODataUri] int key, T1 t)
        {
            return OnPut(key, t);
        }

        /// <summary>
        /// Deletes the entity with the given key from the database set, if the entity exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Response</returns>
        [HttpDelete]
        public virtual IHttpActionResult Delete([FromODataUri] int key)
        {
            return OnDelete(key);
        }

        /// <summary>
        /// Additional routing for getting properties.  Must contain correct attributes with paring GetPropertyFunction delegates.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Property</returns>
        public virtual IHttpActionResult GetProperty([FromODataUri] int key)
        {
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            DbContext.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets all of the entities in the database set.
        /// </summary>
        /// <returns>Entities</returns>
        protected IHttpActionResult OnGetAll()
        {
            if (!Initialized) return NotFound();
            return Ok(DbSet.AsQueryable());
        }

        /// <summary>
        /// Gets a single entity with the given key from the database set, if it exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Entity</returns>
        protected IHttpActionResult OnGetSingle(int key)
        {
            if (!Initialized) return NotFound();
            var entities = DbSet.Where(entity => entity.Key == key);
            return entities.Any() ? (IHttpActionResult) Ok(SingleResult.Create(entities)) : NotFound();
        }

        /// <summary>
        /// Adds the given entity to the database set.
        /// </summary>
        /// <param name="t">Entity</param>
        /// <returns>Response</returns>
        protected IHttpActionResult OnPost(T1 t)
        {
            if (!Initialized) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            DbSet.Add(t);
            DbContext.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the entity with the given key, if it exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="patch">Partial Entity</param>
        /// <returns>Response</returns>
        protected IHttpActionResult OnPatch(int key, Delta<T1> patch)
        {
            if (!Initialized) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entity = DbSet.FirstOrDefault(e => e.Key == key);
            if (entity == null) return NotFound();
            patch.Patch(entity);
            DbContext.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        /// <summary>
        /// Replaces the entity with the given key, if it exists, with the given entity.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="t">Entity</param>
        /// <returns>Response</returns>
        protected IHttpActionResult OnPut(int key, T1 t)
        {
            if (!Initialized) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entity = DbSet.FirstOrDefault(e => e.Key == key);
            if (entity == null) return NotFound();
            t.Key = entity.Key;
            DbContext.Entry(entity).CurrentValues.SetValues(t);
            DbContext.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the entity with the given key from the database set, if the entity exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Response</returns>
        protected IHttpActionResult OnDelete(int key)
        {
            if (!Initialized) return NotFound();
            var entity = DeleteOperation(key, DbContext);
            if (entity == null) return NotFound();
            DbSet.Remove(entity);
            DbContext.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Additional routing for getting properties.  Must contain correct attributes with paring GetPropertyFunction delegates.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Property</returns>
        protected IHttpActionResult OnGetProperty(int key)
        {
            if (!Initialized) return NotFound();
            var propertyName = Url.Request.RequestUri.Segments.Last();
            var entity = DbSet.Include(propertyName).FirstOrDefault(e => e.Key == key);
            if (entity == null) return NotFound();
            GetPropertyFucntion getPropertyFucntion;
            return PropertyFucntions.TryGetValue(propertyName, out getPropertyFucntion)
                ? getPropertyFucntion.Invoke(entity, DbContext)
                : NotFound();
        }

        /// <summary>
        /// Finds the entity with the given key and removes all references of it within other classes.
        /// Do not remove the entity from the given database set.
        /// Return the entity with the given key, if it exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="dbContext">Database Context</param>
        /// <returns>Entity</returns>
        protected abstract T1 DeleteOperation(int key, T2 dbContext);
    }
}