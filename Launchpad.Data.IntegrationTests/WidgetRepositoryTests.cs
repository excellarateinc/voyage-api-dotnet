using FluentAssertions;
using Launchpad.Data.IntegrationTests.Extensions;
using System.Transactions;
using Xunit;

namespace Launchpad.Data.IntegrationTests
{
    [Collection(Constants.CollectionName)]
    public class WidgetRepositoryTests
    {
        [Fact]
        public void GetAll_Should_Return_Widgets()
        {
            using (var transactionScope = new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new WidgetRepository(context);
                    context.AddWidget();
                    context.AddWidget();

                    var widgets = repository.GetAll();

                    widgets.Should().HaveCount(_ => _ >= 2);
                }
            }
        }

        [Fact]
        public void Get_Should_Return_Widget_If_Exists()
        {
            using (var transactionScope = new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new WidgetRepository(context);
                    var newWidget = context.AddWidget();

                    var widget = repository.Get(newWidget.Id);

                    widget.Name.Should().Be(newWidget.Name);
                    widget.Id.Should().Be(newWidget.Id);
                }
            }
        }

        [Fact]
        public void Get_Should_Return_Null_If_Does_Not_Exist()
        {
            using (var transactionScope = new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new WidgetRepository(context);

                    var widget = repository.Get(-1);

                    widget.Should().BeNull();
                }
            }
        }
    }
}
