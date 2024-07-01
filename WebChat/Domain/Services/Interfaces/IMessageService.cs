using System.Collections.Generic;
using System.Threading.Tasks;
using WebChat.Domain.Models;
using WebChat.Domain.Models.DTOs;

namespace WebChat.Domain.Services.Interfaces
{
    public interface IMessageService
    {
        Task<Message> GetMessageByIdAsync(int messageId);
        Task<IEnumerable<Message>> GetMessagesByUserIdAsync(string userId);
        Task<IEnumerable<Message>> GetConversationsByUserIdAsync(string otherUserId);
        Task SendMessageAsync(string sendToName, string content);
        Task UpdateMessageAsync(int id, string content);
        Task DeleteMessageAsync(int messageId);
        Task DeleteMessagesByConversationAsync(string otherUserId);
    }
}
