using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Ploeh.AutoFixture;
using Launchpad.UnitTests.Common;
using Launchpad.Web.Middleware;

namespace Launchpad.Web.UnitTests.Middleware
{
    public class ActivityAuditMiddlewareTests : BaseUnitTest
    {
        private ActivityAuditMiddleware _middleware;

        public ActivityAuditMiddlewareTests()
        {
            
        }

    }
}
