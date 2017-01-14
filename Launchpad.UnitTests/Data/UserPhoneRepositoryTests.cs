using System;

using FluentAssertions;

using Launchpad.Data;
using Launchpad.Data.Repositories.UserPhone;
using Launchpad.Models.Entities;
using Launchpad.UnitTests.Common;

using Xunit;

namespace Launchpad.UnitTests.Data
{
    public class UserPhoneRepositoryTests : BaseUnitTest
    {
        private readonly UserPhoneRepository _phoneRepository;

        public UserPhoneRepositoryTests()
        {
            var mockContext = Mock.Create<ILaunchpadDataContext>();
            _phoneRepository = new UserPhoneRepository(mockContext.Object);
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
