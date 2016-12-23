using Autofac;
using Launchpad.Services.IdentityManagers;
using System.Configuration;
using Launchpad.Services.ApplicationInfo;
using Launchpad.Services.Audit;

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
            builder.RegisterType<ApplicationInfoService>()
                .WithParameter("fileName", ConfigurationManager.AppSettings["ApplicationInfoFileName"])
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<FileReaderService>()
                .AsImplementedInterfaces()
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
