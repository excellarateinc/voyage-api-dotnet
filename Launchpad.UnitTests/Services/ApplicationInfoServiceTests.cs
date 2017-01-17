using FluentAssertions;

using Launchpad.Services.ApplicationInfo;
using Launchpad.Services.FileReader;
using Launchpad.UnitTests.Common;

using Moq;

using Xunit;

namespace Launchpad.UnitTests.Services
{
    public class ApplicationInfoServiceTests : BaseUnitTest
    {
        private readonly ApplicationInfoService _applicationInfoService;

        public ApplicationInfoServiceTests()
        {
            const string fileName = "MyFile";
            var fileReaderService = new Mock<IFileReaderService>();

            fileReaderService.Setup(_ =>
                _.ReadAllText(It.IsAny<string>()))
                .Returns("{ 'build': { 'buildNumber': 'some_number'}}");

            _applicationInfoService = new ApplicationInfoService(fileReaderService.Object, fileName);
        }

        [Fact]
        public void GetApplicationInfo_Should_Read_Application_Info()
        {
            var result = _applicationInfoService.GetApplicationInfo();

            result.BuildNumber.Should().BeOfType<string>().And.Be("some_number");
        }
    }
}
