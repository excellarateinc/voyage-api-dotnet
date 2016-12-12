using AutoMapper;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Models.Map.Profiles
{
    /// <summary>
    /// Configures the mappings for the "Widget" type
    /// This contains the mapping from the EF Model to the Domain Model and vice versa
    /// </summary>
    public class WidgetProfile : Profile
    {
        public WidgetProfile()
        {
            // Map EF Widget to WidgetModel
            CreateMap<Widget, WidgetModel>();

            // Map WidgetModel to EF Widget
            CreateMap<WidgetModel, Widget>()
                .ForMember(_ => _.Id, opt => opt.Ignore());
        }
    }
}
