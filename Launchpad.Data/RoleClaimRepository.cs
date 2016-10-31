﻿using Launchpad.Data.Interfaces;
using Launchpad.Models.EntityFramework;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Launchpad.Data
{
    public class RoleClaimRepository : BaseRepository<RoleClaim>, IRoleClaimRepository
    {
        public RoleClaimRepository(ILaunchpadDataContext context) : base(context)
        {
        }

        public override RoleClaim Add(RoleClaim model)
        {
            Context.RoleClaims.Add(model);
            Context.SaveChanges();
            return model;
        }

        public override void Delete(object id)
        {
            var entity = Get(id);
            Context.RoleClaims.Remove(entity);
            Context.SaveChanges();
        }

        public override RoleClaim Get(object id)
        {

            return Context.RoleClaims.Find(id);
        }

        public override IQueryable<RoleClaim> GetAll()
        {
            return Context.RoleClaims;
        }

        public IQueryable<RoleClaim> GetClaimsByRole(string roleName)
        {
            return Context.RoleClaims
                     .Where(_ => _.Role.Name == roleName);

        }

        public override RoleClaim Update(RoleClaim model)
        {
            Context.SaveChanges();
            return model;
        }
    }
}
