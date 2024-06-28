using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebChat.Domain.Models;
using WebChat.Infrastructure.Data.Context;

namespace WebChat.Infrastructure.Data.Repositories
{

    public class MessageRepository
    {
        private readonly MySQLContext _context;

        public MessageRepository(MySQLContext context)
        {
            _context = context;
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _context.Message
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<IEnumerable<Message>> GetMessagesByUserIdAsync(string userId)
        {
            return await _context.Message
                .Where(m => m.SenderUserId == userId || m.ReceiverUserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetConversations(string userId1, string userId2)
        {
            // TODO check it
            return await _context.Message
                .Where(m =>
                    m.SenderUserId == userId1 && m.ReceiverUserId == userId2 ||
                    m.SenderUserId == userId2 && m.ReceiverUserId == userId1)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Message> CreateMessage(Message message)
        {
            var ret = await _context.Message.AddAsync(message);
            await _context.SaveChangesAsync();
            ret.State = EntityState.Detached;
            return ret.Entity;
        }

        public async Task<int> UpdateMessage(Message message)
        {
            _context.Entry(message).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMessage(int messageId)
        {
            var message = await _context.Message.FindAsync(messageId);
            if (message != null)
            {
                _context.Message.Remove(message);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteConversation(string userId1, string userId2)
        {
            var messages = await _context.Message
                .Where(m => m.SenderUserId == userId1 && m.ReceiverUserId == userId2 ||
                            m.SenderUserId == userId2 && m.ReceiverUserId == userId1)
                .ToListAsync();
            if (messages.Count == 0)
            {
                return false;
            }
            _context.Message.RemoveRange(messages);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}