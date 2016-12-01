using Launchpad.Models;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace Launchpad.Services
{
    /// <summary>
    /// Base class that helps create EntityResults
    /// </summary>
    public abstract class EntityResultService
    {
        /// <summary>
        /// Creates a failure as a result of a missing entity
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>EntityResult</returns>
        protected EntityResult NotFound(object id)
        {
            return new EntityResult(false, true)
              .WithEntityNotFound(id);
        }

        /// <summary>
        /// Creates a failure as a result of a missing entity
        /// </summary>
        /// <typeparam name="TModel">Type of the model</typeparam>
        /// <param name="id">Entity ID</param>
        /// <returns>EntityResult<TModel></returns>
        protected EntityResult<TModel> NotFound<TModel>(object id)
            where TModel : class
        {
            return new EntityResult<TModel>(null, false, true)
              .WithEntityNotFound(id);
        }

        /// <summary>
        /// Creates a successful result with the specified model
        /// </summary>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <param name="model">Model</param>
        /// <returns>EntityResult<TModel></returns>
        protected EntityResult<TModel> Success<TModel>(TModel model)
            where TModel : class
        {
            return new EntityResult<TModel>(model, true, false);
        }

        /// <summary>
        /// Craetes a successful result 
        /// </summary>
        /// <returns>EntityResult</returns>
        protected EntityResult Success()
        {
            return new EntityResult(true, false);
        }


        /// <summary>
        /// Converts IdentityResult to an EntityResult 
        /// </summary>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <param name="result">IdentityResult</param>
        /// <param name="model">Model</param>
        /// <returns>EntityResult<TModel></returns>
        protected EntityResult<TModel> FromIdentityResult<TModel>(IdentityResult result, TModel model)
             where TModel : class
        {
            return new EntityResult<TModel>(model, result.Succeeded, false, result.Errors.ToArray());
        }

        /// <summary>
        /// Converts IdentityResult to an EntityResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected EntityResult FromIdentityResult(IdentityResult result)
        {
            return new EntityResult(result.Succeeded, false, result.Errors.ToArray());
        }

    }
}
