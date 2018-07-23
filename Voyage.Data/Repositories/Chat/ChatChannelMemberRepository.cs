using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace Voyage.Data.Repositories.Chat
{
    public class ChatChannelMemberRepository : BaseRepository<Models.Entities.ChatChannelMember>, IChatChannelMemberRepository
    {
        public ChatChannelMemberRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.ChatChannelMember Add(Models.Entities.ChatChannelMember model)
        {
            Context.ChatChannelMembers.Add(model);
            Context.SaveChanges();
            return model;
        }

        public async override Task<Models.Entities.ChatChannelMember> AddAsync(Models.Entities.ChatChannelMember model)
        {
            Context.ChatChannelMembers.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public override int Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return 0;

            Context.ChatChannelMembers.Remove(entity);
            return Context.SaveChanges();
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return 0;

            Context.ChatChannelMembers.Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public override Models.Entities.ChatChannelMember Get(object id)
        {
            return Context.ChatChannelMembers.Find(id);
        }

        public async override Task<Models.Entities.ChatChannelMember> GetAsync(object id)
        {
            if (Context.ChatChannelMembers is DbSet<Models.Entities.ChatChannelMember> dbSet)
            {
                return await dbSet.FindAsync(id);
            }

            return Context.ChatChannelMembers.Find(id);
        }

        public override IQueryable<Models.Entities.ChatChannelMember> GetAll()
        {
            return Context.ChatChannelMembers;
        }

        public override Models.Entities.ChatChannelMember Update(Models.Entities.ChatChannelMember model)
        {
            Context.ChatChannelMembers.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }

        public async override Task<Models.Entities.ChatChannelMember> UpdateAsync(Models.Entities.ChatChannelMember model)
        {
            Context.ChatChannelMembers.AddOrUpdate(model);
            await Context.SaveChangesAsync();
            return model;
        }
    }
}
