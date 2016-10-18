using Launchpad.Data.Interfaces;
using Launchpad.Models;
using System.Linq;

namespace Launchpad.Data
{
    public class WidgetRepository : BaseRepository<Widget>, IWidgetRepository
    {
        public WidgetRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public override IQueryable<Widget> GetAll()
        {
            return Context.Widgets;
        }
    }
}
