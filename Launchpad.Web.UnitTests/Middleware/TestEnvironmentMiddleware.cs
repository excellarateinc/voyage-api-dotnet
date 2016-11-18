using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Web.UnitTests.Middleware
{
    /// <summary>
    /// Request ID is not being populated into the environment dictionary. For these tests, add this fake
    /// middleware in to compensate. 
    /// </summary>
    public class TestEnvironmentMiddleware : OwinMiddleware
    {
        Dictionary<string, object> _env;
        public TestEnvironmentMiddleware(OwinMiddleware next, Dictionary<string, object> env) : base(next)
        {
            _env = env;

        }
        public async override Task Invoke(IOwinContext context)
        {
            foreach (var pair in _env)
            {
                context.Environment.Add(pair.Key, pair.Value);
            }
            await Next.Invoke(context);

        }
    }
}
