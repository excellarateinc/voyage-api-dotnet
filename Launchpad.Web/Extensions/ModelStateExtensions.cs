using Microsoft.AspNet.Identity;
using System.Web.Http.ModelBinding;

namespace Launchpad.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddErrors(this ModelStateDictionary state, IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                state.AddModelError(string.Empty, error);
            }
        }
    }
}