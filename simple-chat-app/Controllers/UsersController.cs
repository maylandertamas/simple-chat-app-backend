using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleChatApp.Data;
using SimpleChatApp.Entities;
using System;
using System.Threading.Tasks;
using Serilog;
using SimpleChatApp.Dtos.Users;

namespace SimpleChatApp.Controllers
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

        /// <summary>
        /// ROUTE: api/users
        /// Create user
        /// </summary>
        /// <param name="createUser"></param>
        /// <returns></returns>
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
                // Map to DTO
                var map = new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username
                };

                return Ok(map);

            } 
            catch(Exception e)
            {
                Log.Logger.Information($"An error occured on login in {this.GetType()?.Name}: {e}", e);
                return BadRequest("An error occured");
            }
        }
    }
}
