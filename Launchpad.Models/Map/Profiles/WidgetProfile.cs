using AutoMapper;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Models.Map.Profiles
{
    public class WidgetProfile : Profile
    {
        public WidgetProfile()
        {
            CreateMap<Widget, WidgetModel>();
            CreateMap<WidgetModel, Widget>();
        }

    }
}
