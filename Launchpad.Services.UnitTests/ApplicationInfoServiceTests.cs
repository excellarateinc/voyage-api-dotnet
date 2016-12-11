using FluentAssertions;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Moq;
using Xunit;

namespace Launchpad.Services.UnitTests
{
    public class ApplicationInfoServiceTests : BaseUnitTest
    {
        private readonly ApplicationInfoService _applicationInfoService;
        
        public ApplicationInfoServiceTests()
        {
            var configurationManagerService = new Mock<IConfigurationManagerService>();
            var pathProviderService = new Mock<IPathProviderService>();
            var fileReaderService = new Mock<IFileReaderService>();

            configurationManagerService.Setup(_ => _.GetAppSetting("ApplicationInfoFileName")).Returns(string.Empty);
            pathProviderService.Setup(_ => _.LocalPath).Returns(string.Empty);
            fileReaderService.Setup(_ => _.ReadAllText(string.Empty)).Returns("{ 'build': { 'buildNumber': 'some_number'}}");

            _applicationInfoService = new ApplicationInfoService(configurationManagerService.Object, fileReaderService.Object, pathProviderService.Object);
        }

        [Fact]
        public void GetApplicationInfo_Should_Read_Application_Info()
        {
            var result = _applicationInfoService.GetApplicationInfo();

            result.BuildNumber.Should().BeOfType<string>().And.Be("some_number");
        }
    }
}
