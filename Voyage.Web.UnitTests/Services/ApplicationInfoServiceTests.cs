using FluentAssertions;
using Moq;
using Voyage.Services.ApplicationInfo;
using Voyage.Services.FileReader;
using Voyage.Web.UnitTests.Common;
using Xunit;

namespace Voyage.Web.UnitTests.Services
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
