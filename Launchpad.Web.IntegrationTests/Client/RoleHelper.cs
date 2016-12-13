using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Launchpad.Web.IntegrationTests.Client
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

        public async override Task Refresh()
        {
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, "/api/v1/roles");
            var response = await Client.SendAsync(httpRequestMessage);
            _entities = (await response.ReadBody<RoleModel[]>()).ToList();
        }
    }
}
