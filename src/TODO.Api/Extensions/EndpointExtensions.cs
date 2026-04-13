using TODO.Api.Features.Auth;
using TODO.Api.Features.Tasks;
using TODO.Api.Features.Users;

namespace TODO.Api.Extensions
{
    public static class EndpointExtensions
    {
        public static IEndpointRouteBuilder MapMyEndpoints(this IEndpointRouteBuilder app)
        {
            //var api = app.MapGet("/", () => "Hello World");
            app.MapUserEndpoins();
            app.MapAuthEndpoint();
            app.MapTaskEndpoints();

            return app;
        }
    }
}