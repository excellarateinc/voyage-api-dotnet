using System.Collections.Generic;

namespace Launchpad.Models
{
    public class EntityResult
    {
        public List<string> Errors { get; }

        public bool Succeeded { get; }

        public bool IsMissingEntity { get; }

        public EntityResult(bool succeeded, bool isMissingEntity, params string[] errors)
        {
            Succeeded = succeeded;
            IsMissingEntity = isMissingEntity;
            Errors = new List<string>();
            Errors.AddRange(errors);
        }

        public EntityResult WithErrorCodeMessage(string code, string message)
        {
            Errors.Add(string.Format("{0}::{1}", code, message));
            return this;
        }

        public EntityResult WithMissingEntity(object id)
        {
            return this.WithErrorCodeMessage(Constants.ErrorCodes.EntityNotFound, $"Could not locate entity with ID {id}");
        }

    }

    public class EntityResult<TModel> : EntityResult where TModel : class
    {
        public TModel Model { get; }

        public EntityResult(TModel model, bool succeeded, bool isMissingEntity, params string[] errors)
            : base(succeeded, isMissingEntity, errors)
        {
            Model = model;
        }

        public new EntityResult<TModel> WithErrorCodeMessage(string code, string message)
        {
            Errors.Add(string.Format("{0}::{1}", code, message));
            return this;
        }

        public new EntityResult<TModel> WithMissingEntity(object id)
        {
            return this.WithErrorCodeMessage(Constants.ErrorCodes.EntityNotFound, $"Could not locate entity with ID {id}");
        }
    }

}
