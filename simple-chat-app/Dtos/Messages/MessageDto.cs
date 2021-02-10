using SimpleChatApp.Entities;
using System;

namespace SimpleChatApp.Dtos.Messages
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
