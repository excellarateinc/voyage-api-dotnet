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

        public async override Task<Models.Entities.Notification> AddAsync(Models.Entities.Notification model)
        {
            Context.Notifications.Add(model);
            await SaveChangesAsync();
            return model;
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return 0;

            Context.Notifications.Remove(entity);
            return await SaveChangesAsync();
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

        public async override Task<Models.Entities.Notification> UpdateAsync(Models.Entities.Notification model)
        {
            Context.Notifications.AddOrUpdate(model);
            await SaveChangesAsync();
            return model;
        }
    }
}
