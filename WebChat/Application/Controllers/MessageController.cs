using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using WebChat.Domain.Services.Interfaces;
using WebChat.Domain.Models;
using System;
using WebChat.Domain.Models.DTOs;

namespace WebChat.Application.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("get")]
        public async Task<ActionResult<Message>> GetMessageByIdAsync([FromQuery] int messageId)
        {
            try
            {
                var message = await _messageService.GetMessageByIdAsync(messageId);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all-messages-from-user")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByUser([FromQuery] string userId)
        {
            try
            {
                var messagesDTO = await _messageService.GetMessagesByUserIdAsync(userId);
                return Ok(messagesDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("conversation-with-other-user")]
        public async Task<ActionResult<IEnumerable<Message>>> GetConversations([FromQuery] string otherUserName)
        {
            try
            {
                var messages = await _messageService.GetConversationsByUserIdAsync(otherUserName);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] NewMessageRequestDTO requestDTO)
        {
            try
            {
                await _messageService.SendMessageAsync(requestDTO.ReceiverUsername, requestDTO.Content);
                return Ok("Message sent successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateMessage(int messageId, [FromBody] string content)
        {
            try
            {
                await _messageService.UpdateMessageAsync(messageId, content);
                return Ok("Message updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete-message-by-body")]
        public async Task<ActionResult> DeleteMessageByBody([FromBody] int messageId)
        {
            try
            {
                await _messageService.DeleteMessageAsync(messageId);
                return Ok("Message deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete-conversation")]
        public async Task<IActionResult> DeleteMessagesByConversation([FromBody] string otherUserId)
        {
            try
            {
                await _messageService.DeleteMessagesByConversationAsync(otherUserId);
                return Ok("Conversation deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
