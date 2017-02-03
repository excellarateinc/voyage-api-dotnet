using FluentValidation;

namespace Voyage.Models.Validators
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithErrorCodeMessage<T, TProperty>(this IRuleBuilderOptions<T, TProperty> options, string code, string message)
        {
            options.WithMessage("{0}::{1}", code, message);
            return options;
        }
    }
}
