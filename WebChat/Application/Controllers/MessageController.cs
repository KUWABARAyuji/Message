using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using WebChat.Domain.Services.Interfaces;
using WebChat.Domain.Models.DTOs;

namespace WebChat.Application.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MessageController> _logger;
        private readonly IAuthService _authService;
        public MessageController(IMessageService messageService, IAuthService authService, ILogger<MessageController> logger)
        {
            _logger = logger;
            _authService = authService;
            _messageService = messageService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetMessage(int messageId)
        {
            var message = await _messageService.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                return NotFound();
            }

            // Atualiza o campo ReadAt e salva as alterações
            await _messageService.UpdateReadAtAsync(message);

            var messageDto = new MessageDTO
            {
                Id = message.Id,
                Content = message.Content,
                CreatedAt = message.CreatedAt,
                ReadAt = message.ReadAt,
                SenderUsername = (await _authService.GetUserById(message.SenderUserId)).UserName,
                ReceiverUsername = (await _authService.GetUserById(message.ReceiverUserId)).UserName
            };

            return Ok(messageDto);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMessagesByUser()
        {
            var currentUser = await _authService.GetCurrentUser();
            var messages = await _messageService.GetMessagesByUserIdAsync();

            var messageDtos = messages.Select(async m => new MessageDTO
            {
                Id = m.Id,
                Content = m.Content,
                CreatedAt = m.CreatedAt,
                ReadAt = m.ReadAt,
                SenderUsername = (await _authService.GetUserById(m.SenderUserId)).UserName,
                ReceiverUsername = (await _authService.GetUserById(m.ReceiverUserId)).UserName
            }).Select(t => t.Result).ToList();

            return Ok(messageDtos);
        }

        [HttpGet("conversations/{otherUserName}")]
        public async Task<IActionResult> GetConversationsByUser(string otherUserName)
        {
            var messages = await _messageService.GetConversationsByUserIdAsync(otherUserName);
            if (messages == null || !messages.Any())
            {
                return NotFound();
            }

            var messageDtos = messages.Select(async m => new MessageDTO
            {
                Id = m.Id,
                Content = m.Content,
                CreatedAt = m.CreatedAt,
                ReadAt = m.ReadAt,
                SenderUsername = (await _authService.GetUserById(m.SenderUserId)).UserName,
                ReceiverUsername = (await _authService.GetUserById(m.ReceiverUserId)).UserName
            }).Select(t => t.Result).ToList();

            return Ok(messageDtos);
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] NewMessageRequestDTO requestDTO)
        {
            await _messageService.SendMessageAsync(requestDTO.ReceiverUsername, requestDTO.Content);
            return Ok("Message sent successfully");
        }

        [HttpPut("update/{messageId}")]
        public async Task<IActionResult> UpdateMessage(int messageId, [FromBody] string content)
        {
            _logger.LogInformation("UpdateMessage: messageId = {}, content = {}", messageId, content);
            await _messageService.UpdateMessageAsync(messageId, content);
            return Ok("Message updated successfully");
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            await _messageService.DeleteMessageAsync(messageId);
            return Ok("Message deleted successfully");
        }

        [HttpDelete("conversation/{userId}/{contactId}")]
        public async Task<IActionResult> DeleteMessagesByConversation(string otherUserName)
        {
            await _messageService.DeleteMessagesByConversationAsync(otherUserName);
            return Ok("Conversation deleted successfully");
        }
    }

}
