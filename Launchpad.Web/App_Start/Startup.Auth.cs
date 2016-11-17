using Autofac;
using Launchpad.Web.App_Start;
using Launchpad.Web.AuthProviders;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;

namespace Launchpad.Web
{
    using Middleware;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId => "self"; // Configure the application for OAuth based flow
            
        
        public static void Configure(IAppBuilder app)
        {
            //Add autofac to the pipeline
            var httpConfig = new HttpConfiguration();

            ContainerConfig.Register(httpConfig);

            WebApiConfig.Register(httpConfig);



            app.UseAutofacMiddleware(ContainerConfig.Container);
  
            app.UseCors(CorsOptions.AllowAll);
            

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.Use(typeof(ValidationHandler));

            var oauthProvider = ContainerConfig.Container.Resolve<ApplicationOAuthProvider>();
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/v1/login"),
                Provider = oauthProvider,
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),

                //If the config is wrong, let the application crash
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["oAuth:TokenExpireSeconds"])),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = bool.Parse(ConfigurationManager.AppSettings["oAuth:AllowInsecureHttp"]),
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            app.UseWebApi(httpConfig);
        }
    }

    public static class IOwinResponseExtensions
    {
        public static async Task SetBody(this IOwinResponse response, string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);

            var buffer = new MemoryStream(bytes);
            buffer.Seek(0, SeekOrigin.Begin);

            await buffer.CopyToAsync(response.Body);
        }

        public static async Task GetResponseBody(this IOwinContext context, AppFunc next)
        {
            using (var buffer = new MemoryStream())
            {
                //replace the context response with our buffer
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                //invoke the rest of the pipeline
                await next.Invoke(context.Environment);

                //reset the buffer and read out the contents
                buffer.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(buffer);
                using (var bufferReader = new StreamReader(buffer))
                {
                    string body = await bufferReader.ReadToEndAsync();

                    //reset to start of stream
                    buffer.Seek(0, SeekOrigin.Begin);

                    //copy our content to the original stream and put it back
                    await buffer.CopyToAsync(stream);
                    context.Response.Body = stream;
                }
            }
        }
    }
}