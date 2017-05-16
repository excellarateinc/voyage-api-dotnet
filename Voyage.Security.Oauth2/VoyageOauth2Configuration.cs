using System;
using Autofac;

namespace Voyage.Security.Oauth2
{
    public class VoyageOauth2Configuration
    {
        public IContainer Container { get; set; }
        
        /// <summary>
        /// Default value you will be set if not provided. Default value: /OAuth/Authorize
        /// </summary>
        public string AuthorizeEndpointPath { get; set; }

        /// <summary>
        /// Default value you will be set if not provided. Default value: /OAuth/Token
        /// </summary>
        public string TokenEndpointPath { get; set; }

        public TimeSpan AccessTokenExpireTimeSpan { get; set; }
        
        public bool AllowInsecureHttp { get; set; }

        public string TokenExpireSeconds { get; set; }
    }
}