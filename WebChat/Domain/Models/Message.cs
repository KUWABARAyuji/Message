using System;

namespace WebChat.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderUserId { get; set; }
        public ApplicationUser SenderUser { get; set; }
        public string ReceiverUserId { get; set; }
        public ApplicationUser ReceiverUser { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
