using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Reflection;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using Owin;
using Shared;
using Swashbuckle.Application;

namespace SecuredApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = EndPointConstants.TokenServer,
                RequiredScopes = new[] { ResourceScopes.SecuredApi },
            });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);

            config.EnableSwagger("docs/{apiVersion}/swagger", c =>
            {
                c.SingleApiVersion("v1", "Super duper API");

                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                var commentsFile = Path.Combine(baseDirectory, "bin", commentsFileName);
                c.IncludeXmlComments(commentsFile);
            });
        }
    }
}