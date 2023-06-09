using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project4_1.Data;
using Project4_1.Models;
using Project4_1.Models.Dto;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Project4_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _dbContext;
        public AuthController(IConfiguration config, ApplicationDbContext dbContext)
        {

            _config = config;
            _dbContext = dbContext;

        }

        // POST api/Auth/register

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var temp = await _dbContext.TeacherDatabse.AsNoTracking().FirstOrDefaultAsync(x => x.Email == request.Email);
            if (temp != null)
            {
                return BadRequest("This email is already used");
            }

            var (hash, salt) = GenerateHashAndSalt(request.Password);

            var teacher = new Teacher
            {
                Name = request.Name,
                Email = request.Email,
                Rank = request.Rank,
                Phone = request.Phone,
            };

            var auth = new AuthModel
            {
                Id = teacher.Id,
                PasswordHash = hash,
                PasswordSalt = salt,
            };

            await _dbContext.TeacherDatabse.AddAsync(teacher);
            await _dbContext.AuthDatabse.AddAsync(auth);

            await _dbContext.SaveChangesAsync();

            return Ok(teacher);
        }

        // api/Auth/login?Email=email&Password=password

        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            bool isMatched = await Verify(loginDto);

            if (!isMatched)
            {
                return BadRequest("Email or password is invalid");
            }
            var teacher = await _dbContext.TeacherDatabse.AsNoTracking().FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            var token = GenerateToken(teacher!);
            return Ok(token);
        }

        private (byte[], byte[]) GenerateHashAndSalt(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = hmac.Key;
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                return (hash,  salt);
            }
        }

        private async Task<bool> Verify(LoginDto loginDto)
        {
            var teacher = await _dbContext.TeacherDatabse.AsNoTracking().FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (teacher == null)
            {
                return false;
            }

            var hashAndSalt = await _dbContext.AuthDatabse.AsNoTracking().FirstOrDefaultAsync(x => x.Id == teacher.Id);
            if (hashAndSalt == null)
            {
                return false;
            }

            using (var hmac = new HMACSHA512(hashAndSalt.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

                if (computedHash.SequenceEqual(hashAndSalt.PasswordHash))
                {
                    return true;
                }
            }

            return false;
        }




        private string GenerateToken(Teacher teacher)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, teacher.Id),
                new Claim(ClaimTypes.Name, teacher.Name),
                new Claim(ClaimTypes.Email, teacher.Email),
                new Claim(ClaimTypes.Role, teacher.Role),
                new Claim(ClaimTypes.Role, teacher.Rank.ToString()),
                new Claim(ClaimTypes.MobilePhone, teacher.Phone),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //private Teacher Authenticate(LoginDto loginDto)
        //{

        //}
    }
}
