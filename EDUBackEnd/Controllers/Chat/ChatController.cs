using EDUBackEnd.Data;
using EDUBackEnd.Dtos.Chat;
using EDUBackEnd.Hubs;
using EDUBackEnd.Models.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EDUBackEnd.Controllers.Chat
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(AppDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        private string? GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

       
        [HttpGet("chat-list")]
        public async Task<ActionResult<List<ChatListItemDto>>> GetChatList()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized("User ID not found in token");

            var chats = await _context.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g => new ChatListItemDto
                {
                    UserId = g.Key,
                    LastMessage = g.OrderByDescending(m => m.SentAt)
                                   .Select(m => m.Text)
                                   .FirstOrDefault(),

                    LastMessageTime = g.Max(m => m.SentAt),

                    IsSeen = g.OrderByDescending(m => m.SentAt)
                              .Select(m => m.IsSeen)
                              .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(chats);
        }

        [HttpPost("mark-as-seen")]
        public async Task<IActionResult> MarkAsSeen(string otherUserId)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized("User ID not found");

            var messages = await _context.Messages
                .Where(m => m.SenderId == otherUserId &&
                            m.ReceiverId == userId &&
                            !m.IsSeen)
                .ToListAsync();

            foreach (var msg in messages)
            {
                msg.IsSeen = true;
            }

            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(otherUserId)
                .SendAsync("Seen", userId);

            return Ok();
        }

        [HttpGet("class-messages/{eventId}")]
        public async Task<IActionResult> GetClassMessages(int eventId)
        {
            var messages = await _context.Messages
                .Where(m => m.EventId == eventId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            return Ok(messages);
        }

        [HttpPost("messages")]
        public async Task<IActionResult> SaveMessage([FromBody] CreateMessageDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized("User ID not found in token");

            var msg = new Message
            {
                SenderId = userId,
                EventId = dto.EventId,
                Text = dto.Text,
                FileUrl = dto.FileUrl,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group(dto.EventId.ToString())
                .SendAsync("ReceiveClassMessage", msg);

            return Ok(msg);
        }
    }
}