using Login_Test.Models;
using Microsoft.EntityFrameworkCore;
using Token_Test.Models;

namespace Token_Test.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users {get; set;}
        public DbSet<Product> Products {get; set;}
    }
}
