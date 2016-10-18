using Launchpad.Models;
using System.Data.Entity;

namespace Launchpad.Data
{
  

    public class LaunchpadDataContext : DbContext 
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
        /// <param name="connectionStringName">Name of the connection string from the web.config or app.config</param>
        public LaunchpadDataContext(string connectionStringName) : base(connectionStringName)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
