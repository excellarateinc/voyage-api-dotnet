using Microsoft.AspNet.Identity;

namespace Launchpad.Models
{
    public class IdentityResult<TModel>
    {
        public IdentityResult(IdentityResult result, TModel model)
        {
            this.Result = result;
            this.Model = model;
        }

        public IdentityResult Result { get; }

        public TModel Model { get; }
    }
}
