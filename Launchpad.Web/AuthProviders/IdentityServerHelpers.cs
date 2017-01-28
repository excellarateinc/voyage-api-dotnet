using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;

namespace Launchpad.Web.AuthProviders
{
    public static class IdentityServerHelpers
    {
        public static List<Scope> GetScopes()
        {
            var scopes = new List<Scope>
            {
                new Scope
                {
                    Enabled = true,
                    Name = "roles",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },
                new Scope
                {
                    Name = "api",
                    DisplayName = "Access to API",
                    Description = "This will grant you access to the API",
                    Type = ScopeType.Resource,
                    IncludeAllClaimsForUser = true
                }
            };

            scopes.AddRange(StandardScopes.All);

            return scopes;
        }

        public static List<Client> GetClients()
        {
            return new List<Client>
            {
                // no human involved
                new Client
                {
                    ClientName = "Silicon-only Client",
                    ClientId = "silicon",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Reference,
                    Flow = Flows.ClientCredentials,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("F621F470-9731-4A25-80EF-67A6F7C5F4B8".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "api"
                    }
                },

                // human is involved
                new Client
                {
                    ClientName = "Silicon on behalf of Carbon Client",
                    ClientId = "carbon",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowAccessToAllScopes = true,
                    Flow = Flows.ResourceOwner,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("F621F470-9731-4A25-80EF-67A6F7C5F4B8".Sha256())
                    },
                    RedirectUris = new List<string>
                    {
                        "http://localhost:52431"
                    },

                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:52431"
                    }
                },

                new Client
                {
                    Enabled = true,
                    ClientName = "JS Client",
                    ClientId = "js",
                    Flow = Flows.Implicit,

                    RedirectUris = new List<string>
                    {
                        "http://localhost:52431"
                    },

                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:52431"
                    },

                    AllowAccessToAllScopes = true
                }
            };
        }

        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "bob",
                    Password = "secret",
                    Subject = "1",
                    Claims = new[]
                    {
                        new Claim(ClaimTypes.GivenName, "Bob"),
                        new Claim(ClaimTypes.Role, "Geek"),
                        new Claim(ClaimTypes.Role, "Foo")
                    }
                }
            };
        }
    }
}