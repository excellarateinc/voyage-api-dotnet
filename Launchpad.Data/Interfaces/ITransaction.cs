using System;

namespace Launchpad.Data.Interfaces
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
