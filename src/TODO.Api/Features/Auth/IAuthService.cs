using TODO.Api.Features.Users;

namespace TODO.Api.Features.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse?> Login(LoginDto Crendentials);
    }
}