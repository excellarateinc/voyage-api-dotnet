﻿using System;
using FluentAssertions;
using Voyage.Data.Repositories.UserPhone;
using Voyage.Data.UnitTests.Common;
using Voyage.Models.Entities;
using Xunit;

namespace Voyage.Data.UnitTests
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
        public void GetAll_Should_Throw_NotImplementedException()
        {
            Action throwAction = () => _phoneRepository.GetAll();
            throwAction
                .ShouldThrow<NotImplementedException>();
        }
    }
}
