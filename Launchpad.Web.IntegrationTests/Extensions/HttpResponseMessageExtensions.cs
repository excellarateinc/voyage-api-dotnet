using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Launchpad.Web.IntegrationTests.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<TType> ShouldHaveStatusAndPayload<TType>(this HttpResponseMessage message, HttpStatusCode code)
        {
            message.StatusCode.Should().Be(code);
            var rawContent = await message.Content.ReadAsStringAsync();
            TType models = JsonConvert.DeserializeObject<TType>(rawContent);
            models.Should().NotBeNull();
            return models;
        }
    }
}
