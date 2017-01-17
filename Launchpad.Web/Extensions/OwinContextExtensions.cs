using Launchpad.Models;
using Microsoft.Owin;
using System;

namespace Launchpad.Web.Extensions
{
    public static class OwinContextExtensions
    {
        private const string NoIdentity = "No Identity";
        private const string OwinRequestId = "owin.RequestId";
        private static readonly string EmptyId = Guid.Empty.ToString();

        public static string GetIdentityName(this IOwinContext context)
        {
            string identityName = NoIdentity;

            if (!string.IsNullOrEmpty(context.Authentication?.User?.Identity?.Name))
                identityName = context.Authentication.User.Identity.Name;

            return identityName;
        }

        public static ActivityAuditModel ToAuditModel(this IOwinContext context, string requestIdOverride)
        {
            var model = new ActivityAuditModel();

            var owinRequestId = context.Environment[OwinRequestId].ToString();
            model.RequestId = EmptyId.Equals(owinRequestId, StringComparison.InvariantCultureIgnoreCase) ? requestIdOverride : owinRequestId;
            model.IpAddress = context.Request.RemoteIpAddress;
            model.Method = context.Request.Method;
            model.Date = DateTime.Now;
            model.Path = context.Request.Path.Value;
            model.UserName = context.GetIdentityName();
            model.StatusCode = context.Response.StatusCode;
            return model;
        }
    }
}