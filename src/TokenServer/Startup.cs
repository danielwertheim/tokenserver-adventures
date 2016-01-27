using System;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using Owin;

namespace TokenServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/identity", idsrvApp =>
            {
                var options = new IdentityServerOptions
                {
                    SiteName = "The identity server",
                    SigningCertificate = LoadCertificate(),
                    EnableWelcomePage = true,
                    Factory = new IdentityServerServiceFactory()
                        .UseInMemoryUsers(Users.Get())
                        .UseInMemoryClients(Clients.Get())
                        .UseInMemoryScopes(Scopes.Get()),
                    AuthenticationOptions =
                    {
                        EnablePostSignOutAutoRedirect = true,
                        CookieOptions = {AllowRememberMe = false},
                        RememberLastUsername = false
                    }
                };

                idsrvApp.UseIdentityServer(options);
            });
        }

        private static X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2($@"{AppDomain.CurrentDomain.BaseDirectory}\bin\idsrv3test.pfx", "idsrv3test");
        }
    }
}