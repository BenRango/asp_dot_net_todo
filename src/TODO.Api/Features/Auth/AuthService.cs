using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TODO.Api.Data;
using TODO.Api.Features.Users;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace TODO.Api.Features.Auth
{
    public class AuthService : IAuthService
    {
        protected AppDbContext _db;
        protected IMapper _mapper;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public AuthService(AppDbContext db, IMapper mapper, string secretKey, string issuer, string audience)
        {
            _db = db;
            _mapper = mapper;
            _secretKey = secretKey;
            _issuer = issuer ;
            _audience = audience ;
        }
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), 
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<LoginResponse?> Login(LoginDto Credentials)
        {
            if (string.IsNullOrEmpty(Credentials.Username) || string.IsNullOrEmpty(Credentials.Password))
            {
                return null;
            }
            var user = await _db.User.FirstOrDefaultAsync(u => u.Username == Credentials.Username);
            
            if (user == null)
            {
                return null;
            }
            
            var UserResponse = _mapper.Map<UserResponseDto>(user);
            var authenticated = BCrypt.Net.BCrypt.Verify(Credentials.Password, user.Password);
            if (!authenticated)
            {
                return null;
            }

            var token = GenerateToken(user);
            

            return new LoginResponse(UserResponse, token);
        }
    }
}