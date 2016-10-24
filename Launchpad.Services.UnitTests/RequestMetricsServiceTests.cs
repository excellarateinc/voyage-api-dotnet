using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Ploeh.AutoFixture;
using Launchpad.UnitTests.Common;
using Launchpad.Models;

namespace Launchpad.Services.UnitTests
{
    public class RequestMetricsServiceTests : BaseUnitTest
    {
        private readonly RequestMetricsService _service;

        public RequestMetricsServiceTests()
        {
            _service = new RequestMetricsService();
        }

        [Fact]
        public void Log_Should_Store_Entry()
        {
            var datapoint = Fixture.Create<RequestDataPointModel>();

            _service.Log(datapoint);

            var activity = _service.GetActivity();
            activity.Should().HaveCount(1);
            activity.First().ShouldBeEquivalentTo(datapoint);
        }

        [Fact]
        public void Log_Should_Store_Only_10_Entries()
        {
            var datapoints = Fixture.CreateMany<RequestDataPointModel>(20);
            foreach (var p in datapoints)
            {
                _service.Log(p);
                _service.GetActivity().Should().HaveCount(_ => _ <= 10);
            }
        }

        [Fact]
        public void GetActivity_Should_Return_Entries()
        {
            var datapoints = Fixture.CreateMany<RequestDataPointModel>();

            foreach(var p in datapoints)
            {
                _service.Log(p);
            }

            var activity = _service.GetActivity();

            activity.ShouldBeEquivalentTo(datapoints);

        }
    }
}
