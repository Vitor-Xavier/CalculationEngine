using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.IoC.DependencyInjection
{
    public static class ConfigureBindingsDependencyInjection
    {
        public static void RegisterBindings(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureBindingsDatabaseContext.RegisterBindings(services, configuration);
            ConfigureBindingsApplicationService.RegisterBindings(services);
            ConfigureBindingsRepository.RegisterBindings(services);
        }
    }
}
