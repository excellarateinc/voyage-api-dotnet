using Launchpad.Models.EntityFramework;
using Launchpad.Data.Interfaces;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Launchpad.Data
{


    public class LaunchpadDataContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, ILaunchpadDataContext
    {

        #region DbSets

        public IDbSet<Widget> Widgets { get; set; }
        public IDbSet<LaunchpadLog> Logs {get;set;}

        public IDbSet<RoleClaim> RoleClaims { get; set; }
        #endregion 


        public LaunchpadDataContext() : base("LaunchpadDataContext")
        {

        }

        /// <summary>
        /// Pass in the connection string to eliminate the "magic string" above
        /// </summary>
        /// <param name="connectionString">Connection string from the web.config or app.config</param>
        public LaunchpadDataContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
