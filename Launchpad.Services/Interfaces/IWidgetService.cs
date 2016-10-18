using Launchpad.Models;
using System.Collections.Generic;
using System.Linq;

namespace Launchpad.Services.Interfaces
{
    public interface IWidgetService
    {
        IEnumerable<WidgetModel> GetWidgets();
        WidgetModel GetWidget(int id);
    }
}
