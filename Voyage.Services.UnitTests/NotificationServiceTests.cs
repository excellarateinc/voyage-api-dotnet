using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Voyage.Core.Exceptions;
using Voyage.Data.Repositories.Notification;
using Voyage.Services.Notification;
using Voyage.Services.Notification.Push;
using Voyage.Services.UnitTests.Common;
using Voyage.Services.UnitTests.Common.AutoMapperFixture;
using Xunit;

namespace Voyage.Services.UnitTests
{
    [Trait("Category", "Notification.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class NotificationServiceTests : BaseUnitTest
    {
        private readonly NotificationService _notificationService;
        private readonly Mock<INotificationRepository> _mockNotificationRepository;
        private readonly AutoMapperFixture _mapperFixture;
        private readonly Mock<IPushService> _mockPushService;

        public NotificationServiceTests(AutoMapperFixture mapperFixture)
        {
            _mockNotificationRepository = Mock.Create<INotificationRepository>();
            _mockPushService = Mock.Create<IPushService>();
            _mapperFixture = mapperFixture;

            _notificationService = new NotificationService(_mockNotificationRepository.Object, _mapperFixture.MapperInstance, _mockPushService.Object);
        }

        [Fact]
        public void NotificationService_GetNotifications_ShouldReturnUnreadNotificationsForUser()
        {
            // Arrange
            const string userId = "TestUser";
            var notificationsList = new List<Models.Entities.Notification>
            {
                new Models.Entities.Notification { AssignedToUserId = userId, IsRead = false },
                new Models.Entities.Notification { AssignedToUserId = "Other", IsRead = false },
                new Models.Entities.Notification { AssignedToUserId = userId, IsRead = true }
            };
            _mockNotificationRepository.Setup(_ => _.GetAll()).Returns(notificationsList.AsQueryable());

            // Act
            var result = _notificationService.GetNotifications(userId);

            // Assert
            result.Count().Should().Be(1);
            result.First().AssignedToUserId.Should().Be(userId);
            result.First().IsRead.Should().BeFalse();
        }

        [Fact]
        public void NotificationService_CreateNotification_ShouldCreateNotificationForUser()
        {
            // Arrange
            _mockNotificationRepository.Setup(_ => _.Add(It.IsAny<Models.Entities.Notification>()))
                .Returns(new Models.Entities.Notification { Id = 1 });

            _mockPushService.Setup(_ => _.PushNotification(It.IsAny<Models.NotificationModel>()));

            // Act
            var notification = new Models.NotificationModel();
            var result = _notificationService.CreateNotification(notification);

            // Assert
            result.Id.Should().Be(1);
        }

        [Fact]
        public void NotificationService_MarkNotificationAsRead_ShouldMarkNotificationAsRead()
        {
            // Arrange
            const string userId = "TestUser";
            const int notificationId = 1;
            var notification = new Models.Entities.Notification
            {
                Id = notificationId,
                AssignedToUserId = userId,
                IsRead = false
            };
            _mockNotificationRepository.Setup(_ => _.Get(It.IsAny<int>())).Returns(notification);
            _mockNotificationRepository.Setup(_ => _.SaveChanges()).Returns(1);

            // Act
            _notificationService.MarkNotificationAsRead(userId, notificationId);

            // Assert
            notification.IsRead.Should().BeTrue();
        }

        [Fact]
        public void NotificationService_MarkNotificationAsRead_NotificationNotAssignedToCurrentUser_ShouldThrowUnauthorizedException()
        {
            // Arrange
            const string userId = "TestUser";
            const int notificationId = 1;
            var notification = new Models.Entities.Notification
            {
                Id = notificationId,
                AssignedToUserId = "OtherUser",
                IsRead = false
            };
            _mockNotificationRepository.Setup(_ => _.Get(It.IsAny<int>())).Returns(notification);

            // Act
            Action throwAction = () => _notificationService.MarkNotificationAsRead(userId, notificationId);

            // Assert
            throwAction.ShouldThrow<UnauthorizedException>();
        }

        [Fact]
        public void NotificationService_MarkAllNotificationsAsRead_ShouldMarkAllNotificationAsReadForUser()
        {
            // Arrange
            const string userId = "TestUser";
            const int notificationId = 1;
            var notifications = new List<Models.Entities.Notification>
            {
                new Models.Entities.Notification
                {
                    Id = notificationId,
                    AssignedToUserId = userId,
                    IsRead = false
                },
                new Models.Entities.Notification
                {
                    Id = notificationId,
                    AssignedToUserId = userId,
                    IsRead = false
                },
                new Models.Entities.Notification
                {
                    Id = notificationId,
                    AssignedToUserId = "OtherUser",
                    IsRead = false
                }
            };
            _mockNotificationRepository.Setup(_ => _.GetAll()).Returns(notifications.AsQueryable());
            _mockNotificationRepository.Setup(_ => _.SaveChanges()).Returns(1);

            // Act
            _notificationService.MarkAllNotificationsAsRead(userId);

            // Assert
            notifications.Where(_ => _.IsRead).Should().HaveCount(2);
        }
    }
}
