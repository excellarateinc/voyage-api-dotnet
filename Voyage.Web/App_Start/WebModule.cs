using Autofac;
using Autofac.Integration.WebApi;
using Voyage.Core;
using Voyage.Services.IdentityManagers;
using Voyage.Web.AuthProviders;
using Voyage.Web.Middleware;
using Voyage.Web.Middleware.Processors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Voyage.Models.Entities;

namespace Voyage.Web
{
    /// <summary>
    /// Configures the registrations for Autofac
    /// </summary>
    public class WebModule : Module
    {
        private readonly string _connectionString;
        private readonly HttpConfiguration _httpConfig;

        public WebModule(string connectionString, HttpConfiguration httpConfig)
        {
            _httpConfig = httpConfig.ThrowIfNull(nameof(httpConfig));
            _connectionString = connectionString.ThrowIfNull(nameof(connectionString));
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Configure and register Serilog
            var log = ConfigureLogging();
            builder.RegisterInstance(log)
                .As<ILogger>()
                .SingleInstance();

            builder.RegisterHttpRequestMessage(_httpConfig);

            // Register Identity services
            ConfigureIdentityServices(builder);

            ConfigureMiddleware(builder);
        }

        private void ConfigureMiddleware(ContainerBuilder builder)
        {
            builder.RegisterType<ErrorResponseProcessor>()
                .InstancePerRequest()
                .AsSelf();

            builder.RegisterType<ActivityAuditMiddleware>()
                .InstancePerRequest()
                .AsSelf();
        }

        private void ConfigureIdentityServices(ContainerBuilder builder)
        {
            // Register the owin context - this allows non-web api services
            // access to the hosting
            builder.Register(c => c.Resolve<HttpRequestMessage>().GetOwinContext())
                .As<IOwinContext>()
                .InstancePerRequest();

            // Configure the concrete implementation of the
            // IdentityProvider.
            builder.RegisterType<IdentityProvider>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<ApplicationJwtProvider>()
                .AsSelf()
                .SingleInstance();

            // Options
            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>
            {
                DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider(Constants.ApplicationName)
            });

            builder.Register(c =>
            {
                IUserTokenProvider<ApplicationUser, string> tokenProvider = null;
                var options = c.Resolve<IdentityFactoryOptions<ApplicationUserManager>>();
                var dataProtectionProvider = options.DataProtectionProvider;
                if (options.DataProtectionProvider != null)
                {
                    tokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create(Constants.ApplicationName));
                }

                return tokenProvider;
            });

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();
        }

        /// <summary>
        /// Configure the SQL server sink
        /// </summary>
        /// <returns>ILogger interface (this will be registered as a singleton in the container)</returns>
        /// <remarks>Reference: https://github.com/serilog/serilog-sinks-mssqlserver</remarks>
        private ILogger ConfigureLogging()
        {
            // Server=... or the name of a connection string in your .config file
            var connectionString = _connectionString;
            const string tableName = "dbo.ApplicationLog";

            var columnOptions = new ColumnOptions();  // optional
            columnOptions.Store.Add(StandardColumn.LogEvent); // Store the JSON too

            var log = new LoggerConfiguration()
                .WriteTo.MSSqlServer(connectionString, tableName, columnOptions: columnOptions)
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));

            Log.Logger = log; // Configure the global static logger
            return log;
        }
    }
}