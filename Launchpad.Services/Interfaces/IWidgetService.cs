using Launchpad.Models;
using System.Collections.Generic;

namespace Launchpad.Services.Interfaces
{
    /// <summary>
    /// Interface for widget services
    /// </summary>
    public interface IWidgetService
    {
        /// <summary>
        /// Retrieve all widgets
        /// </summary>
        /// <returns>Enumerable of widgets</returns>
        IEnumerable<WidgetModel> GetWidgets();
        WidgetModel AddWidget(WidgetModel widget);

        /// <summary>
        /// Retrieve a single widget by ID
        /// </summary>
        /// <param name="id">ID of the target widget</param>
        /// <returns>Instance of the target widget if it exists, otherwise null</returns>
        WidgetModel GetWidget(int id);
        WidgetModel UpdateWidget(WidgetModel widget);
    }
}
