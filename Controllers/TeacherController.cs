using Microsoft.AspNetCore.Mvc;
using Project4_1.Models;
using Project4_1.Models.Dto;

namespace Project4_1.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private static readonly List<Teacher> _teachers = new List<Teacher>();

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Teacher>> GetAll()
        {
            return Ok(_teachers);
        }

        [HttpGet("email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Teacher> Get(string emil)
        {
            var temp = _teachers.FirstOrDefault(x => x.Email == emil);
            if (temp == null) 
            {
                return NotFound();
            }
            return Ok(temp);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Teacher> Add(SignUpDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // Checking mail
            var temp = _teachers.FirstOrDefault(x => x.Email == request.Email);
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

            _teachers.Add(teacher);
            return Ok(teacher);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Teacher> Update(Teacher request)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }
            var temp = _teachers.FirstOrDefault(y => y.Email == request.Email);
            if (temp == null)
            {
                return BadRequest();
            }
            temp.Name = request.Name;
            temp.Email = request.Email;
            temp.Phone = request.Phone;
            temp.Rank = request.Rank;

            return Ok(temp);
        }
    }
}
