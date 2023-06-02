using Microsoft.EntityFrameworkCore;
using Project4_1.Models;

namespace Project4_1.Data
{
    public class TeacherDbContext : DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public TeacherDbContext(DbContextOptions<TeacherDbContext> options) : base(options)
        {
             
        }
    }
}
