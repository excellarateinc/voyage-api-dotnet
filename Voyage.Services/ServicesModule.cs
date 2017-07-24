using System.Configuration;
using Autofac;
using Voyage.Services.ApplicationInfo;
using Voyage.Services.Audit;
using Voyage.Services.Client;
using Voyage.Services.FileReader;
using Voyage.Services.IdentityManagers;
using Voyage.Services.KeyContainer;
using Voyage.Services.Notification;
using Voyage.Services.PasswordRecovery;
using Voyage.Services.Phone;
using Voyage.Services.Role;
using Voyage.Services.User;

namespace Voyage.Services
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
                .As<IFileReaderService>()
                .InstancePerRequest();

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerRequest();

            builder.RegisterType<ClientService>()
                .As<IClientService>()
                .InstancePerRequest();

            builder.RegisterType<PhoneService>()
                .As<IPhoneService>()
                .InstancePerRequest();

            builder.RegisterType<PasswordRecoverService>()
                .As<IPasswordRecoverService>()
                .InstancePerRequest();

            builder.RegisterType<RoleService>()
                .As<IRoleService>()
                .InstancePerRequest();

            builder.RegisterType<RsaKeyContainerService>()
                .As<IRsaKeyContainerService>()
                .SingleInstance();

            builder.RegisterType<ApplicationUserManager>()
               .AsSelf()
               .InstancePerRequest();

            builder.RegisterType<ApplicationRoleManager>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<AuditService>()
                .As<IAuditService>()
                .InstancePerRequest();

            builder.RegisterType<NotificationService>()
                .As<INotificationService>()
                .InstancePerRequest();
        }
    }
}
