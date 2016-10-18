using Moq;
using Ploeh.AutoFixture;

namespace Launchpad.UnitTests.Common
{
    public abstract class BaseUnitTest
    {
        /// <summary>
        /// Provides access to a Mock Repository in unit tests
        /// </summary>
        protected MockRepository Mock;

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
