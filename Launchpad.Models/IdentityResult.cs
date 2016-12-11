using Microsoft.AspNet.Identity;

namespace Launchpad.Models
{
    public class IdentityResult<TModel>
    {
        public IdentityResult(IdentityResult result, TModel model)
        {
            Result = result;
            Model = model;
        }

        public IdentityResult Result { get; }

        public TModel Model { get; }
    }
}
