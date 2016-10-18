using AutoMapper;

namespace Launchpad.Models.Map
{
    public static class MappingConfig
    {
        public static IMapper ConfigureMapper()
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(MappingConfig).Assembly);
            });

            return config.CreateMapper();
        }
    }
}
