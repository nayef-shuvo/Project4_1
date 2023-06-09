using Microsoft.EntityFrameworkCore;
using Project4_1.Models;
using Project4_1.Models.Items;
using System.Text.Json;


namespace Project4_1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Teacher> TeacherDatabse { get; set; }
        public DbSet<AuthModel> AuthDatabse { get; set; }
        public DbSet<Item> ItemDatabse { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>()
                .HasKey(x => x.Id); 

            modelBuilder.Entity<AuthModel>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Item>()
               .Property(i => i.SubItem)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, new JsonSerializerOptions { }),
                   v => JsonSerializer.Deserialize<Dictionary<string, int>>(v, new JsonSerializerOptions { })!
               );
            modelBuilder.Entity<Item>()
                .HasKey(i => i.Title);

        }

    }
}

