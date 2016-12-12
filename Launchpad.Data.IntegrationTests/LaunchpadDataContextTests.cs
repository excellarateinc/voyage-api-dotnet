using FluentAssertions;
using System.Linq;
using System.Transactions;
using Xunit;
using Launchpad.Data.IntegrationTests.Extensions;

namespace Launchpad.Data.IntegrationTests
{
    /// <summary>
    /// The collection attribute prevents the integration tests from running in parallel. This should be on all integration tests that 
    /// have a transaction scope.
    /// </summary>
    [Collection(Constants.CollectionName)]
    public class LaunchpadDataContextTests
    {
        [Fact]
        public void Widgets_Should_Return_Record()
        {
            // Create a new transaction scope
            using (new TransactionScope())
            {
                // Create a data context
                using (var context = new LaunchpadDataContext())
                {
                    // Arrange

                    // Create a widget (in case the database is empty)
                    context.AddWidget();

                    // Act
                    var widget = context.Widgets.First();

                    // Assert
                    widget.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void ActivityAudits_Should_Query_Database()
        {
            // Create a new transaction scope
            using (new TransactionScope())
            {
                // Create a data context
                using (var context = new LaunchpadDataContext())
                {
                    // Just force a database query
                    var results = context.ActivityAudits.Take(10);

                    results.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void Logs_Should_Query_Database()
        {
            // Create a new transaction scope
            using (new TransactionScope())
            {
                // Create a data context
                using (var context = new LaunchpadDataContext())
                {
                    // Just force a database query
                    var results = context.Logs.Take(10);

                    results.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void RoleClaims_Should_Query_Database()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var results = context.RoleClaims.Take(10).ToList();
                    results.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void Users_Should_Query_Database()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var results = context.Users.Take(10).ToList();
                    results.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void Users_Should_Have_Phones()
        {
            using (new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    var results = context.Users.First();
                    results.Phones.Should().NotBeNull();
                }
            }
        }
    }
}
