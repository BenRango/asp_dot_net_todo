using TODO.Api.Features.Users;

namespace TODO.Api.Features.Auth
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/auth").WithTags("Auth");
            group.MapPost("/login", async (LoginDto credentials, IAuthService authService) =>
            {
                var result   =  await authService.Login(credentials);

                return result == null ? Results.Unauthorized() : Results.Ok(result);
            });
            
        }
    }
}