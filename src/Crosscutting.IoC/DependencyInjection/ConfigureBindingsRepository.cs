using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.IoC.DependencyInjection
{
    static class ConfigureBindingsRepository
    {
        public static void RegisterBindings(IServiceCollection services)
        {
            services.AddScoped<ContribuinteRepository>();
            services.AddScoped<FisicoRepository>();
            services.AddScoped<FaceQuadraRepository>();
            services.AddScoped<DynamicSearchRepository>();
        }
    }
}
