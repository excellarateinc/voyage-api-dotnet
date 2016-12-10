using FluentAssertions;
using Launchpad.Data.IntegrationTests.Extensions;
using Launchpad.Models.EntityFramework;
using System;
using System.Globalization;
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
            using (new TransactionScope())
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
            using (new TransactionScope())
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
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new WidgetRepository(context);

                    var widget = repository.Get(-1);

                    widget.Should().BeNull();
                }
            }
        }

        [Fact]
        public void Add_Should_Create_New_Widget()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new WidgetRepository(context);
                    var newWidget = new Widget { Name = "Super Widget" };

                    var widget = repository.Add(newWidget);

                    widget.Should().NotBeNull();
                    widget.Id.Should().BeGreaterThan(0);

                    var retrievedWidget = repository.Get(newWidget.Id);
                    retrievedWidget.Should().NotBeNull();
                    retrievedWidget.Name.Should().Be(widget.Name);
                    retrievedWidget.Id.Should().Be(widget.Id);                    
                }
            }
        }

        [Fact]
        public void Update_Should_Modify_Widget()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new WidgetRepository(context);
                    var widget = context.AddWidget();
                    var name = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                    widget.Name = name;

                    repository.Update(widget);

                    var retrievedWidget = repository.Get(widget.Id);
                    retrievedWidget.Name.Should().Be(name);
                }
            }
        }

        [Fact]
        public void Delete_Should_Remove_Widget()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var repository = new WidgetRepository(context);
                    var widget = context.AddWidget();

                    repository.Delete(widget.Id);

                    var retrievedWidget = repository.Get(widget.Id);
                    retrievedWidget.Should().BeNull();
                }
            }
        }
    }
}
