using Autofac;
using Launchpad.Services.IdentityManagers;
using System.Net.Http;

namespace Launchpad.Services
{
    public class ServicesModule : Module
    {
        /* Note: If this module is shared with an application that does not have a request lifecycle, these registrations
         * need to be updated (Instance Per Request will cause an error) See the reference link for options
         * Reference: http://docs.autofac.org/en/latest/faq/per-request-scope.html
         * */
        protected override void Load(ContainerBuilder builder)
        {
            //This will register the type WidgetService as it's implemented interfaces. In this case, dependencies on IWidgetService will resolve to a concrete
            //instance of WidgetService
            builder.RegisterType<WidgetService>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<ApplicationInfoService>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<ConfigurationManagerService>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<FileReaderService>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<PathProviderService>()
                .AsImplementedInterfaces()
                .WithParameter("localPath", System.Web.HttpContext.Current.Server.MapPath("~"));

            //Let the container dispose of it at the end of the request    
            builder.RegisterType<HttpClient>()
                .InstancePerRequest();        

            builder.RegisterType<UserService>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<RoleService>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<ApplicationUserManager>()
               .AsSelf()
               .InstancePerRequest();

            builder.RegisterType<ApplicationRoleManager>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<AuditService>()
                .AsImplementedInterfaces()
                .InstancePerRequest();
        }
    }
}
