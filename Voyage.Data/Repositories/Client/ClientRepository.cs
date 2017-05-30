using System.Data.Entity.Migrations;
using System.Linq;
using Voyage.Core.Exceptions;
using System.Configuration;

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

        public override void Delete(object id)
        {
            var entity = Context.Clients.Find(id);
            Context.Clients.Remove(entity);
        }

        public override Models.Entities.Client Get(object id)
        {
            return Context.Clients.Find(id);
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

        public bool ValidateClient(string clientIdentifier, string clientSecret)
        {
            var client = Context.Clients.Where(k => k.ClientIdentifier.Equals(clientIdentifier) && k.ClientSecret.Equals(clientSecret) && !k.IsDeleted);
            return client != null ? true : false;
        }

        public Models.Entities.Client GetByName(string clientIdentifier)
        {
            return Context.Clients.FirstOrDefault(k => k.ClientIdentifier.Equals(clientIdentifier));
        }

        public void UpdateFailedLoginAttempts(string id, bool isIncrement)
        {
            var client = Get(id);
            if (client == null)
                throw new NotFoundException();

            int maxFailedLoginAttempts = int.Parse(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]);

            if (isIncrement)
            {
                if (maxFailedLoginAttempts - client.FailedLoginAttempts <= 1)
                {
                    client.FailedLoginAttempts = 0;
                    client.IsAccountLocked = true;
                }
                else if (maxFailedLoginAttempts - client.FailedLoginAttempts > 1)
                {
                    client.FailedLoginAttempts++;
                }
            }
            else
            {
                client.FailedLoginAttempts = 0;
            }

            Update(client);
        }

        public bool IsLockedOut(string id)
        {
            var client = Get(id);
            if (client == null)
                throw new NotFoundException();
            return client.IsAccountLocked;
        }

        public void UnlockClient(string id)
        {
            var client = Get(id);
            if (client == null)
            {
                client.IsAccountLocked = true;
                Update(client);
            }
        }
    }
}
