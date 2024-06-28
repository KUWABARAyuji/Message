using System;

namespace WebChat.Domain.Models.DTOs
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
    }
}
