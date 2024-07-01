using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebChat.Domain.Models;
using WebChat.Domain.Models.DTOs;
using WebChat.Domain.Services.Interfaces;
using WebChat.Infrastructure.Data.Repositories;

namespace WebChat.Domain.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly MessageRepository _messageRepository;
        private readonly IAuthService _authService;
        private readonly UserRepository _userRepository;

        public MessageService(MessageRepository messageRepository, IAuthService authService, UserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                throw new Exception("Message not found.");
            }

            message.ReadAt = DateTime.UtcNow;
            await _messageRepository.UpdateMessage(message);

            return message;
        }

        public async Task<IEnumerable<Message>> GetMessagesByUserIdAsync(string userId = null)
        {
            var currentUser = await _authService.GetCurrentUser();
            userId ??= currentUser.Id;

            return await _messageRepository.GetMessagesByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Message>> GetConversationsByUserIdAsync(string otherUserId)
        {
            var currentUser = await _authService.GetCurrentUser();
            return await _messageRepository.GetConversations(currentUser.Id, otherUserId);
        }

        public async Task SendMessageAsync(string sendToName, string content)
        {
            var sender = await _authService.GetCurrentUser();
            var receiver = await _authService.GetUserByUserName(sendToName);

            if (receiver == null)
            {
                throw new Exception("Receiver not found.");
            }

            var message = new Message
            {
                SenderUserId = sender.Id,
                ReceiverUserId = receiver.Id,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            await _messageRepository.CreateMessage(message);
        }

        public async Task UpdateMessageAsync(int messageId, string content)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                throw new Exception("Message not found.");
            }

            message.Content = content;
            await _messageRepository.UpdateMessage(message);
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            await _messageRepository.DeleteMessage(messageId);
        }

        public async Task DeleteMessagesByConversationAsync(string otherUserId)
        {
            var currentUser = await _authService.GetCurrentUser();
            var otherUser = await _authService.GetUserById(otherUserId);

            if (otherUser == null)
            {
                throw new Exception("User not found.");
            }

            await _messageRepository.DeleteConversation(currentUser.Id, otherUser.Id);
        }

    }
}
