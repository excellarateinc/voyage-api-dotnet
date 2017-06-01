using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voyage.Data.Repositories
{
    public interface IClientRepository : IRepository<Voyage.Models.Entities.Client>
    {
        bool ValidateClient(string clientIdentifier, string clientSecret);

        Models.Entities.Client GetByName(string clientIdentifier);

        void UpdateFailedLoginAttempts(string id, bool isIncrement);

        bool IsLockedOut(string id);

        void UnlockClient(string id);
    }
}
