
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using Launchpad.Services;
using Launchpad.Data;
using System.Configuration;
using Launchpad.Models.Map;
using System.Web.Http;

namespace Launchpad.Web.App_Start
{
    /// <summary>
    /// This is vanilla autofac configuration based on:
    /// http://docs.autofac.org/en/latest/integration/webapi.html#quick-start
    /// </summary>
    public class ContainerConfig
    {

        public static IContainer Container { get; private set; }

        public static void Register(HttpConfiguration httpConfig)
        {
            var builder = new ContainerBuilder();

            //Register the types in the container
            var connectionString = ConfigurationManager.ConnectionStrings["LaunchpadDataContext"].ConnectionString;
            builder.RegisterModule(new DataModule(connectionString));
            builder.RegisterModule<AutoMapperModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule(new WebModule(connectionString));
            
            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(httpConfig);

            // Set the dependency resolver to be Autofac.
            Container = builder.Build();
            
            httpConfig.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }
    }
}