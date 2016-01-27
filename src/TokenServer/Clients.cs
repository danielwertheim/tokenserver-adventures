using System.Collections.Generic;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using Shared;

namespace TokenServer
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    Enabled = true,
                    AllowRememberConsent = false,
                    ClientId = ClientConstants.MvcClientId,
                    ClientName = "MVC Implicit",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(ClientConstants.MvcClientSecret.Sha256())
                    },
                    Flow = Flows.Implicit,
                    RedirectUris = new List<string>
                    {
                        EndPointConstants.MvcClient
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        EndPointConstants.MvcClient
                    },
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Email,
                        Constants.StandardScopes.Roles
                    }
                }
            };
        }
    }
}