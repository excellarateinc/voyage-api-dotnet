using Launchpad.Models;
using System;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;

namespace Launchpad.Web.Extensions
{
    public static class ModelErrorExtensions
    {
        public static RequestErrorModel ToModel(this ModelError error, string field)
        {
            var model = new RequestErrorModel { Field = field };
            var codedMessage = error.ErrorMessage.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
            if (codedMessage.Length == 2)
            {
                model.Error = codedMessage[0];
                model.ErrorDescription = codedMessage[1];
            }
            else
            {
                model.ErrorDescription = error.ErrorMessage;
            }

            return model;
        }

        public static IEnumerable<RequestErrorModel> ConvertToResponseModel(this ModelStateDictionary modelState)
        {
            var errorList = new List<RequestErrorModel>();

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

        public static List<RequestErrorModel> ToRequestErrorModel(this string message)
        {
            var model = new RequestErrorModel();
            var codedMessage = message.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
            if (codedMessage.Length == 2)
            {
                model.Error = codedMessage[0];
                model.ErrorDescription = codedMessage[1];
            }
            else
            {
                model.ErrorDescription = message;
            }

            return new List<RequestErrorModel> { model };
        }
    }
}
