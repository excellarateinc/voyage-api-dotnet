using Launchpad.Services.Interfaces;
using System.Linq;
using Launchpad.Models;

namespace Launchpad.Services
{
    public class WidgetService : IWidgetService
    {
        public IQueryable<WidgetModel> GetWidgets()
        {
            return Enumerable.Range(0, 10).Select(_ => new WidgetModel { Id = _ }).AsQueryable();
        }
    }
}
