using System.Threading.Tasks;

namespace Voyage.Data.Repositories.Client
{
    public interface IClientRepository : IRepository<Voyage.Models.Entities.Client>
    {
        Task<bool> ValidateClientAsync(string clientIdentifier, string clientSecret);

        Task<Models.Entities.Client> GetByNameAsync(string clientIdentifier);

        Task UpdateFailedLoginAttemptsAsync(string id, bool isIncrement);

        Task<bool> IsLockedOutAsync(string id);

        Task UnlockClientAsync(string id);
    }
}
