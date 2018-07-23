using System.Data.Entity.Migrations;
using System.Linq;
using Voyage.Core.Exceptions;
using System.Configuration;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Voyage.Data.Repositories.Client
{
    public class ClientRepository : BaseRepository<Models.Entities.Client>, IClientRepository
    {
        public ClientRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.Client Add(Models.Entities.Client model)
        {
            Context.Clients.Add(model);
            Context.SaveChanges();
            return model;
        }

        public async override Task<Models.Entities.Client> AddAsync(Models.Entities.Client model)
        {
            Context.Clients.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public override int Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return 0;

            Context.Clients.Remove(entity);
            return Context.SaveChanges();
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return 0;

            Context.Clients.Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public override Models.Entities.Client Get(object id)
        {
            return Context.Clients.Find(id);
        }

        public async override Task<Models.Entities.Client> GetAsync(object id)
        {
            if (Context.Clients is DbSet<Models.Entities.Client> dbSet)
            {
                return await dbSet.FindAsync(id);
            }

            return Get(id);
        }

        public override IQueryable<Models.Entities.Client> GetAll()
        {
            return Context.Clients;
        }

        public override Models.Entities.Client Update(Models.Entities.Client model)
        {
            Context.Clients.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }

        public async override Task<Models.Entities.Client> UpdateAsync(Models.Entities.Client model)
        {
            Context.Clients.AddOrUpdate(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public bool ValidateClient(string clientIdentifier, string clientSecret)
        {
            return Context.Clients.Any(k => k.ClientIdentifier.Equals(clientIdentifier) && k.ClientSecret.Equals(clientSecret) && !k.IsDeleted);
        }

        public Models.Entities.Client GetByName(string clientIdentifier)
        {
            return Context.Clients.FirstOrDefault(k => k.ClientIdentifier.Equals(clientIdentifier));
        }

        /// <summary>
        /// This method updates the field 'FailedLoginAttempts' and locks the account if the number of attempts have exceeded the number mentioned in the web.config key 'MaxFailedAccessAttemptsBeforeLockout'
        /// Also, resets the count to 0 if, the 'isIncrement' is set to false
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isIncrement"></param>
        public void UpdateFailedLoginAttempts(string id, bool isIncrement)
        {
            var client = Get(id);
            if (client == null)
                throw new NotFoundException();

            // Get the maxFailedAttempts from the config file. In case the MaxFailedAccessAttemptsBeforeLockout is not a number, this will throw an error and fail.
            var maxFailedLoginAttempts = int.Parse(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]);

            // in case of incorrect attempt
            if (isIncrement)
            {
                // in case this attempt was the last attempt remaining, lock the account. This will hold good even if the MaxFailedAccessAttemptsBeforeLockout is set to zero.
                if (maxFailedLoginAttempts - client.FailedLoginAttempts <= 1)
                {
                    client.FailedLoginAttempts = 0;
                    client.IsAccountLocked = true;
                }

                // in case there are more attempts remaining, increment the FailedLoginAttempts.
                else if (maxFailedLoginAttempts - client.FailedLoginAttempts > 1)
                {
                    client.FailedLoginAttempts++;
                }
            }

            // Reset the FailedLoginAttempts to zero.
            else
            {
                client.FailedLoginAttempts = 0;
            }

            Update(client);
        }

        /// <summary>
        /// Check if the client has been locked out or not.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsLockedOut(string id)
        {
            var client = Get(id);
            if (client == null)
                throw new NotFoundException();
            return client.IsAccountLocked;
        }

        /// <summary>
        /// Unlock the client if invoked.
        /// </summary>
        /// <param name="id"></param>
        public void UnlockClient(string id)
        {
            var client = Get(id);
            if (client == null)
                return;

            client.IsAccountLocked = true;
            Update(client);
        }
    }
}
