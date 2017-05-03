using System;
using AutoMapper;
using Voyage.Models.Map;

namespace Voyage.Services.UnitTests.Common.AutoMapperFixture
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
