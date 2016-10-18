using Launchpad.Models;
using System.Collections.Generic;

namespace Launchpad.Services.Interfaces
{
    public interface IWidgetService
    {
        IEnumerable<WidgetModel> GetWidgets();
        WidgetModel GetWidget(int id);
    }
}
