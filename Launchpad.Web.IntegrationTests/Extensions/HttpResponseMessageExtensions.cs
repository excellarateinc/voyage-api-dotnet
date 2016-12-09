using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Launchpad.Web.IntegrationTests.Extensions
{

    /// <summary>
    /// Helper methods for response messages
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Helper method for reading the response body
        /// </summary>
        /// <typeparam name="TType">Target deserialization type</typeparam>
        /// <param name="message">Response message</param>
        /// <returns>Deserialized body</returns>
        public static async Task<TType> ReadBody<TType>(this HttpResponseMessage message)
        {

            var rawContent = await message.Content.ReadAsStringAsync();
            TType models = JsonConvert.DeserializeObject<TType>(rawContent);
            return models;
        }
    }
}
