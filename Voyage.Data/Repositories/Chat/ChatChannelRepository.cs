using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

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

        public async override Task<Models.Entities.ChatChannel> AddAsync(Models.Entities.ChatChannel model)
        {
            Context.ChatChannels.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public override int Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return 0;

            Context.ChatChannels.Remove(entity);
            return Context.SaveChanges();
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return 0;

            Context.ChatChannels.Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public override Models.Entities.ChatChannel Get(object id)
        {
            return Context.ChatChannels.Find(id);
        }

        public async override Task<Models.Entities.ChatChannel> GetAsync(object id)
        {
            if (Context.ChatChannels is DbSet<Models.Entities.ChatChannel> dbSet)
            {
                return await dbSet.FindAsync(id);
            }

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

        public async override Task<Models.Entities.ChatChannel> UpdateAsync(Models.Entities.ChatChannel model)
        {
            Context.ChatChannels.AddOrUpdate(model);
            await Context.SaveChangesAsync();
            return model;
        }
    }
}
