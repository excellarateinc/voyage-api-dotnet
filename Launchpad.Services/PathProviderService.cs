using Launchpad.Services.Interfaces;

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
