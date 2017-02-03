using System.Collections.Generic;
using System.Threading.Tasks;

namespace Voyage.IntegrationTests.Web.Client
{
    public abstract class DataHelper<TModel> : ApiConsumer
    {
        public abstract Task Refresh();

        public abstract TModel GetSingleEntity();

        public abstract IEnumerable<TModel> GetAllEntities();
    }
}
