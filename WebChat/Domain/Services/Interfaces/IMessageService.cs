using System.Collections.Generic;
using System.Threading.Tasks;
using WebChat.Domain.Models;

namespace WebChat.Domain.Services.Interfaces
{
    public interface IMessageService
    {
        Task<Message> GetMessageByIdAsync(int messageId);
        Task<IEnumerable<Message>> GetMessagesByUserIdAsync();
        Task<IEnumerable<Message>> GetConversationsByUserIdAsync(string sendTo);
        Task SendMessageAsync(string sendTo, string content);
        Task UpdateMessageAsync(int id, string message);
        Task DeleteMessageAsync(int messageId);
        Task DeleteMessagesByConversationAsync(string contactId);
        Task UpdateReadAtAsync(Message message);


    }
}
