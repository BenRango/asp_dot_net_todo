using Microsoft.AspNetCore.Mvc;

namespace TODO.Api.Features.Users
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoins( this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/users").WithTags("Users");

            group.MapPost("/", async ([FromForm] string username, 
                [FromForm] string ?password, 
                [FromForm] string confirmPassword, 
                IFormFile? file, 
                IUserService userServices) =>
                {
                var registerUserDto = new UserRegisterInfoDto(
                    username,
                    password,
                    confirmPassword,
                    file
                );

                try
                {
                    var result = await userServices.RegisterUser(registerUserDto);
                    return Results.Ok(result);
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message, statusCode: 500);
                }
            })
            .DisableAntiforgery();

            group.MapGet("/", async (IUserService userService) => Results.Ok(await userService.FetchEmAll()));            
        }
    }
}