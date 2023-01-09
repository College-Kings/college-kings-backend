using CollegeKingsBackend.Application.V1.Lovense;
using CollegeKingsBackend.Application.V1.Lovense.Queries;
using CollegeKingsBackend.Application.V1.Lovense.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ILovenseService, LovenseService>();
            services.AddTransient<LovenseQueries>();
            return services;
        }
    }
}