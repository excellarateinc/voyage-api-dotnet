using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Launchpad.Models;
using Launchpad.Models.Entities;
using Launchpad.Services.UnitTests.Fixture;
using Launchpad.UnitTests.Common;
using Ploeh.AutoFixture;
using Xunit;

namespace Launchpad.Services.UnitTests
{
    [Collection(AutoMapperCollection.CollectionName)]
    public class EntityResultServiceTests : BaseUnitTest
    {
        /// <summary>
        /// Wrapped the abstract class in order to test the methods
        /// </summary>
        public class TestPassThrough : EntityResultService
        {
            public TestPassThrough(IMapper mapper) : base(mapper)
            {
            }

            public EntityResult InvokeNotFound(object id)
            {
                return NotFound(id);
            }

            public EntityResult<TModel> InvokeNotFound<TModel>(object id)
                 where TModel : class
            {
                return NotFound<TModel>(id);
            }

            public EntityResult InvokeSuccess()
            {
                return Success();
            }

            public EntityResult<TModel> InvokeSuccess<TModel>(TModel model)
                 where TModel : class
            {
                return Success(model);
            }

            public void InvokeMergeCollection<TSource, TDest>(
                ICollection<TSource> source,
                ICollection<TDest> destination,
                Func<TSource, TDest, bool> predicate,
                Action<TDest> deleteAction)
            {
                MergeCollection(source, destination, predicate, deleteAction);
            }
        }

        private readonly TestPassThrough _testPassThrough;

        public EntityResultServiceTests(AutoMapperFixture mapperFixture)
        {
            _testPassThrough = new TestPassThrough(mapperFixture.MapperInstance);
        }

        [Fact]
        public void MergeCollection_Should_Delete_Items_In_Destination_That_Are_Not_In_Source()
        {
            int deleteCount = 0;

            var source = new List<UserPhoneModel>();

            var user = Fixture.Build<UserPhone>()
                .With(_ => _.User, null)
                .Create();

            var destination = new List<UserPhone>
            {
                user
            };

            _testPassThrough.InvokeMergeCollection(
                source,
                destination,
                (s, d) => s.Id == d.Id,
                    phone => ++deleteCount);

            destination
                .Should()
                .BeEmpty();

            deleteCount.Should().Be(1);
        }

        [Fact]
        public void MergeCollection_Should_Update_Matching_Items_In_Destination()
        {
            var userPhoneModel = Fixture.Create<UserPhoneModel>();

            var source = new List<UserPhoneModel>
            {
                userPhoneModel
            };

            var user = Fixture.Build<UserPhone>()
                .With(_ => _.Id, userPhoneModel.Id)
                .With(_ => _.User, null)
                .Create();

            var destination = new List<UserPhone>
            {
                user
            };
            _testPassThrough
                .InvokeMergeCollection(source, destination, (s, d) => s.Id == d.Id, phone => { });

            destination
                .Should()
                .NotBeEmpty()
                .And
                .HaveCount(1);

            destination
                .First()
                .ShouldBeEquivalentTo(
                    userPhoneModel,
                    options => options.Excluding(_ => _.User));
        }

        [Fact]
        public void MergeCollection_Should_Add_New_Items_To_Destination()
        {
            var userPhoneModel = Fixture.Create<UserPhoneModel>();

            var source = new List<UserPhoneModel>
            {
                userPhoneModel
            };

            var destination = new List<UserPhone>();
            _testPassThrough
                .InvokeMergeCollection(source, destination, (s, d) => s.Id == d.Id, phone => { });

            destination
                .Should()
                .NotBeEmpty()
                .And
                .HaveCount(1);

            destination
                .First()
                .ShouldBeEquivalentTo(
                    userPhoneModel,
                    options => options.Excluding(_ => _.User));
        }

        [Fact]
        public void Success_Should_Return_Result()
        {
            var result = _testPassThrough.InvokeSuccess();
            result.Succeeded.Should().BeTrue();
            result.IsEntityNotFound.Should().BeFalse();
            result.Errors.Should().HaveCount(0);
        }

        [Fact]
        public void SuccessTModel_Should_Return_Result()
        {
            var model = new object();
            var result = _testPassThrough.InvokeSuccess(model);
            result.Succeeded.Should().BeTrue();
            result.IsEntityNotFound.Should().BeFalse();
            result.Errors.Should().HaveCount(0);
            result.Model.Should().Be(model);
        }

        [Fact]
        public void NotFound_Should_Return_Result()
        {
            var id = Fixture.Create<string>();
            var result = _testPassThrough
                .InvokeNotFound(id);

            result.Succeeded.Should().BeFalse();
            result.IsEntityNotFound.Should().BeTrue();
            result.Errors
                .Should()
                .HaveCount(1)
                .And
                .HaveElementAt(0, "notfound.entity::Could not locate entity with ID " + id);
        }

        [Fact]
        public void NotFoundTModel_Should_Returnn_Result()
        {
            var id = Fixture.Create<string>();
            var result = _testPassThrough
                .InvokeNotFound<object>(id);

            result.Succeeded.Should().BeFalse();
            result.IsEntityNotFound.Should().BeTrue();
            result.Errors
                .Should()
                .HaveCount(1)
                .And
                .HaveElementAt(0, "notfound.entity::Could not locate entity with ID " + id);
        }
    }
}
