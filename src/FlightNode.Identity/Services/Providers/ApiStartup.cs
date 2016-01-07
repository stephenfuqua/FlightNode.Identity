using FlightNode.Identity.Domain.Entities;
using FlightNode.Identity.Infrastructure.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Owin;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FlightNode.Identity.Services.Providers
{
    public static class ApiStartup
    {
        public static IAppBuilder Configure(IAppBuilder app, string issuer)
        {
            app = ConfigureIdentityManagement(app, issuer);
            app = ConfigureOAuthTokenConsumption(app, issuer);

            return app;
        }

        private static IAppBuilder ConfigureIdentityManagement(IAppBuilder app, string issuer)
        {
            app.CreatePerOwinContext(IdentityDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);

            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttpConnection = false)
                AllowInsecureHttp = Properties.Settings.Default.AllowInsecureHttpConnection,

                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new OAuthProvider(),
                ApplicationCanDisplayErrors = false,
                
                AccessTokenFormat = new JwtFormat(issuer)
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);


            //var OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            //OAuthBearerOptions.AccessTokenFormat = OAuthServerOptions.AccessTokenFormat;
            //OAuthBearerOptions.AccessTokenProvider = OAuthServerOptions.AccessTokenProvider;
            //OAuthBearerOptions.AuthenticationMode = OAuthServerOptions.AuthenticationMode;
            //OAuthBearerOptions.AuthenticationType = OAuthServerOptions.AuthenticationType;
            //OAuthBearerOptions.Description = OAuthServerOptions.Description;

            //OAuthBearerOptions.Provider = new CustomBearerAuthenticationProvider();
            //OAuthBearerOptions.SystemClock = OAuthServerOptions.SystemClock;

            //app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            //app.UseOAuthBearerTokens(OAuthServerOptions);

            return app;
        }

        private static IAppBuilder ConfigureOAuthTokenConsumption(IAppBuilder app, string issuer)
        {
            var audienceId = Properties.Settings.Default.AudienceId;
            var audienceSecret = TextEncodings.Base64Url.Decode(Properties.Settings.Default.AudienceSecret);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });

            return app;
        }


        public static IUnityContainer ConfigureDependencyInjection(IUnityContainer container)
        {
            container = RegisterAllTypesIn(container, Assembly.GetExecutingAssembly());

            container.RegisterType<IdentityDbContext>();
            container.RegisterType(typeof(IUserStore<User, int>), typeof(AppUserStore));

            return container;
        }

        private static IUnityContainer RegisterAllTypesIn(IUnityContainer container, Assembly repoAssembly)
        {
            container.RegisterTypes(AllClasses.FromAssemblies(repoAssembly),
                                                 WithMappings.FromAllInterfacesInSameAssembly,
                                                 WithName.Default,
                                                 WithLifetime.PerResolve);

            return container;
        }

    }

    //public class CustomBearerAuthenticationProvider : OAuthBearerAuthenticationProvider
    //{
    //    // This validates the identity based on the issuer of the claim.
    //    // The issuer is set in the API endpoint that logs the user in
    //    public override Task ValidateIdentity(OAuthValidateIdentityContext context)
    //    {
    //        var claims = context.Ticket.Identity.Claims;
    //        if (claims.Count() == 0 || claims.Any(claim => claim.Issuer != "Facebook" && claim.Issuer != "LOCAL_AUTHORITY"))
    //            context.Rejected();
    //        return Task.FromResult<object>(null);
    //    }
    //}
}
