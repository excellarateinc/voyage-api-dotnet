using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Launchpad.Web.IntegrationTests.Client
{
    public class WidgetHelper : DataHelper<WidgetModel>
    {
        private List<WidgetModel> _entities = Enumerable.Empty<WidgetModel>().ToList();

        public override IEnumerable<WidgetModel> GetAllEntities()
        {
            return _entities;
        }

        public override WidgetModel GetSingleEntity()
        {
            return _entities.First();
        }

        public async override Task Refresh()
        {
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, "/api/v1/widgets");
            var response = await Client.SendAsync(httpRequestMessage);
            _entities = (await response.ReadBody<WidgetModel[]>()).ToList();
        }

        public async Task<WidgetModel> CreateWidgetAsync()
        {
            var widgetModel = new WidgetModel { Name = Guid.NewGuid().ToString() };

            var httpRequestMessage = CreateSecureRequest(HttpMethod.Post, $"/api/v1/widgets")
                .WithJson(widgetModel);
            var httpResponseMessage = await Client.SendAsync(httpRequestMessage);
            var responseModel = await httpResponseMessage.ReadBody<WidgetModel>();
            return responseModel;
        }
    }
}
