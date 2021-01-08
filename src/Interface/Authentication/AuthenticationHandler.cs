using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Interface.Authentication
{
    public class AuthenticationHandler : AuthenticationHandler<CalculationAuthenticationOptions>
	{
		private const string ProblemDetailsContentType = "application/problem+json";

		private readonly IEnumerable<AuthenticationKey> _authenticationKeys;

		private const string ApiKeyHeaderName = "X-Api-Key";

		public AuthenticationHandler(IOptionsMonitor<CalculationAuthenticationOptions> options,
									 ILoggerFactory logger,
									 UrlEncoder encoder,
									 IOptions<Authentication> authentication,
									 ISystemClock clock) : base(options, logger, encoder, clock)
		{
			_authenticationKeys = authentication.Value.AuthenticationKeys;
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues) || !(apiKeyHeaderValues.FirstOrDefault() is string providedApiKey))
				return AuthenticateResult.NoResult();

			if (string.IsNullOrWhiteSpace(providedApiKey))
				return AuthenticateResult.NoResult();

			var auth = _authenticationKeys.FirstOrDefault(a => a.Key == providedApiKey);

			if (auth != null)
			{
				var claims = new List<Claim> { new Claim(ClaimTypes.Role, auth.Owner) };

				var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
				var identities = new List<ClaimsIdentity> { identity };
				var principal = new ClaimsPrincipal(identities);
				var ticket = new AuthenticationTicket(principal, Options.Scheme);

				return AuthenticateResult.Success(ticket);
			}

			return await Task.FromResult(AuthenticateResult.Fail("Chave de API fornecida é inválida."));
		}

		protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
		{
			Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			Response.ContentType = ProblemDetailsContentType;

			await Response.WriteAsync(JsonSerializer.Serialize("Você não está autorizado a utilizar essa API."));
		}
	}
}
