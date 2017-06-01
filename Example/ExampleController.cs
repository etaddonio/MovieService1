using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ODataHelper;

namespace Example
{
    public class ExampleController : ControllerBase<ExampleModel, ExampleContext>
    {
        public ExampleController()
        {
            //Create a new context
            var exampleContext = new ExampleContext();
            
            //Call the initalize function and pass in the database set, database context, and then any get property functions
            //The get property functions are optional, but must be set up correctly to use them (see below)
            Initialize(exampleContext.ExampleModels, exampleContext, Collection);
            
            // *** IF YOU DO NOT CALL INITIALIZE ALL REQUESTS WILL RETURN NOT FOUND ***
        }

        //To get a property, override the GetProperty function
        //Add the [HttpGet] and [EnableQuery] Attributes
        //For each property, add a new OData Route
        [HttpGet]
        [ODataRoute("Examples({key})/Collection")] // <- Note how the route goes to 'Collection' which has the same name as the GetPropertyFunction below
        [EnableQuery]
        public override IHttpActionResult GetProperty(int key)
        {
            //Use the OnGetProperty function to get the property
            return OnGetProperty(key);
        }

        protected override ExampleModel DeleteOperation(int key, ExampleContext exampleContext)
        {
            //Find the entity within the given database set
            var removeExampleModel = exampleContext.ExampleModels.Include("Collection").FirstOrDefault(e => e.Key == key);
            
            //Return null if no entity exists
            if (removeExampleModel == null) return null;
            
            //Find all references to this entity
            var exampleModels = exampleContext.ExampleModels.Include("Collection")
                .Where(c => c.Collection.Select(e => e.Key).AsQueryable().Contains(key)).ToList();
            
            //Remove entity from those locations
            foreach (var exampleModel in exampleModels)
            {
                exampleModel.Collection.Remove(removeExampleModel);
            }
            
            //Return the entity
            return removeExampleModel;
            
            // *** DO NOT REMOVE THE ENTITY FROM THE DATABASE CONTEXT OR MODIFY THE DATABASE CONTEXT IN ANY WAY ***
        }

        //Gets the property 'Collection' from the given exampleModel object.
        //The name of this function needs to be the same as the OData Route
        //For example, '/Foo' will not work if the function is named 'Bar'
        private IHttpActionResult Collection(ExampleModel exampleModel, ExampleContext exampleContext)
        {
            return Ok(exampleModel.Collection);
        }
    }
}