using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace Voyage.Data.Repositories.Notification
{
    public class NotificationRepository : BaseRepository<Models.Entities.Notification>, INotificationRepository
    {
        public NotificationRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.Notification Add(Models.Entities.Notification model)
        {
            Context.Notifications.Add(model);
            SaveChanges();
            return model;
        }

        public async override Task<Models.Entities.Notification> AddAsync(Models.Entities.Notification model)
        {
            Context.Notifications.Add(model);
            await SaveChangesAsync();
            return model;
        }

        public override int Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return 0;

            Context.Notifications.Remove(entity);
            return SaveChanges();
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return 0;

            Context.Notifications.Remove(entity);
            return await SaveChangesAsync();
        }

        public override Models.Entities.Notification Get(object id)
        {
            return Context.Notifications.Find(id);
        }

        public async override Task<Models.Entities.Notification> GetAsync(object id)
        {
            if (Context.Notifications is DbSet<Models.Entities.Notification> dbSet)
            {
                return await dbSet.FindAsync(id);
            }

            return Context.Notifications.Find(id);
        }

        public override IQueryable<Models.Entities.Notification> GetAll()
        {
            return Context.Notifications;
        }

        public override Models.Entities.Notification Update(Models.Entities.Notification model)
        {
            Context.Notifications.AddOrUpdate(model);
            SaveChanges();
            return model;
        }

        public async override Task<Models.Entities.Notification> UpdateAsync(Models.Entities.Notification model)
        {
            Context.Notifications.AddOrUpdate(model);
            await SaveChangesAsync();
            return model;
        }
    }
}
