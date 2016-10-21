using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Launchpad.Web.AppMembership
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(string connectionString) 
            : base(connectionString, throwIfV1Schema: true)
        {

        }

        /// <summary>
        /// Creates a new context -> TODO: Move to DI
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["LaunchpadDataContext"].ConnectionString;
            return new ApplicationDbContext(connectionString);
        }
    }
}