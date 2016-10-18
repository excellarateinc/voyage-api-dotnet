using Autofac;
using AutoMapper;

namespace Launchpad.Models.Map
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            MappingConfig.ConfigureMapper();

            builder.RegisterInstance(Mapper.Instance)
                .SingleInstance();
        }

        
    }
}
