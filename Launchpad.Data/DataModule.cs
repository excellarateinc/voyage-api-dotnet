using Autofac;
using Launchpad.Data.Interfaces;

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
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<SqlConnectionStatus>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("displayName", "Launchpad Database")
                .AsImplementedInterfaces()
                .InstancePerRequest();

            //Shortcut to register all repositories using a marker interface
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IRepository>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

        }

    }
}
