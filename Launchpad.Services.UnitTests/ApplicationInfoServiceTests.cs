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
            var _configurationManagerService = new Mock<IConfigurationManagerService>();
            var _pathProviderService = new Mock<IPathProviderService>();
            var _fileReaderService = new Mock<IFileReaderService>();

            _configurationManagerService.Setup(_ => _.GetAppSetting("ApplicationInfoFileName")).Returns(string.Empty);
            _pathProviderService.Setup(_ => _.LocalPath).Returns(string.Empty);
            _fileReaderService.Setup(_ => _.ReadAllText(string.Empty)).Returns("{ 'build': { 'buildNumber': 'some_number'}}");

            _applicationInfoService = new ApplicationInfoService(_configurationManagerService.Object, _fileReaderService.Object, _pathProviderService.Object);
        }

        [Fact]
        public void GetApplicationInfo_Should_Read_Application_Info()
        {
            var result = _applicationInfoService.GetApplicationInfo();

            result.BuildNumber.Should().BeOfType<string>().And.Be("some_number");
        }
    }
}
