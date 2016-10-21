using Autofac;
using Launchpad.Core;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Launchpad.Web.App_Start
{
    /// <summary>
    /// Configures the registrations for Autofac
    /// </summary>
    public class WebModule : Module
    {
        private string _connectionString;

        public WebModule(string connectionString)
        {
            _connectionString = connectionString.ThrowIfNull();
        }

        protected override void Load(ContainerBuilder builder)
        {
            var log = ConfigureLogging();
            builder.RegisterInstance(log)
                .As<ILogger>()
                .SingleInstance();
        }

        /// <summary>
        /// Configure the SQL server sink 
        /// </summary>
        /// <returns>ILogger interface (this will be registered as a singleton in the container)</returns>
        /// <remarks>Reference: https://github.com/serilog/serilog-sinks-mssqlserver</remarks>
        private ILogger ConfigureLogging()
        {
            var connectionString = _connectionString;  //Server=... or the name of a connection string in your .config file
            var tableName = "LaunchpadLogs";

            var columnOptions = new ColumnOptions();  // optional
            columnOptions.Store.Add(StandardColumn.LogEvent); //Store the JSON too 
           

            var log = new LoggerConfiguration()
                .WriteTo.MSSqlServer(connectionString, tableName, columnOptions: columnOptions)
                .CreateLogger();

            Log.Logger = log; //Configure the global static logger
            return log;
        }
    }
}