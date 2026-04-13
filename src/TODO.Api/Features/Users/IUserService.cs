namespace TODO.Api.Features.Users
{
    public interface IUserService
    {
        public Task<UserResponseDto> RegisterUser(UserRegisterInfoDto userDto);
        public Task<IEnumerable<UserResponseDto>> FetchEmAll();
        public Task<UserResponseDto> GetUserById(Guid Id);
    }
}