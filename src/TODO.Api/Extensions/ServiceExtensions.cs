namespace TODO.Api.Extensions
{
    public static class ServiceExtensions{
        public static IServiceCollection  AddApplicationServices (this IServiceCollection services, IConfiguration config){
            return services;
        }
        public static IServiceCollection  AddInfrastructureServices (this IServiceCollection services, IConfiguration config){
            return services;
        }
    }
}