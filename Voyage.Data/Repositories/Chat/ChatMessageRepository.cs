using System.Data.Entity.Migrations;
using System.Linq;

namespace Voyage.Data.Repositories.Chat
{
    public class ChatMessageRepository : BaseRepository<Models.Entities.ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.ChatMessage Add(Models.Entities.ChatMessage model)
        {
            Context.ChatMessages.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;

            Context.ChatMessages.Remove(entity);
            Context.SaveChanges();
        }

        public override Models.Entities.ChatMessage Get(object id)
        {
            return Context.ChatMessages.Find(id);
        }

        public override IQueryable<Models.Entities.ChatMessage> GetAll()
        {
            return Context.ChatMessages;
        }

        public override Models.Entities.ChatMessage Update(Models.Entities.ChatMessage model)
        {
            Context.ChatMessages.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
