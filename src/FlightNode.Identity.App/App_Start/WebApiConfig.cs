using FlightNode.Identity.App;
using FlightNode.Identity.Services.Filters;
using FlightNode.Identity.Services.Providers;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace FligthNode.Identity.App
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config = ConfigureRoutes(config);
            config = ConfigureFilters(config);
            config.DependencyResolver = UnityConfig.RegisterComponents();
            
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
        }

        private static HttpConfiguration ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            return config;
        }

        private static HttpConfiguration ConfigureFilters(HttpConfiguration config)
        {
            config.Filters.Add(new NotImplementedExceptionAttribute());
            config.Filters.Add(new UnhandledExceptionFilterAttribute());

            return config;
        }


    }
}
