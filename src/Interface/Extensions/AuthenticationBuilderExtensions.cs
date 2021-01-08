using Interface.Authentication;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Interface.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder authenticationBuilder, Action<CalculationAuthenticationOptions> options) =>
                authenticationBuilder.AddScheme<CalculationAuthenticationOptions, AuthenticationHandler>(CalculationAuthenticationOptions.DefaultScheme, options);
    }
}
