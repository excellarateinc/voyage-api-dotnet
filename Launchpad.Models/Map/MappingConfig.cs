using AutoMapper;

namespace Launchpad.Models.Map
{
    public static class MappingConfig
    {
        public static void ConfigureMapper()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfiles(typeof(MappingConfig).Assembly);
            });
        }
    }
}
