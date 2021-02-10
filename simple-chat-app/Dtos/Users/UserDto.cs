using SimpleChatApp.Entities;
using System.Collections.Generic;

namespace SimpleChatApp.Dtos.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
