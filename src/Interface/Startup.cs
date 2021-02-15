using Crosscutting.IoC.DependencyInjection;
using Interface.Authentication;
using Interface.Extensions;
using Interface.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Interface
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureBindingsDependencyInjection.RegisterBindings(services, Configuration);

            services.Configure<Authentication.Authentication>(Configuration.GetSection("Authentication"));

            services.AddControllers();
            services.ConfigureCors();
            services.ConfigureSwagger();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CalculationAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = CalculationAuthenticationOptions.DefaultScheme;
            }).AddApiKey(options => { });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors("CorsPolicy");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint(env.IsDevelopment() ? "/swagger/v1/swagger.json" : "/calculationserver/swagger/v1/swagger.json", "Interface v1");
            });

            //app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
