using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Launchpad.Models.UnitTests
{
    public static class ValidationTestExtensions
    {
        /// <summary>
        /// Run the validations for a given model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns>Tuple where the first item is the result of TryValidateObject and the second item is the validation results</returns>
        /// <remarks>A tuple... Remember, it is test code</remarks>
        public static Tuple<bool, List<ValidationResult>> RunValidations<TModel>(this TModel model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model);
            var result = Validator.TryValidateObject(model, context, validationResults, true);
            return new Tuple<bool, List<ValidationResult>>(result, validationResults);
        }
    }
}
