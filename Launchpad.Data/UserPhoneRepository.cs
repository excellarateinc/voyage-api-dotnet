using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using System;
using System.Linq;

namespace Launchpad.Data
{
    public class UserPhoneRepository : BaseRepository<UserPhone>, IUserPhoneRepository
    {
        public UserPhoneRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public override UserPhone Add(UserPhone model)
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
        public override UserPhone Get(object id)
        {
            return Context.UserPhones.Find(id);
        }

        public override IQueryable<UserPhone> GetAll()
        {
            throw new NotImplementedException("Phone numbers are loaded via the user object");
        }

        public override UserPhone Update(UserPhone model)
        {
            throw new NotImplementedException("Updates are handled via the user object");
        }
    }
}
