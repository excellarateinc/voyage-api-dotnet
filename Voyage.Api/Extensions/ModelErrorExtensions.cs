using Voyage.Models;
using System;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;

namespace Voyage.Api.Extensions
{
    public static class ModelErrorExtensions
    {
        public static ResponseErrorModel ToModel(this ModelError error, string field)
        {
            var model = new ResponseErrorModel { Field = field };
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

        public static IEnumerable<ResponseErrorModel> ConvertToResponseModel(this ModelStateDictionary modelState)
        {
            var errorList = new List<ResponseErrorModel>();

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

        public static List<ResponseErrorModel> ToRequestErrorModel(this string message)
        {
            var model = new ResponseErrorModel();
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

            return new List<ResponseErrorModel> { model };
        }
    }
}
