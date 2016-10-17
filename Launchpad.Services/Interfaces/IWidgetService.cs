using Launchpad.Models;
using System.Linq;

namespace Launchpad.Services.Interfaces
{
    public interface IWidgetService
    {
        IQueryable<WidgetModel> GetWidgets();
    }
}
