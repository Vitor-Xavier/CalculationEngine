using Microsoft.AspNetCore.Authentication;

namespace Interface.Authentication
{
    public class CalculationAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";

        public string Scheme => DefaultScheme;

        public string AuthenticationType = DefaultScheme;
    }
}
