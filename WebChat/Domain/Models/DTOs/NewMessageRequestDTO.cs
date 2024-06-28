namespace WebChat.Domain.Models.DTOs
{
    public class NewMessageRequestDTO
    {
        public string Content { get; set; }
        public string ReceiverUsername { get; set; }
    }
}
