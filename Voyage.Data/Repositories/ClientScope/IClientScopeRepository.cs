﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voyage.Models;
using Voyage.Models.Entities;

namespace Voyage.Data.Repositories
{
    public interface IClientScopeRepository : IRepository<ClientScope>
    {
        List<ClientScope> GetClientScopesByClientId(string id);
    }
}
