using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel.Client;
using Owin;
using Shared;

namespace MvcClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new UseCookieAuthAgainstTokenServerOptions(EndPointConstants.TokenServer, ClientConstants.MvcClientId, new[]
            {
                ClaimTypeKeys.Subject,
                ClaimTypeKeys.GivenName,
                ClaimTypeKeys.FamilyName,
                ClaimTypeKeys.Email,
                ClaimTypeKeys.Role
            })
            {
                ResponseType = ResponseType.IdToken | ResponseType.AccessToken,
                Scopes = new[] { "openid", "profile", "roles", ResourceScopes.SecuredApi },
                RedirectUriAfterLogin = EndPointConstants.MvcClient,
                RedirectUriAfterLogout = EndPointConstants.MvcClient,
                //OnTransformingValidatedIdentity = async orgIdentity =>
                //{

                //},
                OnLookupUserInfo = async accesstoken =>
                {
                    var userInfoClient = new UserInfoClient(new Uri(EndPointConstants.TokenServer + "/connect/userinfo"), accesstoken);
                    var userInfo = await userInfoClient.GetAsync();

                    var claims = new List<Claim>();
                    userInfo.Claims.ToList().ForEach(ui => claims.Add(new Claim(ui.Item1, ui.Item2)));

                    return claims.ToArray();
                }
            };
            app.UseCookieAuthAgainstTokenServer(options);
        }
    }
}