using System.Collections.Generic;
using System.Threading.Tasks;

namespace Launchpad.IntegrationTests.Web.Client
{
    public abstract class DataHelper<TModel> : ApiConsumer
    {
        public abstract Task Refresh();

        public abstract TModel GetSingleEntity();

        public abstract IEnumerable<TModel> GetAllEntities();
    }
}
