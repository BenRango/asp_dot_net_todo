using TODO.Api.Features.Users;

namespace TODO.Api.Features.Auth
{
    public record LoginDto(string Username, string Password);
    public record LoginResponse(UserResponseDto User, string AccessToken);
}