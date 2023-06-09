using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project4_1.Data;
using Project4_1.Models.Items;

namespace Project4_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ItemController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _dbContext.ItemDatabse.ToListAsync();
            return Ok(items);
        }

        // GET api/Item/title
        [HttpGet("{title}")]
        public async Task<ActionResult<Item>> GetByTitle(string title)
        {
            var item = await _dbContext.ItemDatabse.FindAsync(title);
            if (item == null)
            {
                return NotFound();

            }

            return item;
        }

        // POST api/Item
        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var temp = await _dbContext.ItemDatabse.FirstOrDefaultAsync(x => x.Title == item.Title);
            if (temp != null)
            {
                return BadRequest("Item with same title already exist") ;
            }

            await _dbContext.ItemDatabse.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByTitle), new { title = item.Title }, item);
        }


        // PUT /api/Item/title
        [HttpPut("{title}")]
        public async Task<IActionResult> UpdateItem(string title, Item updatedItem)
        {
            if (title != updatedItem.Title)
            {
                return BadRequest();
            }

            _dbContext.Entry(updatedItem).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(title))
                {
                    return NotFound();
                }
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE api/items/
        [HttpDelete("{title}")]
        public async Task<IActionResult> DeleteItem(string title)
        {
            var item = await _dbContext.ItemDatabse.FindAsync(title);
            if (item == null)
            {
                return NotFound();
            }

            _dbContext.ItemDatabse.Remove(item);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(string title)
        {
            return _dbContext.ItemDatabse.Any(e => e.Title == title);
        }
    }
}
