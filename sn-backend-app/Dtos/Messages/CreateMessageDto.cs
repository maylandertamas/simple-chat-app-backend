using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnBackendApp.Dtos.Messages
{
    public class CreateMessageDto
    {
        public string Text { get; set; }
        public int UserId { get; set; }
    }
}
