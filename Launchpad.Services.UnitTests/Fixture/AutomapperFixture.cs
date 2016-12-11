using System;
using AutoMapper;
using Launchpad.Models.Map;

namespace Launchpad.Services.UnitTests.Fixture
{
    public class AutoMapperFixture
    {
        private readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(() =>
        {
            var instance = MappingConfig.ConfigureMapper();
            return instance;
        });

        public IMapper MapperInstance => _mapper.Value;
    }
}
