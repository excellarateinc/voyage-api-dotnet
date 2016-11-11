using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Moq;
using System.Linq;
using Xunit;
using Ploeh.AutoFixture;
using FluentAssertions;
using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;

namespace Launchpad.Services.UnitTests
{
    public class ApplicationInfoServiceTests : BaseUnitTest
    {
        private ApplicationInfoService _applicationInfoService;


        public ApplicationInfoServiceTests()
        {
            _applicationInfoService = new ApplicationInfoService();
        }

        [Fact]
        public void GetApplicationInfo_Should_Read_Application_Info()
        {
            var result = _applicationInfoService.GetApplicationInfo(new Dictionary<string, string>() { { "buildNumber", "some_number" } });

            result.BuildNumber.Should().BeOfType<string>().And.Be("some_number");
        }
    }
}
