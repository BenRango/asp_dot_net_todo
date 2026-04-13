using Microsoft.EntityFrameworkCore;
using TODO.Api.Features.Tasks;
using TODO.Api.Features.Users;

namespace TODO.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options): base (options){}
        public DbSet<TodoTask> TodoTask => Set<TodoTask>();
        public DbSet<User> User => Set<User>();
    }
}