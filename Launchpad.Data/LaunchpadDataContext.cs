using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Launchpad.Core;
using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using Microsoft.AspNet.Identity.EntityFramework;
using Serilog;
using TrackerEnabledDbContext.Common.Models;
using TrackerEnabledDbContext.Identity;

namespace Launchpad.Data
{
    using System.Diagnostics.CodeAnalysis;

    public sealed class LaunchpadDataContext : TrackerIdentityContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, ILaunchpadDataContext
    {
        private readonly IIdentityProvider _identityProvider;
        private readonly ILogger _logger;

        #region DbSets

        public IDbSet<ActivityAudit> ActivityAudits { get; set; }

        public IDbSet<Widget> Widgets { get; set; }

        public IDbSet<ApplicationLog> Logs { get; set; }

        public IDbSet<RoleClaim> RoleClaims { get; set; }

        public IDbSet<UserPhone> UserPhones { get; set; }

        #endregion

        public LaunchpadDataContext() : base("LaunchpadDataContext")
        {
        }

        public LaunchpadDataContext(string connectionString, IIdentityProvider identityProvider, ILogger logger) : base(connectionString)
        {
            _identityProvider = identityProvider.ThrowIfNull(nameof(identityProvider));
            _logger = logger.ThrowIfNull(nameof(logger));

            // Configure the username factory for the auditing 
            ConfigureUsername(() => _identityProvider.GetUserName());
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            // Disable the default validation for users and roles otherwise
            // Create a new user with a deleted user's email address will 
            // throw an error.        
            if (entityEntry != null && entityEntry.State == EntityState.Added)
            {
                var errors = new List<DbValidationError>();
                if ((entityEntry.Entity is ApplicationUser) || (entityEntry.Entity is ApplicationRole))
                {
                    return new DbEntityValidationResult(entityEntry, errors);
                }

            }
            return base.ValidateEntity(entityEntry, items);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Disable EF migrations
            Database.SetInitializer<LaunchpadDataContext>(null);

            #region Boilerplate configuration
            // Migrations were not being generated corretly because the order in which the base was executing
            // the model configurations and then the attempt to rename the tables. As a result, easiest solution was to take the base code
            // move it here and make it explicit
            // https://aspnetidentity.codeplex.com/SourceControl/latest#src/Microsoft.AspNet.Identity.EntityFramework/IdentityDbContext.cs
            var user = modelBuilder.Entity<ApplicationUser>()
              .ToTable("User", Constants.Schemas.FrameworkTables);
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256);

            // CONSIDER: u.Email is Required if set on options?
            user.Property(u => u.Email).HasMaxLength(256);

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("UserRole", Constants.Schemas.FrameworkTables);

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("UserLogin", Constants.Schemas.FrameworkTables);

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("UserClaim", Constants.Schemas.FrameworkTables);

            var role = modelBuilder.Entity<ApplicationRole>()
                .ToTable("Role", Constants.Schemas.FrameworkTables);

            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
            #endregion

            // Register the other models
            modelBuilder.Configurations.AddFromAssembly(typeof(LaunchpadDataContext).Assembly);

            // Configure the namespace for the audit 
            modelBuilder.Entity<AuditLog>()
                .ToTable("AuditLog", Constants.Schemas.FrameworkTables);

            modelBuilder.Entity<AuditLogDetail>()
                .ToTable("AuditLogDetail", Constants.Schemas.FrameworkTables);

            modelBuilder.Entity<LogMetadata>()
                .ToTable("LogMetadata", Constants.Schemas.FrameworkTables);
        }

        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                return await base.SaveChangesAsync();
            }
            catch (DbEntityValidationException validationException)
            {
                // Log errors generated from attempting to commit changes
                var errorMessages = validationException.EntityValidationErrors
                    .SelectMany(entityError => entityError.ValidationErrors)
                    .Select(validationError => $"'{validationError.PropertyName}' has error '{validationError.ErrorMessage}'");

                _logger
                    .ForContext<LaunchpadDataContext>()
                    .Error(validationException, "({eventCode:l}) {validationErrors}", EventCodes.EntityValidation, string.Join(";", errorMessages));

                throw;
            }
        }
    }
}
