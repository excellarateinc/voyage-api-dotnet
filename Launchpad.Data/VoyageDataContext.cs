﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Voyage.Core;
using Voyage.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Serilog;
using TrackerEnabledDbContext.Common.Models;
using TrackerEnabledDbContext.Identity;

namespace Voyage.Data
{
    public sealed class VoyageDataContext : TrackerIdentityContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, IVoyageDataContext
    {
        private readonly IIdentityProvider _identityProvider;
        private readonly ILogger _logger;

        public VoyageDataContext()
            : base("VoyageDataContext")
        {
        }

        public VoyageDataContext(string connectionString, IIdentityProvider identityProvider, ILogger logger)
            : base(connectionString)
        {
            _identityProvider = identityProvider.ThrowIfNull(nameof(identityProvider));
            _logger = logger.ThrowIfNull(nameof(logger));

            // Configure the username factory for the auditing
            ConfigureUsername(() => _identityProvider.GetUserName());
        }

        public IDbSet<ActivityAudit> ActivityAudits { get; set; }

        public IDbSet<ApplicationLog> Logs { get; set; }

        public IDbSet<RoleClaim> RoleClaims { get; set; }

        public IDbSet<UserPhone> UserPhones { get; set; }

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
                    .ForContext<VoyageDataContext>()
                    .Error(validationException, "({eventCode:l}) {validationErrors}", EventCodes.EntityValidation, string.Join(";", errorMessages));

                throw;
            }
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
            Database.SetInitializer<VoyageDataContext>(null);

            // Migrations were not being generated correctly because the order in which the base was executing
            // the model configurations and then the attempt to rename the tables. As a result, easiest solution was to take the base code
            // move it here and make it explicit
            // https://aspnetidentity.codeplex.com/SourceControl/latest#src/Microsoft.AspNet.Identity.EntityFramework/IdentityDbContext.cs
            var user = modelBuilder.Entity<ApplicationUser>()
              .ToTable("User");
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
                .ToTable("UserRole");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("UserLogin");

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("UserClaim");

            var role = modelBuilder.Entity<ApplicationRole>()
                .ToTable("Role");

            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);

            // Register the other models.
            modelBuilder.Configurations.AddFromAssembly(typeof(VoyageDataContext).Assembly);

            // Configure the namespace for the audit.
            modelBuilder.Entity<AuditLog>()
                .ToTable("AuditLog");

            modelBuilder.Entity<AuditLogDetail>()
                .ToTable("AuditLogDetail");

            modelBuilder.Entity<LogMetadata>()
                .ToTable("LogMetadata");
        }
    }
}
