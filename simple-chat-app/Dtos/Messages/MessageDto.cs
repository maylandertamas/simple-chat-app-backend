using SnBackendApp.Entities;
using System;

namespace SnBackendApp.Dtos.Messages
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
