using AntPathMatching;
using System.Collections.Generic;
using System.Linq;

namespace Voyage.Services.Ant
{
    public class AntService : IAntService
    {
        public IAnt GetAntPath(string pattern) => new AntPathMatching.Ant(pattern);

        public IAnt[] GetAntPaths(IEnumerable<string> patterns)
        {
            return patterns?.Select(x => new AntPathMatching.Ant(x)).ToArray();
        }
    }
}
