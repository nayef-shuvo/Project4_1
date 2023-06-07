using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project4_1.Models.Dto;
using Project4_1.Models;
using Project4_1.Data;
using System.Security.Cryptography;
using System.Text;

namespace Project4_1.Controllers
{
    public class AuthController : Controller
    {
        private readonly TeacherDbContext _context;
        private readonly AuthDbContext _passwordContext;

        public AuthController(TeacherDbContext context, AuthDbContext passwordContext)
        {
            _context = context;
            _passwordContext = passwordContext;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Teacher>> Register(RegisterDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // Checking mail
            var temp = await _context.Teachers.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (temp != null)
            {
                return BadRequest("Email already exist");
            }

            Teacher teacher = new()
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Rank = request.Rank,
            };

            //Generating Password Hash and Salt
            GenerateHashAndSalt(request.Password, out byte[] hash, out byte[] salt);
            PasswordHash password = new()
            {
                Email = request.Email,
                Hash = hash,
                Salt = salt,
            };


            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();

            await _passwordContext.PasswordHashes.AddAsync(password);
            await _passwordContext.SaveChangesAsync();

            return Ok(password);
        }

        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private void GenerateHashAndSalt(string password, out byte[] hash, out byte[] salt)
        {
            byte[] saltBytes = new byte[64]; // 64 bytes = 512 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var hmac = new HMACSHA512(saltBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(passwordBytes);
                salt = saltBytes;
                hash = hashBytes;
            }
        }
        private bool Verify(string password, byte[] hash, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }
    }
}
