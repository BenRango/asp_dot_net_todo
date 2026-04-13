using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Routing;

namespace TODO.Api.Features.Users
{
    public record UserResponseDto(Guid Id, string Username, string? ProfilePicUrl );
    public record UserRegisterInfoDto(
    [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire")] string Username,
    [Required] string? Password,
    [Required] string ConfirmPassword, IFormFile? File);
    public record LoginDto(string Username, string Password);
    public record AuthResponseDto( string Acces_token);
}