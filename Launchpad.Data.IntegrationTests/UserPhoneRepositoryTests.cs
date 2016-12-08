using FluentAssertions;
using Launchpad.Models.EntityFramework;
using System.Linq;
using System.Transactions;
using Xunit;

namespace Launchpad.Data.IntegrationTests
{
    public class UserPhoneRepositoryTests
    {
        [Fact]
        public void Get_Should_Return_Phone()
        {
            using (var transactionScope = new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    //Arrange
                    //Create a user phone number
                    var user = context.Users.First();
                    var phone = new UserPhone
                    {
                        User = user,
                        UserId = user.Id,
                        PhoneNumber = "454343",
                        PhoneType = 0
                    };
                    user.Phones.Add(phone);
                    context.SaveChanges();

                    //Act
                    var repository = new UserPhoneRepository(context);

                    //Assert
                    var retrievedPhone = repository.Get(phone.Id);
                    retrievedPhone.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void Delete_Should_Remove_Phone()
        {
            using (var transactionScope = new TransactionScope())
            {
                using (var context = new LaunchpadDataContext())
                {
                    //Arrange
                    //Create a user phone number
                    var user = context.Users.First();
                    var phone = new UserPhone
                    {
                        User = user,
                        UserId = user.Id,
                        PhoneNumber = "454343",
                        PhoneType = 0
                    };
                    user.Phones.Add(phone);
                    context.SaveChanges();


                    //Act
                    var repository = new UserPhoneRepository(context);
                    repository.Delete(phone.Id);
                    context.SaveChanges();

                    //Assert
                    var retrievedPhone = repository.Get(phone.Id);
                    retrievedPhone.Should().BeNull();
                }
            }
        }
    }
}
