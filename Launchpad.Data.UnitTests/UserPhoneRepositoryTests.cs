using FluentAssertions;
using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using Launchpad.UnitTests.Common;
using Moq;
using System;
using Xunit;

namespace Launchpad.Data.UnitTests
{
    public class UserPhoneRepositoryTests : BaseUnitTest
    {
        private readonly UserPhoneRepository _phoneRepository;
        private readonly Mock<ILaunchpadDataContext> _mockContext;

        public UserPhoneRepositoryTests()
        {
            _mockContext = Mock.Create<ILaunchpadDataContext>();
            _phoneRepository = new UserPhoneRepository(_mockContext.Object);
        }

        [Fact]
        public void Add_Should_Throw_NotImplementedException()
        {
            Action throwAction = () => _phoneRepository.Add(new UserPhone());
            throwAction
                .ShouldThrow<NotImplementedException>();

        }

        [Fact]
        public void Update_Should_Throw_NotImplementedException()
        {
            Action throwAction = () => _phoneRepository.Update(new UserPhone());
            throwAction
                .ShouldThrow<NotImplementedException>();

        }

        [Fact]
        public void GetAll_Should_Throw_NotImplementedException()
        {
            Action throwAction = () => _phoneRepository.GetAll();
            throwAction
                .ShouldThrow<NotImplementedException>();

        }
    }
}
