﻿using System.Linq;
using System.Transactions;

using FluentAssertions;

using Voyage.Data;
using Voyage.Data.Repositories.UserPhone;
using Voyage.Models.Entities;

using Xunit;

namespace Voyage.IntegrationTests.Data
{
    [Collection(Constants.CollectionName)]
    public class UserPhoneRepositoryTests
    {
        [Fact]
        public void Get_Should_Return_Phone()
        {
            using (new TransactionScope())
            {
                using (var context = new VoyageDataContext())
                {
                    // Arrange
                    // Create a user phone number
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

                    // Act
                    var repository = new UserPhoneRepository(context);

                    // Assert
                    var retrievedPhone = repository.Get(phone.Id);
                    retrievedPhone.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void Delete_Should_Remove_Phone()
        {
            using (new TransactionScope())
            {
                using (var context = new VoyageDataContext())
                {
                    // Arrange
                    // Create a user phone number
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

                    // Act
                    var repository = new UserPhoneRepository(context);
                    repository.Delete(phone.Id);
                    context.SaveChanges();

                    // Assert
                    var retrievedPhone = repository.Get(phone.Id);
                    retrievedPhone.Should().BeNull();
                }
            }
        }
    }
}
