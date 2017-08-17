using System.Data.Entity.Migrations;
using System.Linq;
using Voyage.Core.Exceptions;

namespace Voyage.Data.Repositories.SecurityQuestion
{
    public class SecurityQuestionRepository : BaseRepository<Models.Entities.SecurityQuestion>, ISecurityQuestionRepository
    {
        public SecurityQuestionRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.SecurityQuestion Add(Models.Entities.SecurityQuestion model)
        {
            Context.SecurityQuestions.Add(model);
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

        public override Models.Entities.SecurityQuestion Get(object id)
        {
            return Context.SecurityQuestions.FirstOrDefault(u => u.IsDeleted == false);
        }

        public override IQueryable<Models.Entities.SecurityQuestion> GetAll()
        {
            return Context.SecurityQuestions;
        }

        public override Models.Entities.SecurityQuestion Update(Models.Entities.SecurityQuestion model)
        {
            Context.SecurityQuestions.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}
