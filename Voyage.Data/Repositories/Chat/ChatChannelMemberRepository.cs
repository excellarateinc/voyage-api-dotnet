using System.Data.Entity.Migrations;
using System.Linq;

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

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;

            Context.ChatChannelMembers.Remove(entity);
            Context.SaveChanges();
        }

        public override Models.Entities.ChatChannelMember Get(object id)
        {
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
    }
}
