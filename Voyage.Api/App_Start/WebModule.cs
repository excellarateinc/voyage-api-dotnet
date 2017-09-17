using Autofac;
using Autofac.Integration.WebApi;
using Voyage.Core;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json;
using Voyage.Api.AuthProviders;
using Voyage.Api.Middleware;
using Voyage.Api.Middleware.Processors;
using Voyage.Models.Entities;
using Voyage.Services.IdentityManagers;
using Voyage.Services.Notification.Push;

namespace Voyage.Api
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

            ConfigureSignalR(builder);
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

            //// Options
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

        private static void ConfigureSignalR(ContainerBuilder builder)
        {
            // Configure a contract resolver to ensure objects are serialized with camel case.
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new SignalRContractResolver()
            };
            var serializer = JsonSerializer.Create(settings);
            builder.Register(c => serializer).As<JsonSerializer>();

            // Custom user id provider to map connections to the user.
            builder.Register(c => new SignalRUserIdProvider(HttpContext.Current.GetOwinContext().Authentication)).As<IUserIdProvider>();
        }
    }
}