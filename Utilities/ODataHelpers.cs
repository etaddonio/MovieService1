using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;

namespace Utilities
{
    public static class ODataHelpers
    {
        public static bool HasProperty(this object instance, string property)
        {
            return instance.GetType().GetProperty(property) != null;
        }

        public static object GetValue(this object instance, string property)
        {
            var propertyInfo = instance.GetType().GetProperty(property);
            if (propertyInfo == null)
            {
                throw new HttpException("Can't find property: " + property);
            }
            return propertyInfo.GetValue(instance, new object[] { });
        }

        public static IHttpActionResult CreateOk(this ODataController controller, object value)
        {
            var methods = controller.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            var okMethod =
                methods.FirstOrDefault(method => method.Name.Equals("Ok") && method.GetParameters().Length == 1);
            if (okMethod == null) return new NotFoundResult(controller);
            okMethod = okMethod.MakeGenericMethod(value.GetType());
            return (IHttpActionResult) okMethod.Invoke(controller, new object[] {value});
        }
    }
}