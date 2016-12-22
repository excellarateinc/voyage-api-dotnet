using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Data
{
    public class WidgetRepository : BaseRepository<Widget>, IWidgetRepository
    {
        public WidgetRepository(ILaunchpadDataContext context) : base(context)
        {
        }      
    }
}
