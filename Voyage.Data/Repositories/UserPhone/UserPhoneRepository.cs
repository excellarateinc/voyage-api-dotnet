using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Voyage.Data.Repositories.UserPhone
{
    public class UserPhoneRepository : BaseRepository<Models.Entities.UserPhone>, IUserPhoneRepository
    {
        public UserPhoneRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.UserPhone Add(Models.Entities.UserPhone model)
        {
            throw new NotImplementedException("Phone numbers are added via user object");
        }

        /// <summary>
        /// Deletes a phone number from the database
        /// </summary>
        /// <param name="id">Phone number ID</param>
        public override void Delete(object id)
        {
            var entity = Context.UserPhones.Find(id);
            Context.UserPhones.Remove(entity);
        }

        /// <summary>
        /// Fetch a phone number ID
        /// </summary>
        /// <param name="id">Phone number ID</param>
        /// <returns>Phone Number</returns>
        public override Models.Entities.UserPhone Get(object id)
        {
            return Context.UserPhones.Find(id);
        }

        public override IQueryable<Models.Entities.UserPhone> GetAll()
        {
            throw new NotImplementedException("Phone numbers are loaded via the user object");
        }

        public override Models.Entities.UserPhone Update(Models.Entities.UserPhone model)
        {
            Context.UserPhones.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
