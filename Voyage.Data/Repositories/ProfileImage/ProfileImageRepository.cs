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

        public async override Task<Models.Entities.ProfileImage> AddAsync(Models.Entities.ProfileImage model)
        {
            Context.ProfileImages.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public async override Task<int> DeleteAsync(object id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return 0;

            Context.ProfileImages.Remove(entity);
            return await Context.SaveChangesAsync();
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

        public async override Task<Models.Entities.ProfileImage> UpdateAsync(Models.Entities.ProfileImage model)
        {
            Context.ProfileImages.AddOrUpdate(model);
            await Context.SaveChangesAsync();
            return model;
        }
    }
}