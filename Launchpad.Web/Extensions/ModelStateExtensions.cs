using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace Launchpad.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddErrors(this ModelStateDictionary state, IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                state.AddModelError("", error);
            }
        }
    }
}