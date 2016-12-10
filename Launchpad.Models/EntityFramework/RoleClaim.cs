namespace Launchpad.Models.EntityFramework
{
    public class RoleClaim
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        
        public virtual ApplicationRole Role { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }


    }
}
