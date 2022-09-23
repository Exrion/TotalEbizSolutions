using AdventureWorksClassLib;
using AdventureWorksClassLib.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace AdventureWorksAPI.Handlers
{
    public class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AuthClass authClass;
        public AuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AuthClass authClass)
            : base(options, logger, encoder, clock)
        {
            this.authClass = authClass;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //Check Auth header
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header not found");

            try
            {
                StringValues reqHeadAuth = Request.Headers["Authorization"];
                UserDTO user = authClass.verifyAuth(reqHeadAuth);

                if (user != null)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.Email) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("An error has occured");
            }

            return AuthenticateResult.Fail("An error has occured");
        }
    }
}
