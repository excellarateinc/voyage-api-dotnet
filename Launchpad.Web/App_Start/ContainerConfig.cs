
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace Launchpad.Web.App_Start
{
    public class ContainerConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            //Register the types in the container
            builder.RegisterModule<WebModule>();

            // Get your HttpConfiguration.
            var config = System.Web.Http.GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();


            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}