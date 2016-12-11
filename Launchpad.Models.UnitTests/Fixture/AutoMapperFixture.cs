using AutoMapper;
using Launchpad.Models.Map;
using System;

namespace Launchpad.Models.UnitTests.Fixture
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
