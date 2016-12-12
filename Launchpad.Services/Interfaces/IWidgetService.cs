using Launchpad.Models;
using System.Collections.Generic;

namespace Launchpad.Services.Interfaces
{
    public interface IWidgetService
    {
        EntityResult<IEnumerable<WidgetModel>> GetWidgets();

        EntityResult<WidgetModel> AddWidget(WidgetModel widget);

        EntityResult<WidgetModel> GetWidget(int id);

        EntityResult<WidgetModel> UpdateWidget(int id, WidgetModel widget);

        EntityResult DeleteWidget(int id);
    }
}
