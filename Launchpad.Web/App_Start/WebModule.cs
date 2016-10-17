using Autofac;
using Launchpad.Services;

namespace Launchpad.Web.App_Start
{
    /// <summary>
    /// Configures the registrations for Autofac
    /// </summary>
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //To keep things simple, we will use a single module that registers the components for all the 
            //libaries. In the future, this can be split into a module per library. However, the per request 
            //registration will need to be passed down to each of those libraries.
            //Reference: http://docs.autofac.org/en/latest/faq/per-request-scope.html

            LoadService(builder);
        }

        private void LoadService(ContainerBuilder builder)
        {
            //This will register the type WidgetService as it's implemented interfaces. In this case, dependencies on IWidgetService will resolve to a concrete
            //instance of WidgetService
            builder.RegisterType<WidgetService>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

        }
    }
}