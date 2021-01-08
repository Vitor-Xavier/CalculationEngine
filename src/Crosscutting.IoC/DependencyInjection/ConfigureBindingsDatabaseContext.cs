using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.IoC.DependencyInjection
{
    public static class ConfigureBindingsDatabaseContext
    {
        public static void RegisterBindings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer().AddDbContextPool<CalculationContext>(
                (sp, options) => options.UseSqlServer(configuration.GetConnectionString("calculoserver"),
                    providerOptions => providerOptions.EnableRetryOnFailure()).UseInternalServiceProvider(sp).EnableSensitiveDataLogging());
        }
    }
}
