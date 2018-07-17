using AntPathMatching;
using System.Collections.Generic;

namespace Voyage.Services.Ant
{
    public interface IAntService
    {
        IAnt GetAntPath(string pattern);

        IAnt[] GetAntPaths(IEnumerable<string> patterns);
    }
}
