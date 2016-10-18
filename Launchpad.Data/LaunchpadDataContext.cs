using Launchpad.Models;
using Launchpad.Data.Interfaces;
using System.Data.Entity;

namespace Launchpad.Data
{
  

    public class LaunchpadDataContext : DbContext, ILaunchpadDataContext
    {

        #region DbSets

        public IDbSet<Widget> Widgets { get; set; }

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
