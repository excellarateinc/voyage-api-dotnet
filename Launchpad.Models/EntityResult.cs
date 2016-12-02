using System.Collections.Generic;

namespace Launchpad.Models
{
    /// <summary>
    /// Represents the result of a service operation which involves an entity such as a User
    /// </summary>
    public class EntityResult
    {
        /// <summary>
        /// Collection of errors that occured while processing the service call
        /// </summary>
        public List<string> Errors { get; }

        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// Indicates if the opearation was missing a specified entity (404 scenario)
        /// </summary>
        public bool IsEntityNotFound { get; }

        public EntityResult(bool succeeded, bool isEntityNotFound, params string[] errors)
        {
            Succeeded = succeeded;
            IsEntityNotFound = isEntityNotFound;
            Errors = new List<string>();
            Errors.AddRange(errors);
        }

        /// <summary>
        /// Adds an error message that includes both a code and description
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        /// <returns></returns>
        public EntityResult WithErrorCodeMessage(string code, string message)
        {
            Errors.Add(string.Format("{0}::{1}", code, message));
            return this;
        }

        /// <summary>
        /// Adds an error message for the common scenario of an entity not being found
        /// </summary>
        /// <param name="id">ID of the entity</param>
        /// <returns></returns>
        public EntityResult WithEntityNotFound(object id)
        {
            return this.WithErrorCodeMessage(Constants.ErrorCodes.EntityNotFound, $"Could not locate entity with ID {id}");
        }

    }

    /// <summary>
    ///  Represents the result of a service operation which includes a model result
    /// </summary>
    /// <typeparam name="TModel">Model type</typeparam>
    public class EntityResult<TModel> : EntityResult where TModel : class
    {
        /// <summary>
        /// Model type - this can be a single entity or a collection
        /// </summary>
        public TModel Model { get; }

        public EntityResult(TModel model, bool succeeded, bool isEntityNotFound, params string[] errors)
            : base(succeeded, isEntityNotFound, errors)
        {
            Model = model;
        }

        /// <summary>
        /// Adds an error message that includes both a code and description
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        /// <returns></returns>
        public new EntityResult<TModel> WithErrorCodeMessage(string code, string message)
        {
            Errors.Add(string.Format("{0}::{1}", code, message));
            return this;
        }

        /// <summary>
        /// Adds an error message for the common scenario of an entity not being found
        /// </summary>
        /// <param name="id">ID of the entity</param>
        /// <returns></returns>
        public new EntityResult<TModel> WithEntityNotFound(object id)
        {
            return this.WithErrorCodeMessage(Constants.ErrorCodes.EntityNotFound, $"Could not locate entity with ID {id}");
        }
    }

}
