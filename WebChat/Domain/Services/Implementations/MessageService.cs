using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebChat.Domain.Models;
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
                return null;
            }

            var currentUser = await _authService.GetCurrentUser();

            // Verifica se o usuário atual é o remetente ou destinatário da mensagem
            if (message.SenderUserId != currentUser.Id && message.ReceiverUserId != currentUser.Id)
            {
                throw new UnauthorizedAccessException("You do not have permission to view this message.");
            }

            return message;
        }

        public async Task UpdateReadAtAsync(Message message)
        {
            message.ReadAt = DateTime.UtcNow;
            await _messageRepository.UpdateMessage(message);
        }


        public async Task<IEnumerable<Message>> GetMessagesByUserIdAsync()
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            return await _messageRepository.GetMessagesByUserIdAsync(currentUser.Id);
        }

        public async Task<IEnumerable<Message>> GetConversationsByUserIdAsync(string otherUserId)
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            return await _messageRepository.GetConversations(currentUser.Id, otherUserId);
        }

        public async Task SendMessageAsync(string sendToName, string content)
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            var sendToUser = await _authService.GetUserByUserName(sendToName);
            var message = new Message
            {
                SenderUserId = currentUser.Id,
                ReceiverUserId = sendToUser.Id,
                Content = content,
                CreatedAt = DateTime.Now
            };
            await _messageRepository.CreateMessage(message);
        }

        public async Task UpdateMessageAsync(int id, string content)
        {
            // Obtém a mensagem com o ID fornecido
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null)
            {
                throw new Exception("Message not found.");
            }

            // Obtém o usuário atual
            var currentUser = await _authService.GetCurrentUser();

            // Verifica se o remetente da mensagem é o usuário atual
            if (message.SenderUserId != currentUser.Id)
            {
                throw new UnauthorizedAccessException("You can only update your own messages.");
            }

            // Atualiza o conteúdo da mensagem
            message.Content = content;

            // Salva as alterações
            await _messageRepository.UpdateMessage(message);
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            // Obtém a mensagem com o ID fornecido
            var message = await _messageRepository.GetMessageByIdAsync(messageId) ?? throw new Exception("Message not found.");

            // Obtém o usuário atual
            var currentUser = await _authService.GetCurrentUser();

            // Verifica se o remetente da mensagem é o usuário atual
            if (message.SenderUserId != currentUser.Id)
            {
                throw new UnauthorizedAccessException("You can only delete your own messages.");
            }

            // Exclui a mensagem
            await _messageRepository.DeleteMessage(messageId);
        }

        public async Task DeleteMessagesByConversationAsync(string otherUsername)
        {
            // Obtém o usuário atual
            var currentUser = await _authService.GetCurrentUser();

            // Obtém o ID do outro usuário com base no nome de usuário fornecido
            var otherUser = await _userRepository.GetUserByUsername(otherUsername) ?? throw new Exception("User not found.");

            // Exclui todas as mensagens da conversa entre os dois usuários
            await _messageRepository.DeleteConversation(currentUser.Id, otherUser.Id);
        }

    }
}

