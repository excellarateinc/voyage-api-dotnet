using Microsoft.AspNet.Identity;

namespace Voyage.Services.Identity
{
    public class IdentityResultResponse
    {
        public IdentityResult IdentityResult { get; }

        public string Id { get; set; }

        public IdentityResultResponse(IdentityResult identityResult)
        {
            IdentityResult = identityResult;
        }
    }
}
