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
        public TeacherController(TeacherDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Teacher>>> GetAll()
        {
            var list = await _context.Teachers.ToListAsync();
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Teacher>> Add(RegisterDto request)
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

            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return Ok(teacher);
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
            if (temp == null)
            {
                return BadRequest();
            }
            _context.Teachers.Remove(temp);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
