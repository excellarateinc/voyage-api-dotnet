using System.Data.Entity.Migrations;
using System.Linq;
using Voyage.Core.Exceptions;

namespace Voyage.Data.Repositories.UserSecurityQuestion
{
    public class UserSecurityQuestionRepository : BaseRepository<Models.Entities.UserSecurityQuestion>, IUserSecurityQuestionRepository
    {
        public UserSecurityQuestionRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.UserSecurityQuestion Add(Models.Entities.UserSecurityQuestion model)
        {
            Context.UserSecurityQuestions.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                throw new NotFoundException();

            entity.IsDeleted = true;
            Context.SaveChanges();
        }

        public override Models.Entities.UserSecurityQuestion Get(object id)
        {
            return Context.UserSecurityQuestions.FirstOrDefault(u => u.IsDeleted == false);
        }

        public override IQueryable<Models.Entities.UserSecurityQuestion> GetAll()
        {
            return Context.UserSecurityQuestions;
        }

        public override Models.Entities.UserSecurityQuestion Update(Models.Entities.UserSecurityQuestion model)
        {
            Context.UserSecurityQuestions.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
