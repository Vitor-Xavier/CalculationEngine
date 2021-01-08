using ApplicationService.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.IoC.DependencyInjection
{
    static class ConfigureBindingsApplicationService
    {
        public static void RegisterBindings(IServiceCollection services)
        {
            services.AddScoped<DynamicSearchService>();
            services.AddScoped<ExecutionService>();
        }
    }
}
