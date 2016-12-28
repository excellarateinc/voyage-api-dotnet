using System.Data.Entity;
using Autofac;
using Autofac.Core;
using Launchpad.Core;
using Launchpad.Data.Interfaces;
using Launchpad.Data.Stores;
using Launchpad.Models;
using Launchpad.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TrackerEnabledDbContext.Common.Configuration;

namespace Launchpad.Data
{
    public class DataModule : Module
    {
        private readonly string _connectionString;

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
            // Configure the auditing for the context
            GlobalTrackingConfig.SetSoftDeletableCriteria<ISoftDeleteable>(_ => _.Deleted);
            BaseAuditConfiguration.Configure();

            // Register a data context with an instance per request
            // Dependency Type: ILaunchpadDataContext
            // Concrete Type: LaunchpadDataContext
            builder.RegisterType<LaunchpadDataContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(IIdentityProvider), (pi, ctx) => ctx.Resolve<IIdentityProvider>()))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            // Shortcut to register all repositories using a marker interface
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IRepository>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            // Register the user store (wrapper around the identity tables)
            builder.RegisterType<CustomUserStore>()
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
