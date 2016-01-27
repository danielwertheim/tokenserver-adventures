using System.Collections.Generic;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;

namespace TokenServer
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            var email = StandardScopes.Email;
            //email.IncludeAllClaimsForUser = true;
            //email.Claims.ForEach(c => c.AlwaysIncludeInIdToken = true);

            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                email,
                new Scope
                {
                    Enabled = true,
                    Name = StandardScopes.Roles.Name,
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim(Constants.ClaimTypes.Role)
                    }
                }
            };
        }
    }
}