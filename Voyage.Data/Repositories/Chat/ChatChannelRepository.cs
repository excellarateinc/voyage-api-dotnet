using System.Data.Entity.Migrations;
using System.Linq;

namespace Voyage.Data.Repositories.Chat
{
    public class ChatChannelRepository : BaseRepository<Models.Entities.ChatChannel>, IChatChannelRepository
    {
        public ChatChannelRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.ChatChannel Add(Models.Entities.ChatChannel model)
        {
            Context.ChatChannels.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;

            Context.ChatChannels.Remove(entity);
            Context.SaveChanges();
        }

        public override Models.Entities.ChatChannel Get(object id)
        {
            return Context.ChatChannels.Find(id);
        }

        public override IQueryable<Models.Entities.ChatChannel> GetAll()
        {
            return Context.ChatChannels;
        }

        public override Models.Entities.ChatChannel Update(Models.Entities.ChatChannel model)
        {
            Context.ChatChannels.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
