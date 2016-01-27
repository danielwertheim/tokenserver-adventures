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
                RedirectUriAfterLogout = EndPointConstants.MvcClient
            };
            app.UseCookieAuthAgainstTokenServer(options);
        }
    }
}