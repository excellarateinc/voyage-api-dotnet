using System;

using FluentAssertions;

using Voyage.Data;
using Voyage.Data.Repositories.UserPhone;
using Voyage.Models.Entities;
using Voyage.UnitTests.Common;

using Xunit;

namespace Voyage.UnitTests.Data
{
    public class UserPhoneRepositoryTests : BaseUnitTest
    {
        private readonly UserPhoneRepository _phoneRepository;

        public UserPhoneRepositoryTests()
        {
            var mockContext = Mock.Create<IVoyageDataContext>();
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
        public void GetAll_Should_Throw_NotImplementedException()
        {
            Action throwAction = () => _phoneRepository.GetAll();
            throwAction
                .ShouldThrow<NotImplementedException>();
        }
    }
}
