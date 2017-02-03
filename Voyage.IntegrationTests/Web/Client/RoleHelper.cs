using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Voyage.IntegrationTests.Web.Extensions;
using Voyage.Models;

namespace Voyage.IntegrationTests.Web.Client
{
    public class RoleHelper : DataHelper<RoleModel>
    {
        private List<RoleModel> _entities = Enumerable.Empty<RoleModel>().ToList();

        public override IEnumerable<RoleModel> GetAllEntities()
        {
            return _entities;
        }

        public override RoleModel GetSingleEntity()
        {
            return _entities.First();
        }

        public async Task<RoleModel> CreateRoleAsync()
        {
            var roleModel = new RoleModel { Name = Guid.NewGuid().ToString() };

            var httpRequestMessage = CreateSecureRequest(HttpMethod.Post, $"/api/v1/roles")
                .WithJson(roleModel);
            var httpResponseMessage = await Client.SendAsync(httpRequestMessage);
            var responseModel = await httpResponseMessage.ReadBody<RoleModel>();
            return responseModel;
        }

        public override async Task Refresh()
        {
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, "/api/v1/roles");
            var response = await Client.SendAsync(httpRequestMessage);
            _entities = (await response.ReadBody<RoleModel[]>()).ToList();
        }
    }
}
