using Launchpad.Models;
using Launchpad.Web.IntegrationTests.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Launchpad.Web.IntegrationTests.Client
{
    public class UserHelper : DataHelper<UserModel>
    {
        private List<UserModel> _entities = Enumerable.Empty<UserModel>().ToList();

        public override IEnumerable<UserModel> GetAllEntities()
        {
            return _entities;
        }

        public override UserModel GetSingleEntity()
        {
            return _entities.First();
        }

        public async override Task Refresh()
        {
            var httpRequestMessage = CreateSecureRequest(HttpMethod.Get, "/api/v1/users");
            var response = await Client.SendAsync(httpRequestMessage);
            _entities = (await response.ReadBody<UserModel[]>()).ToList();
        }
    }
}
