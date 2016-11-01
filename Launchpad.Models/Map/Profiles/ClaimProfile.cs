using AutoMapper;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Models.Map.Profiles
{
  
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<RoleClaim, ClaimModel>();
        }
    }
}
