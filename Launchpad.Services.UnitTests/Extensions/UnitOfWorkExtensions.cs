using Launchpad.Data.Interfaces;
using Moq;

namespace Launchpad.Services.UnitTests.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static Mock<ITransaction> MockTransaction(this MockRepository repo)
        {
            var mockTransaction = repo.Create<ITransaction>();
            mockTransaction.Setup(_ => _.Commit());
            mockTransaction.Setup(_ => _.Dispose());
            return mockTransaction;
        }

        public static void SetupBegin(this Mock<IUnitOfWork> unitOfWork, ITransaction transaction)
        {
            unitOfWork.Setup(_ => _.Begin()).Returns(transaction);
        }

        public static void SetupTransaction(this Mock<IUnitOfWork> unitOfWork, ITransaction transaction)
        {
            unitOfWork.SetupBegin(transaction);
            unitOfWork.Setup(_ => _.SaveChanges()).Returns(1);
        }
    }
}
