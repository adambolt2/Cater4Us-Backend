using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cater4Us_Backend.Data;
using Cater4Us_Backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Packaging.Signing;

namespace Cater4Us_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public FoodsController(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        // GET: api/Foods
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoodModel()
        {
            return await _context.FoodModel.ToListAsync();
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Food>> GetFood(Guid id)
        {
            var food = await _context.FoodModel.FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }


        [HttpGet("type/{foodType}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Food>> GetFoodByType(string foodType)
        {
            var food = await _context.FoodModel
                .Where(f => f.FoodType.Equals(foodType))
                .ToListAsync();

            if (food == null)
            {
                return NotFound();
            }

            return Ok(food);
        }

        // PUT: api/Foods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PutFood(Guid id, Food food)
        {
            if (id != food.Id)
            {
                return BadRequest();
            }

            _context.Entry(food).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Foods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Food>> PostFood(Food food)
        {

            food.Id = Guid.NewGuid();
            _context.FoodModel.Add(food);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFood", new { id = food.Id }, food);
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteFood(Guid id)
        {
            var food = await _context.FoodModel.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            _context.FoodModel.Remove(food);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodExists(Guid id)
        {
            return _context.FoodModel.Any(e => e.Id == id);
        }
    }
}
