using Launchpad.Models.EntityFramework;

namespace Launchpad.Data.IntegrationTests.Extensions
{
    public static class WidgetContextExtensions
    {
        public static Widget AddWidget(this LaunchpadDataContext context)
        {
            var widget = new Models.EntityFramework.Widget { Name = "My Test Widget" };
            context.Widgets.Add(widget);
            context.SaveChanges();
            return widget;
        }
    }
}
