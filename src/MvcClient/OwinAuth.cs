using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Shared;

namespace MvcClient
{
    public class UseCookieAuthAgainstTokenServerOptions
    {
        public string TokenServerEndpoint { get; }
        public string ClientId { get; }
        public string ClientSecret { get; set; }
        public ResponseType ResponseType { get; set; } = ResponseType.IdToken;
        public string[] Scopes { get; set; } = { "openid" };
        public string RedirectUriAfterLogin { get; set; }
        public string RedirectUriAfterLogout { get; set; }
        public string[] ClaimTypesOfInterest { get; }

        public UseCookieAuthAgainstTokenServerOptions(string tokenServerEndPoint, string clientId, string[] claimTypesOfInterest)
        {
            TokenServerEndpoint = tokenServerEndPoint.EnsureEndsWith("/");
            ClientId = clientId;
            ClaimTypesOfInterest = claimTypesOfInterest;
        }
    }

    internal static class StringExtensions
    {
        internal static string EnsureEndsWith(this string src, string expectedEnd)
        {
            return src.EndsWith(expectedEnd) ? src : $"{src}{expectedEnd}";
        }
    }

    [Flags]
    public enum ResponseType
    {
        IdToken = 0,
        AccessToken = 1
    }

    internal static class ResponseTypeExtensions
    {
        internal static string AsString(this ResponseType value)
        {
            return $"{(value.HasFlag(ResponseType.IdToken) ? "id_token" : null)} {(value.HasFlag(ResponseType.AccessToken) ? "token" : null)}".Trim();
        }
    }

    public static class OwinAuth
    {
        public static IAppBuilder UseCookieAuthAgainstTokenServer(this IAppBuilder app, UseCookieAuthAgainstTokenServerOptions options)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypeKeys.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = options.TokenServerEndpoint,
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                RedirectUri = options.RedirectUriAfterLogin,
                PostLogoutRedirectUri = options.RedirectUriAfterLogout,
                ResponseType = options.ResponseType.AsString(),
                Scope = string.Join(" ", options.Scopes),
                SignInAsAuthenticationType = "Cookies",

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = notification =>
                    {
                        var claimsIdentity = notification.AuthenticationTicket.Identity;
                        var transformedIdentity = new ClaimsIdentity(
                            claimsIdentity.AuthenticationType,
                            ClaimTypeKeys.GivenName,
                            ClaimTypeKeys.Role);

                        transformedIdentity.AddClaim(new Claim("id_token", notification.ProtocolMessage.IdToken));
                        if (notification.ProtocolMessage.AccessToken != null)
                            transformedIdentity.AddClaim(new Claim("token", notification.ProtocolMessage.AccessToken));

                        var claimsOfInterest = claimsIdentity.Claims.Where(c => options.ClaimTypesOfInterest.Contains(c.Type));
                        transformedIdentity.AddClaims(claimsOfInterest);

                        notification.AuthenticationTicket = new AuthenticationTicket(
                            transformedIdentity,
                            notification.AuthenticationTicket.Properties);

                        return Task.CompletedTask;
                    },
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.RequestType != OpenIdConnectRequestType.LogoutRequest)
                            return Task.CompletedTask;

                        var idToken = notification.OwinContext.Authentication.User.FindFirst("id_token");
                        if (idToken != null)
                            notification.ProtocolMessage.IdTokenHint = idToken.Value;

                        return Task.CompletedTask;
                    }
                }
            });

            return app;
        }
    }
}