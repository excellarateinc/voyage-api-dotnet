using Launchpad.Models;
using Microsoft.Owin;
using System;

namespace Launchpad.Web.Extensions
{
    public static class OwinContextExtensions
    {
        private const string _noIdentity = "No Identity";
        private const string _owinRequestId = "owin.RequestId";

        private static string _getIdentityName(IOwinContext context)
        {
            string identityName = _noIdentity;

            if (context.Authentication != null &&
                context.Authentication.User != null &&
                context.Authentication.User.Identity != null &&
                !string.IsNullOrEmpty(context.Authentication.User.Identity.Name))
                identityName = context.Authentication.User.Identity.Name;

            return identityName;
        }

        public static ActivityAuditModel ToAuditModel(this IOwinContext context)
        {
            ActivityAuditModel model = new ActivityAuditModel();

            model.RequestId = context.Environment[_owinRequestId].ToString();
            model.IpAddress = context.Request.RemoteIpAddress;
            model.Method = context.Request.Method;
            model.Date = DateTime.Now;
            model.Path = context.Request.Path.Value;
            model.UserName = _getIdentityName(context);
            model.StatusCode = context.Response.StatusCode;
            return model;
        }
    }
}