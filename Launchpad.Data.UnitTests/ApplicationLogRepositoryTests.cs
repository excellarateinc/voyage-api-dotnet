using System;
using Xunit;
using FluentAssertions;
using Launchpad.UnitTests.Common;
using Launchpad.Data.Repositories.ApplicationLog;
using Launchpad.Models.Entities;

namespace Launchpad.Data.UnitTests
{
    public class ApplicationLogRepositoryTests : BaseUnitTest
    {
        private readonly ApplicationLogRepository _repository;

        public ApplicationLogRepositoryTests()
        {
            var mockContext = Mock.Create<ILaunchpadDataContext>();
            _repository = new ApplicationLogRepository(mockContext.Object);
        }

        [Fact]
        public void Add_Should_Throw_NotImplementedException()
        {
            Action throwAction = () => _repository.Add(new ApplicationLog());
            throwAction.ShouldThrow<NotImplementedException>();
        }

        [Fact]
        public void Delete_hould_Throw_NotImplementedException()
        {
            Action throwAction = () => _repository.Delete(1);
            throwAction.ShouldThrow<NotImplementedException>();
        }

        [Fact]
        public void Update_Should_Throw_NotImplementedException()
        {
            Action throwAction = () => _repository.Update(new ApplicationLog());
            throwAction.ShouldThrow<NotImplementedException>();
        }

        [Fact]
        public void GetAll_Should_Throw_NotImplementedException()
        {
            Action throwAction = () => _repository.GetAll();
            throwAction.ShouldThrow<NotImplementedException>();
        }

        [Fact]
        public void Get_Should_Throw_NotImplementedException()
        {
            Action throwAction = () => _repository.Get(1);
            throwAction.ShouldThrow<NotImplementedException>();
        }
    }
}
