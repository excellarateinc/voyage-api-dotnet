﻿using System.Linq;
using System.Transactions;

using FluentAssertions;

using Launchpad.Data;
using Launchpad.Data.Stores;

using Xunit;

namespace Launchpad.IntegrationTests.Data.Stores
{
    [Collection(Constants.CollectionName)]
    public class CustomUserStoreTests
    {
        [Fact]
        public async void FindByEmailAsync_Should_Not_Match_Deleted_User()
        {
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    // Arrange
                    var user = ctx.Users.First();
                    user.Deleted = true;
                    ctx.SaveChanges();

                    // Act
                    var userStore = new CustomUserStore(ctx);
                    var matched = await userStore.FindByEmailAsync(user.Email);

                    // Assert
                    matched.Should().BeNull();
                }
            }
        }

        [Fact]
        public async void FindByIdAsync_Should_Not_Match_Deleted_User()
        {
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    // Arrange
                    var user = ctx.Users.First();
                    user.Deleted = true;
                    ctx.SaveChanges();

                    // Act
                    var userStore = new CustomUserStore(ctx);
                    var matched = await userStore.FindByIdAsync(user.Id);

                    // Assert
                    matched.Should().BeNull();
                }
            }
        }

        [Fact]
        public async void FindByNameAsync_Should_Not_Match_Deleted_User()
        {
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    // Arrange
                    var user = ctx.Users.First();
                    user.Deleted = true;
                    ctx.SaveChanges();

                    // Act
                    var userStore = new CustomUserStore(ctx);
                    var matched = await userStore.FindByNameAsync(user.UserName);

                    // Assert
                    matched.Should().BeNull();
                }
            }
        }

        [Fact]
        public async void FindByEmailAsync_Should_Match_Existing_User()
        {
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    // Arrange
                    var user = ctx.Users.First(_ => !_.Deleted);

                    // Act
                    var userStore = new CustomUserStore(ctx);
                    var matched = await userStore.FindByEmailAsync(user.Email);

                    // Assert
                    matched.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public async void FindByIdAsync_Should_Match_Existing_User()
        {
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    // Arrange
                    var user = ctx.Users.First(_ => !_.Deleted);

                    // Act
                    var userStore = new CustomUserStore(ctx);
                    var matched = await userStore.FindByIdAsync(user.Id);

                    // Assert
                    matched.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public async void FindByNameAsync_Should_Match_Existing_User()
        {
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var ctx = new LaunchpadDataContext())
                {
                    // Arrange
                    var user = ctx.Users.First(_ => !_.Deleted);

                    // Act
                    var userStore = new CustomUserStore(ctx);
                    var matched = await userStore.FindByNameAsync(user.UserName);

                    // Assert
                    matched.Should().NotBeNull();
                }
            }
        }
    }
}
