using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MovieService1.Models;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace MovieService1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {


            // Web API routes
            config.MapHttpAttributeRoutes();

            // Web API configuration and services
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            //Creates and Entity Data Model
            builder.EntitySet<Movie>("Movies");
            //adds a route
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: builder.GetEdmModel());

           

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
