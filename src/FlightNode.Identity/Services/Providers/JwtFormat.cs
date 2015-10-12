using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;

namespace FlightNode.Identity.Services.Providers
{
    public class JwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _issuer = string.Empty;

        public JwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            // TODO add try/catch to keep errors from going back to the client
            // right now the full details and stack trace are sent. Need to 
            // log the error and hide details. Might be best to add a handler
            // in the main application. The existing UnhandledExceptionFilter
            // is not catching this, so not sure yet how to intercept it.

            string audienceId = Properties.Settings.Default.AudienceId;

            string symmetricKeyAsBase64 = Properties.Settings.Default.AudienceSecret;
            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            var signingKey = new HmacSigningCredentials(keyByteArray);

            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;
            
            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}
