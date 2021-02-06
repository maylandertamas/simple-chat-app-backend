using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnBackendApp.Data;
using SnBackendApp.Entities;
using System;
using System.Threading.Tasks;
using Serilog;
using SnBackendApp.Dtos.Users;

namespace SnBackendApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext context;
        public UsersController(DataContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] CreateUserDto createUser)
        {
            try
            {
                // Early out if input invalid
                if (createUser == null || string.IsNullOrWhiteSpace(createUser.Username))
                {
                    return BadRequest("Invalid username");
                }
                // Check if user exits
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username.Equals(createUser.Username));
                
                // No user found with user name: create new user
                if (user == null)
                {
                    user = new User { Username = createUser.Username };
                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();
                }
                return Ok(user);

            } 
            catch(Exception e)
            {
                Log.Logger.Information($"An error occured on login in {this.GetType()?.Name}: {e}", e);
                return BadRequest("An error occured");
            }

            
        }
    }
}
