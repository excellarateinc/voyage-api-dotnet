using System.Data.Entity.Migrations;
using System.Linq;

namespace Voyage.Data.Repositories.ProfileImage
{
    public class ProfileImageRepository : BaseRepository<Models.Entities.ProfileImage>, IProfileImageRepository
    {
        public ProfileImageRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.ProfileImage Add(Models.Entities.ProfileImage model)
        {
            Context.ProfileImages.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return;

            Context.ProfileImages.Remove(entity);
            Context.SaveChanges();
        }

        public override Models.Entities.ProfileImage Get(object id)
        {
            return Context.ProfileImages.Find(id);
        }

        public override IQueryable<Models.Entities.ProfileImage> GetAll()
        {
            return Context.ProfileImages;
        }

        public override Models.Entities.ProfileImage Update(Models.Entities.ProfileImage model)
        {
            Context.ProfileImages.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
        }
    }
}