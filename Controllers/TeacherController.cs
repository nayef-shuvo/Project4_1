using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project4_1.Data;
using Project4_1.Models;
using Project4_1.Models.Dto;

namespace Project4_1.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherDbContext _context;
        private readonly AuthDbContext _passwordContext;
        public TeacherController(TeacherDbContext context, AuthDbContext passwordContext)
        {
            _context = context;
            _passwordContext = passwordContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Teacher>>> GetAll()
        {
            var list = await _context.Teachers.ToListAsync();
            return Ok(list);
        }
        [Route("api/hash")]
        [HttpGet]
        public async Task<IActionResult> GetHash()
        {
            var list = await _passwordContext.PasswordHashes.ToListAsync();
            return Ok(list);
        }

        [HttpGet("email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Teacher>> Get(string emil)
        {
            var temp = await _context.Teachers.FirstOrDefaultAsync(x => x.Email == emil);
            if (temp == null) 
            {
                return NotFound();
            }
            return Ok(temp);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Teacher>> Update(Teacher request)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }
            var temp = await _context.Teachers.FirstOrDefaultAsync(y => y.Email == request.Email);
            if (temp == null)
            {
                return BadRequest();
            }
            temp.Name = request.Name;
            temp.Email = request.Email;
            temp.Phone = request.Phone;
            temp.Rank = request.Rank;

            await _context.SaveChangesAsync();
            return Ok(temp);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string email)
        {
            var temp = await _context.Teachers.FirstOrDefaultAsync(y => y.Email == email);
            var password = await _passwordContext.PasswordHashes.FirstOrDefaultAsync(password => password.Email == email);

            if (temp == null || password == null)
            {
                return BadRequest();
            }
            _context.Teachers.Remove(temp);
            await _context.SaveChangesAsync();

            _passwordContext.PasswordHashes.Remove(password);
            await _passwordContext.SaveChangesAsync();


            return NoContent();
        }

        [Route("/login")]
        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] LoginDto loginDto)
        {
            var password = await _passwordContext.PasswordHashes.AsNoTracking().FirstOrDefaultAsync(
                x => x.Email == loginDto.Email);

            if (password == null)
            {
                return BadRequest("Wrong email");
            }
            //var isMatched = Verify(loginDto.Password, password.Hash, password.Salt);
            //if (!isMatched)
            //{
            //    return BadRequest("Wrong password");
            //}
            var teacher = await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(
                x => x.Email == password.Email);
            return Ok(  );

        }

        [Route("/pass")]
        [HttpGet]
        public async Task<IActionResult> GetPass(string email)
        {
            var password = await _passwordContext.PasswordHashes.FirstOrDefaultAsync(x => x.Email == email);
            if (password == null)
            {
                return BadRequest();
            }
            return Ok(password);
        }

        

    }
}
