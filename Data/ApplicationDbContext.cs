using Microsoft.EntityFrameworkCore;
using Project4_1.Models;

namespace Project4_1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Teacher> TeacherDatabse { get; set; }
        public DbSet<AuthModel> AuthDatabse { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

    }
}

