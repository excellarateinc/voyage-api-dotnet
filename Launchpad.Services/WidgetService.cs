using Launchpad.Services.Interfaces;
using System.Linq;
using Launchpad.Models;
using Launchpad.Data;

namespace Launchpad.Services
{
    public class WidgetService : IWidgetService
    {
        private ILaunchpadDataContext _context;

        public WidgetService(ILaunchpadDataContext context)
        {
            _context = context;
        }

        public IQueryable<WidgetModel> GetWidgets()
        {
            return _context.Widgets.Select(_ => new WidgetModel { Id = _.Id, Name = _.Name });
        }
    }
}
