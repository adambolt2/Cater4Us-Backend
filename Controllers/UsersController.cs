using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cater4Us_Backend.Data;
using Cater4Us_Backend.Models.Entities;
using Cater4Us_Backend.Services;
using Cater4Us_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Caching.Memory;

// Quick breakdown on whats happening here
// This is now restricted in a way that only the website and Admin Users can access this using JWT tokens
// Frontend sends a request to get a token (only acceptable within the correct CORS, specified in Program.cs
// After so, we create a custom JWT authentication using singletons and now we allow our website to do said expressions 
// This is a whole setup for admin/website/customer stuff
// implemented a BFF in the front-end to handle this 

namespace Cater4Us_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtTokenService _tokenService;
        private readonly IMemoryCache _memoryCache;
        public UsersController(ApplicationDbContext context, JwtTokenService tokenService, IMemoryCache memoryCache)
        {
            _context = context;
            _tokenService = tokenService;
            _memoryCache = memoryCache;
        }


        [HttpGet("generate-admin-token")]
        [EnableCors("AllowSpecificOrigins")]
        public IActionResult GenerateAdminToken()
        {
            // Generate a random identifier (e.g., a GUID) for the token
            var uniqueIdentifier = Guid.NewGuid().ToString();

            // Assign the admin role
            var adminRole = 1; // Admin role

            // Generate the token
            var token = _tokenService.GenerateToken(uniqueIdentifier, 0, adminRole);

            // Store the token in memory with an expiration time
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1)); // Set to match token's lifetime

            _memoryCache.Set("AdminToken", token, cacheEntryOptions);

            return Ok(new { Token = token });
        }



        [HttpGet("get-stored-token")]
        [Authorize(Policy = "AdminOnly")] // Restrict access to users with role "1"

        public IActionResult GetStoredToken()
        {
            // Try to retrieve the token from the memory cache
            if (_memoryCache.TryGetValue("AdminToken", out string token))
            {
                return Ok(new { Token = token });
            }
            return NotFound("Token not found.");
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            // Find the user by email and password
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            // Generate JWT token
            var token = _tokenService.GenerateToken(user.Email,user.Id,user.Role);

            return Ok(new { Token = token });
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")] // Restrict access to users with role "1"

        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")] // Restrict access to users with role "1"
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "AdminOnly")] // Restrict access to users with role "1"

        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] // Restrict access to users with role "1"

        public async Task<IActionResult> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
