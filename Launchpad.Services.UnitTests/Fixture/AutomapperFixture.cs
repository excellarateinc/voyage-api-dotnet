using AutoMapper;
using Launchpad.Models.Map;
using System;

namespace Launchpad.Services.Fixture
{
    public class AutoMapperFixture
    {
        public AutoMapperFixture()
        {

        }


        readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(() =>
        {
            MappingConfig.ConfigureMapper();
            return Mapper.Instance;
        });

        public IMapper MapperInstance
        {
            get { return _mapper.Value; }
        }
    }
}
