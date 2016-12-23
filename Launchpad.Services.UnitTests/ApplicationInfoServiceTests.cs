using FluentAssertions;
using Launchpad.Services.Interfaces;
using Launchpad.UnitTests.Common;
using Moq;
using Xunit;
using Launchpad.Services.ApplicationInfo;

namespace Launchpad.Services.UnitTests
{    
    public class ApplicationInfoServiceTests : BaseUnitTest
    {
        private readonly ApplicationInfoService _applicationInfoService;
        
        public ApplicationInfoServiceTests()
        {
            const string fileName = "MyFile";
            var _fileReaderService = new Mock<IFileReaderService>();

            _fileReaderService.Setup(_ =>
                _.ReadAllText(It.IsAny<string>()))
                .Returns("{ 'build': { 'buildNumber': 'some_number'}}");

            _applicationInfoService = new ApplicationInfoService(_fileReaderService.Object, fileName);
        }

        [Fact]
        public void GetApplicationInfo_Should_Read_Application_Info()
        {
            var result = _applicationInfoService.GetApplicationInfo();

            result.BuildNumber.Should().BeOfType<string>().And.Be("some_number");
        }
    }
}
