using Autofac;
using AutoMapper;

namespace Launchpad.Models.Map
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var instance = MappingConfig.ConfigureMapper();

            builder.RegisterInstance(instance)
                .SingleInstance();
        }

        
    }
}
