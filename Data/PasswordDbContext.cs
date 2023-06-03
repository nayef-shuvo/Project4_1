using Microsoft.EntityFrameworkCore;
using Project4_1.Models;

namespace Project4_1.Data
{
    public class PasswordDbContext : DbContext
    {
        public DbSet<PasswordHash> PasswordHashes { get; set; }
        public PasswordDbContext(DbContextOptions<PasswordDbContext> options) : base(options)
        {
            
        }
    }
}
