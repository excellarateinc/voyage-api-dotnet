using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Voyage.Web.UnitTests.Middleware
{
    /// <summary>
    /// Request ID is not being populated into the environment dictionary. For these tests, add this fake
    /// middleware in to compensate.
    /// </summary>
    public class TestEnvironmentMiddleware : OwinMiddleware
    {
        private readonly Dictionary<string, object> _env;

        public TestEnvironmentMiddleware(OwinMiddleware next, Dictionary<string, object> env)
            : base(next)
        {
            _env = env;
        }

        public override async Task Invoke(IOwinContext context)
        {
            foreach (var pair in _env)
            {
                context.Environment.Add(pair.Key, pair.Value);
            }

            await Next.Invoke(context);
        }
    }
}
