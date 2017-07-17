using System.Data.Entity.Migrations;
using System.Linq;

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
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;

            Context.Notifications.Remove(entity);
            Context.SaveChanges();
        }

        public override Models.Entities.Notification Get(object id)
        {
            return Context.Notifications.Find(id);
        }

        public override IQueryable<Models.Entities.Notification> GetAll()
        {
            return Context.Notifications;
        }

        public override Models.Entities.Notification Update(Models.Entities.Notification model)
        {
            Context.Notifications.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
