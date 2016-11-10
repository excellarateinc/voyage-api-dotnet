using Moq;
using Ploeh.AutoFixture;
using System.Threading;

namespace Launchpad.UnitTests.Common
{
    public abstract class BaseUnitTest
    {
        /// <summary>
        /// Provides access to a Mock Repository in unit tests
        /// </summary>
        protected MockRepository Mock;

        protected CancellationToken CreateCancelToken()
        {
            return new CancellationToken();
        }

        /// <summary>
        /// Provides access to Autofixture in unit tests
        /// </summary>
        protected Fixture Fixture;

        public BaseUnitTest()
        {
            Fixture = new Fixture();
            Mock = new MockRepository(MockBehavior.Strict);
        }
    }
}
