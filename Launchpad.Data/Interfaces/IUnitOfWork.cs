namespace Launchpad.Data.Interfaces
{
    public interface IUnitOfWork
    {
        ITransaction Begin();

        int SaveChanges();
    }
}
