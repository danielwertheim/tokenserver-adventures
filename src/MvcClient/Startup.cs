using System.Security.Claims;
using System.Threading.Tasks;
using Owin;
using Shared;

namespace MvcClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new UseCookieAuthAgainstTokenServerOptions(EndPointConstants.TokerServer, ClientConstants.MvcClientId, new[]
            {
                ClaimTypeKeys.Subject,
                ClaimTypeKeys.GivenName,
                ClaimTypeKeys.FamilyName,
                ClaimTypeKeys.Email,
                ClaimTypeKeys.Role
            })
            {
                Scopes = new[] { "openid", "profile", "email", "roles" },
                RedirectUriAfterLogin = EndPointConstants.MvcClient,
                RedirectUriAfterLogout = EndPointConstants.MvcClient,

                //Can use the orgIdentity to look up information about user in e.g. IdentityServer
                OnTransformingValidatedIdentity = orgIdentity => Task.FromResult(new[] { new Claim("hobby", "baking") })
            };
            app.UseCookieAuthAgainstTokenServer(options);
        }
    }
}