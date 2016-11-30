using Launchpad.Models;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace Launchpad.Services
{
    public abstract class EntityResultService
    {
        protected EntityResult Missing(object id)
        {
            return new EntityResult(false, true)
              .WithMissingEntity(id);
        }

        protected EntityResult<TModel> Missing<TModel>(object id)
            where TModel : class
        {
            return new EntityResult<TModel>(null, false, true)
              .WithMissingEntity(id);
        }


        protected EntityResult<TModel> Success<TModel>(TModel model)
            where TModel : class
        {
            return new EntityResult<TModel>(model, true, false);
        }

        protected EntityResult Success()
        {
            return new EntityResult(true, false);
        }


        protected EntityResult<TModel> FromIdentityResult<TModel>(IdentityResult result, TModel model)
             where TModel : class
        {
            return new EntityResult<TModel>(model, result.Succeeded, false, result.Errors.ToArray());
        }

        protected EntityResult FromIdentityResult(IdentityResult result)
        {
            return new EntityResult(result.Succeeded, false, result.Errors.ToArray());
        }

    }
}
