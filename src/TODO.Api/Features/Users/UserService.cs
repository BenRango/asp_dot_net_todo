
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TODO.Api.Data;
using AutoMapper.QueryableExtensions;
using Amazon.S3;
using Amazon.S3.Model;
namespace TODO.Api.Features.Users
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IAmazonS3 _s3Client;

        public UserService ( AppDbContext db, IMapper mapper, IAmazonS3 s3Client)
        {
            _db = db;
            _mapper = mapper;
            _s3Client = s3Client;
        }

        public async Task<UserResponseDto> RegisterUser(UserRegisterInfoDto createUserDto)
        {
            User? existingUser = await _db.User.FirstOrDefaultAsync(u => u.Username == createUserDto.Username);
            if (existingUser != null)
            {
                throw new HttpRequestException("Conflict");
            }
            if (createUserDto.Password != createUserDto.ConfirmPassword)
            {
                throw new BadHttpRequestException("Mots de passe incomptatibles");
            }
            var user = _mapper.Map<User>(createUserDto);
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password, salt);
            user.Password = hashedPassword;
            var file = createUserDto.File!;
            string extension = Path.GetExtension(file.FileName);
            string fileName = $"profile_pictures/{Guid.NewGuid()}{extension}";
            using (var stream = file.OpenReadStream())
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = "todo",
                    Key = fileName,
                    InputStream = stream,
                    ContentType = file.ContentType
                };
                try
                {
                    await _s3Client.PutObjectAsync(putRequest);
                }
                catch (System.Exception e)
                {
                    
                    Console.WriteLine($"Erreur S3: {e.Message}"); 
                    throw;
                }
                
            }
            string file_url = "https://s3.actuscientifique.com/todo/"+fileName;
            user.ProfilePicUrl = file_url;
            Console.WriteLine(fileName);
            _db.User.Add(user);
            await _db.SaveChangesAsync();
            var newUser = _mapper.Map<UserResponseDto>(user);
            return newUser;
        }

        public async Task<UserResponseDto> GetUserById(Guid Id)
        {
            var user = await _db.User.FindAsync(Id);
            var foundUser = _mapper.Map<UserResponseDto>(user);
            return foundUser;
        }
        public async Task<IEnumerable<UserResponseDto>> FetchEmAll()
        {
            return await _db.User
            .ProjectTo<UserResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }
        public  AuthResponseDto? Login(LoginDto credentials)
        {
            return new AuthResponseDto("vvd");
        }
    }
}