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
        EntityResult<IEnumerable<WidgetModel>> GetWidgets();

        /// <summary>
        /// Creates a new widget in the database
        /// </summary>
        /// <param name="widget">Model containing the properties for the new widget</param>
        /// <returns>New entity</returns>
        EntityResult<WidgetModel> AddWidget(WidgetModel widget);

        /// <summary>
        /// Retrieve a single widget by ID
        /// </summary>
        /// <param name="id">ID of the target widget</param>
        /// <returns>Instance of the target widget if it exists, otherwise null</returns>
        EntityResult<WidgetModel> GetWidget(int id);

        /// <summary>
        /// Update a widget by ID
        /// </summary>
        /// <param name="widget">Model containing updated properties</param>
        /// <returns>Updated entity</returns>
        EntityResult<WidgetModel> UpdateWidget(int id, WidgetModel widget);

        /// <summary>
        /// Removes a widget from the database
        /// </summary>
        /// <param name="id">ID of the widget that should be removed</param>
        EntityResult DeleteWidget(int id);
    }
}
