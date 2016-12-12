using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

namespace Launchpad.Web.AuthProviders
{
    public interface ILoginOrchestrator
    {
        string TokenPath { get; }

        bool ValidateRequest(Microsoft.Owin.IReadableStringCollection parameters);

        Task<bool> ValidateCredential(OAuthGrantResourceOwnerCredentialsContext context);
    }
}
