using System.Collections.Generic;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using Shared;

namespace TokenServer
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,
                StandardScopes.Roles,
                new Scope
                {
                    Enabled = true,
                    Name = ResourceScopes.SecuredApi,
                    DisplayName = "Secured API",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim(Constants.ClaimTypes.Role)
                    }
                }
            };
        }
    }
}