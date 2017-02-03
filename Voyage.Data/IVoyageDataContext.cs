using System.Data.Entity;
using Voyage.Models.Entities;

namespace Voyage.Data
{
    public interface IVoyageDataContext
    {
        IDbSet<ApplicationLog> Logs { get; set; }

        IDbSet<RoleClaim> RoleClaims { get; set; }

        IDbSet<ApplicationUser> Users { get; set; }

        IDbSet<ActivityAudit> ActivityAudits { get; set; }

        IDbSet<UserPhone> UserPhones { get; set; }

        int SaveChanges();
    }
}
