using System;
using System.Threading;
using Moq;
using Ploeh.AutoFixture;

namespace Voyage.Api.UnitTests.Common
{
    public abstract class BaseUnitTest : IDisposable
    {
#pragma warning disable SA1306 // Field names must begin with lower-case letter

        /// <summary>
        /// Provides access to a Mock Repository in unit tests
        /// </summary>
        protected MockRepository Mock;
#pragma warning restore SA1306 // Field names must begin with lower-case letter

        public void Dispose()
        {
            Mock.VerifyAll();
        }

        protected CancellationToken CreateCancelToken()
        {
            return new CancellationToken();
        }

#pragma warning disable SA1306 // Field names must begin with lower-case letter

        /// <summary>
        /// Provides access to Autofixture in unit tests
        /// </summary>
        protected Fixture Fixture;
#pragma warning restore SA1306 // Field names must begin with lower-case letter

        protected BaseUnitTest()
        {
            Fixture = new Fixture();
            Mock = new MockRepository(MockBehavior.Strict);
        }
    }
}
