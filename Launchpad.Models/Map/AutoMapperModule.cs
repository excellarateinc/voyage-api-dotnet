using Autofac;

namespace Launchpad.Models.Map
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var instance = MappingConfig.ConfigureMapper();

            // The mapper can be shared for the lifetime of the application, register it as a singleton 
            builder.RegisterInstance(instance)
                .SingleInstance();
        }        
    }
}
