using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SnBackendApp.Data;
using SnBackendApp.Dtos.Messages;
using SnBackendApp.Entities;
using SnBackendApp.Hubs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SnBackendApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly DataContext context;
        private IHubContext<ChatHub> hub;

        public MessagesController(DataContext context, IHubContext<ChatHub> hub)
        {
            this.context = context;
            this.hub = hub;
        }

        /// <summary>
        /// ROUTE: api/messages
        /// Get all messages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMessagesAsync([FromQuery] int? limit = null)
        {
            try
            {
                // NUmber of all messages in DB
                var numOfMessages = await context.Messages.CountAsync();
                // Get all messages with related users order by creation date
                var query = context.Messages
                .Include(m => m.User)
                .OrderBy(m => m.Id)
                .AsNoTracking()
                // Map to DTO
                .Select(m => new MessageDto()
                {
                    Id = m.Id,
                    Text = m.Text,
                    User = m.User,
                    CreatedAt = m.CreatedAt
                });

                // Return last x number of elements, default: return last 100 items
                query = limit.HasValue ? query.Skip(Math.Max(0, numOfMessages - limit.Value)) : query.Skip(Math.Max(0, numOfMessages - 100));

                var messages = await query.ToListAsync();

                return Ok(messages);
            }
            catch (Exception e)
            {
                Log.Logger.Information($"An error occured on get messages in {this.GetType()?.Name}: {e}", e);
                return BadRequest("An error occured");
            }
        }

        /// <summary>
        /// ROUTE: api/messages
        /// Create a message
        /// </summary>
        /// <param name="createMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMessageAsync([FromBody] CreateMessageDto createMessage)
        {
            try
            {
                // Check input validity
                if (createMessage == null || string.IsNullOrWhiteSpace(createMessage.Text) || createMessage.UserId == null)
                {
                    return BadRequest("Invalid input");
                }

                // Check if message sender user exists
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == createMessage.UserId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Create new message
                var newMessage = new Message()
                {
                    Text = createMessage.Text,
                    UserId = createMessage.UserId
                };
                // Save new message
                await context.Messages.AddAsync(newMessage);
                await context.SaveChangesAsync();

                // Query out new message to prevent reference looping
                var result = await context.Messages
                    .Where(m => m.Id == newMessage.Id)
                    // Map to DTO
                    .Select(m => new MessageDto()
                    {
                        Id = m.Id,
                        Text = m.Text,
                        User = m.User,
                        UserId = m.UserId,
                        CreatedAt = m.CreatedAt
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                // Send message through hub
                await hub.Clients.All.SendAsync("NewMessageAdded", result);

                return Ok(result);

            }
            catch (Exception e)
            {
                Log.Logger.Information($"An error occured on createing message in {this.GetType()?.Name}: {e}", e);
                return BadRequest("An error occured");
            }

        } 
    }
}
