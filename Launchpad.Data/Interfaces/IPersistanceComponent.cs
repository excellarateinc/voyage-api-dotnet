namespace Launchpad.Data.Interfaces
{
    public interface IPersistanceComponent
    {
        int SaveChanges();

        ITransaction BeginTransaction();
    }
}
