using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

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

        public async override Task<Models.Entities.ProfileImage> AddAsync(Models.Entities.ProfileImage model)
        {
            Context.ProfileImages.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public override int Delete(object id)
        {
            var entity = Get(id);
            if (entity == null)
                return 0;

            Context.ProfileImages.Remove(entity);
            return Context.SaveChanges();
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return 0;

            Context.ProfileImages.Remove(entity);
            return await Context.SaveChangesAsync();
        }

        public override Models.Entities.ProfileImage Get(object id)
        {
            return Context.ProfileImages.Find(id);
        }

        public async override Task<Models.Entities.ProfileImage> GetAsync(object id)
        {
            if (Context.ProfileImages is DbSet<Models.Entities.ProfileImage> dbSet)
            {
                return await dbSet.FindAsync(id);
            }

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

        public async override Task<Models.Entities.ProfileImage> UpdateAsync(Models.Entities.ProfileImage model)
        {
            Context.ProfileImages.AddOrUpdate(model);
            await Context.SaveChangesAsync();
            return model;
        }
    }
}