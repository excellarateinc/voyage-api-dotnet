using System;
using System.Data.Entity;
using System.Linq;
using Autofac;
using Autofac.Core;
using Launchpad.Core;
using Launchpad.Data.Auditing;
using Launchpad.Data.Interfaces;
using Launchpad.Data.Stores;
using Launchpad.Models;
using Launchpad.Models.EntityFramework;
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

        /// <summary>
        /// Scan the assembly for the audit configuration and call configure
        /// </summary>
        protected void ConfigureAuditing()
        {
            GlobalTrackingConfig
                .SetSoftDeletableCriteria<ISoftDeleteable>(_ => _.Deleted);

            var auditing = typeof(IAuditConfiguration)
                .Assembly
                .GetTypes()
                .Where(_ => _.IsAssignableTo<IAuditConfiguration>() && !_.IsAbstract)
                .Select(_ => Activator.CreateInstance(_) as IAuditConfiguration);

            foreach (var config in auditing)
            {
                config.Configure();
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Configure the auditing for the context
            ConfigureAuditing();

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

            builder.RegisterType<UnitOfWork>()
                .AsImplementedInterfaces()
                .InstancePerRequest();
        }
    }
}
