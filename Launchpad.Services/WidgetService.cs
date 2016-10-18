using Launchpad.Services.Interfaces;
using System.Linq;
using Launchpad.Models;
using Launchpad.Data.Interfaces;

namespace Launchpad.Services
{
    public class WidgetService : IWidgetService
    {
        private IWidgetRepository _widgetRepository;

        public WidgetService(IWidgetRepository widgetRepository)
        {
            _widgetRepository = widgetRepository;
        }

        public IQueryable<WidgetModel> GetWidgets()
        {
            return _widgetRepository.GetAll().Select(_ => new WidgetModel { Id = _.Id, Name = _.Name });
        }
    }
}
