using System.ComponentModel.DataAnnotations.Schema;
namespace TODO.Api.Features.Users
{
    [Table("users")]
    public class User
    {
        public Guid Id {get; set;} = Guid.NewGuid();
        public string Username {get; set;} = string.Empty;
        public string Password {get; set;} = string.Empty;
        public string ProfilePicUrl {get; set;} = string.Empty;
        public User() {}
        public User(UserRegisterInfoDto createUserDto)
        {
            Username = createUserDto.Username;
            Password = createUserDto.ConfirmPassword;
        }
    }
}