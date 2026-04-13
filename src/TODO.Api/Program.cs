using Microsoft.EntityFrameworkCore;
using dotenv.net;
using Amazon.S3;
using System.Text;

using TODO.Api.Data;
using TODO.Api.Maps;
using TODO.Api.Extensions;
using TODO.Api.Features.Tasks;
using TODO.Api.Features.Users;
using TODO.Api.Features.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

DotEnv.Load(options: new DotEnvOptions(envFilePaths: ["../../.env"]));
var builder = WebApplication.CreateBuilder(args);

var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "defaultSecretKey";
var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "defaultIssuer";
var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "defaultAudience";

if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("La variable d'environnement JWT_SECRET_KEY est manquante !");
}

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var awsOptions = builder.Configuration.GetAWSOptions("AWS");
var s3Config = new AmazonS3Config
{
    ServiceURL = builder.Configuration["AWS:Endpoint"], 
    ForcePathStyle = true
};

builder.Services.AddSingleton<IAmazonS3>(sp => 
{
    var credentials = new Amazon.Runtime.BasicAWSCredentials(
        Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
        Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")
    );
    return new AmazonS3Client(credentials, s3Config);
});

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>(sp => 
{
    var db = sp.GetRequiredService<AppDbContext>();
    var mapper = sp.GetRequiredService<IMapper>();
    return new AuthService(db, mapper, secretKey, issuer, audience);
});;
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
    .UseSnakeCaseNamingConvention()
);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapMyEndpoints();

app.Run();
