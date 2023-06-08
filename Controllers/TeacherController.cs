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
        private readonly ApplicationDbContext _dbContext;

        public TeacherController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var list = await _dbContext.TeacherDatabse.ToListAsync();
            return Ok(list);
        }

        [HttpGet("email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var temp = await _dbContext.TeacherDatabse.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

            if (temp == null) 
            {
                return NotFound();
            }
            return Ok(temp);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Teacher request)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }
            var temp = await _dbContext.TeacherDatabse.FirstOrDefaultAsync(y => y.Id == request.Id);
            if (temp == null)
            {
                return BadRequest();
            }
            temp.Name = request.Name;
            temp.Email = request.Email;
            temp.Phone = request.Phone;
            temp.Rank = request.Rank;

            await _dbContext.SaveChangesAsync();

            return Ok(temp);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var teacher = await _dbContext.TeacherDatabse.FirstOrDefaultAsync(x => x.Id == id);
            var auth = await _dbContext.AuthDatabse.FirstOrDefaultAsync(x => x.Id == id);

            if (teacher == null || auth == null) 
            {
                return BadRequest();
            }

            _dbContext.TeacherDatabse.Remove(teacher);
            _dbContext.AuthDatabse.Remove(auth);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
