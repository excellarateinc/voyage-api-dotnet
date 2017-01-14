using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Launchpad.IntegrationTests.Web.Extensions;
using Launchpad.Models;

namespace Launchpad.IntegrationTests.Web.Client
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

        public async Task<UserModel> CreateUserAsync()
        {
            var userModel = new UserModel
            {
                Username = $"userhelper{DateTime.Now.Ticks}@test.com",
                Email = $"userhelper{DateTime.Now.Ticks}@test.com",
                FirstName = "Theodore",
                LastName = "TestsALot",
                IsActive = true
            };

            var request = CreateSecureRequest(HttpMethod.Post, "/api/v1/users").WithJson(userModel);
            var response = await Client.SendAsync(request);

            var responseModel = await response.ReadBody<UserModel>();
            return responseModel;
        }
    }
}
