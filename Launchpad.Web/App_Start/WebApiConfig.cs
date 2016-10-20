using Launchpad.Services;
using Launchpad.Web.App_Start;
using Launchpad.Web.Filters;
using Launchpad.Web.Handlers;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using Autofac;
using Launchpad.Services.Interfaces;

namespace Launchpad.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //Service locator - not ideal
             var metricsService = config.DependencyResolver.GetService(typeof(IRequestMetricsService)) as IRequestMetricsService;
            config.MessageHandlers.Add(new RequestMetricsHandler(metricsService));

            config.Filters.Add(new ValidateModelAttribute()); //Globally configure model validation

            //Set camelcasing on for JSON
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
