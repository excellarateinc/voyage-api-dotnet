using Launchpad.Models;
using System;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;

namespace Launchpad.Web.Extensions
{
    public static class ModelErrorExtensions
    {
        public static BadRequestErrorModel ToModel(this ModelError error, string field)
        {
            var model = new BadRequestErrorModel { Field = field };
            var codedMessage = error.ErrorMessage.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
            if(codedMessage.Length == 2)
            {
                model.Code = codedMessage[0];
                model.Description = codedMessage[1];
            }
            else
            {
                model.Description = error.ErrorMessage;
            }
            return model;
        }

        public static IEnumerable<BadRequestErrorModel> ConvertToResponseModel(this ModelStateDictionary modelState)
        {
            var errorList = new List<BadRequestErrorModel>();

            foreach (var state in modelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    var converted = error.ToModel(state.Key);
                    errorList.Add(converted);
                }
            }
            return errorList;
        }
    }
}
