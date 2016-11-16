using Launchpad.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Services
{
    public class PathProviderService : IPathProviderService
    {
        public string LocalPath { get; private set; }

        public PathProviderService(string localPath)
        {
            LocalPath = localPath;
        }
    }
}
