using FluentAssertions;
using System.Linq;
using System.Transactions;
using Xunit;

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

            //Create a new transaction scope
            using(var scope = new TransactionScope())
            {

                //Create a data context
                using(var context = new LaunchpadDataContext())
                {

                    //Arrange

                    //Create a widget (in case the database is empty
                    context.Widgets.Add(new Models.EntityFramework.Widget { Name = "My Test Widget" });
                    context.SaveChanges();

                    //Act
                    var widget = context.Widgets.First();


                    //Assert
                    widget.Should().NotBeNull();
        
                }

            }
        }
    }
}
