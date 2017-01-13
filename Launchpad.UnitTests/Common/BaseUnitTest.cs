using System;
using System.Threading;

using Moq;

using Ploeh.AutoFixture;

namespace Launchpad.UnitTests.Common
{
    public abstract class BaseUnitTest : IDisposable
    {
        /// <summary>
        /// Provides access to a Mock Repository in unit tests
        /// </summary>
        protected MockRepository Mock;

        protected CancellationToken CreateCancelToken()
        {
            return new CancellationToken();
        }

        public void Dispose()
        {
            Mock.VerifyAll();
        }

        /// <summary>
        /// Provides access to Autofixture in unit tests
        /// </summary>
        protected Fixture Fixture;

        protected BaseUnitTest()
        {
            Fixture = new Fixture();
            Mock = new MockRepository(MockBehavior.Strict);
        }
    }
}
