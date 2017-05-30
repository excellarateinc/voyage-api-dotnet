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

        IDbSet<Client> Clients { get; set; }

        IDbSet<ClientRole> ClientRoles { get; set; }

        IDbSet<ClientScope> ClientScopes { get; set; }

        IDbSet<ClientScopeType> ClientScopeTypes { get; set; }

        int SaveChanges();
    }
}
