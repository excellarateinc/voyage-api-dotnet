using Autofac;
using Autofac.Core;
using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Launchpad.Data
{
    public class DataModule : Module
    {
        private string _connectionString;

        /// <summary>
        /// Creates a new data module for registration with autofac
        /// </summary>
        /// <param name="connectionString">The connection string to use with the DataContext registration</param>
        public DataModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //Register a data context with an instance per request
            //Dependency Type: ILaunchpadDataContext
            //Concrete Type: LaunchpadDataContext
            builder.RegisterType<LaunchpadDataContext>()
                .WithParameter("connectionString", _connectionString)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerRequest();

           
            //Shortcut to register all repositories using a marker interface
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IRepository>()
                .AsImplementedInterfaces()
                .InstancePerRequest();


            //Register the user store (wrapper around the identity tables)
            builder.RegisterType<UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>>()
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(DbContext), (pi, ctx) => ctx.Resolve<LaunchpadDataContext>()))
                .As<IUserStore<ApplicationUser, string>>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<RoleStore<ApplicationRole>>()
               .WithParameter(new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(DbContext), (pi, ctx) => ctx.Resolve<LaunchpadDataContext>()))
               .As<IRoleStore<ApplicationRole, string>>()
               .InstancePerRequest();


        }

    }
}
