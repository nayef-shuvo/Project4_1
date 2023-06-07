using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project4_1.Models;

namespace Project4_1.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public DbSet<PasswordHash> PasswordHashes { get; set; }
    }
}
