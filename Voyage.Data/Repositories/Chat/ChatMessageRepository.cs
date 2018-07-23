using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

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

        public async override Task<Models.Entities.ChatMessage> AddAsync(Models.Entities.ChatMessage model)
        {
            Context.ChatMessages.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public override int Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return 0;

            Context.ChatMessages.Remove(entity);
            return Context.SaveChanges();
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return 0;

            Context.ChatMessages.Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public override Models.Entities.ChatMessage Get(object id)
        {
            return Context.ChatMessages.Find(id);
        }

        public async override Task<Models.Entities.ChatMessage> GetAsync(object id)
        {
            if (Context.ChatMessages is DbSet<Models.Entities.ChatMessage> dbSet)
            {
                return await dbSet.FindAsync(id);
            }

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

        public async override Task<Models.Entities.ChatMessage> UpdateAsync(Models.Entities.ChatMessage model)
        {
            Context.ChatMessages.AddOrUpdate(model);
            await Context.SaveChangesAsync();
            return model;
        }
    }
}
