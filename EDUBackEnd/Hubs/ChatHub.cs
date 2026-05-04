using EDUBackEnd.Data;
using EDUBackEnd.Models.Chat;
using EDUBackEnd.Models.Notificaiton;
using Microsoft.AspNetCore.SignalR;

namespace EDUBackEnd.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return Context.User?.FindFirst("sub")?.Value;
        }

        // =========================
        // PRIVATE CHAT
        // =========================
        public async Task SendPrivateMessage(string receiverId, string message)
        {
            var senderId = GetUserId();
            if (string.IsNullOrEmpty(senderId)) return;

            var msg = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Text = message,
                SentAt = DateTime.UtcNow
            };

            await _context.Messages.AddAsync(msg);
            await _context.SaveChangesAsync();

            await Clients.User(receiverId)
                .SendAsync("ReceiveMessage", senderId, message);

            await Clients.User(receiverId)
                .SendAsync("ChatListUpdated");
        }

        // =========================
        // CLASS CHAT (ÓRA)
        // =========================

        public async Task JoinClassRoom(string eventId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, eventId);
        }

        public async Task LeaveClassRoom(string eventId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, eventId);
        }

        public async Task SendMessageToClass(int eventId, string message)
        {
            var senderId = GetUserId();
            if (string.IsNullOrEmpty(senderId)) return;

            var msg = new Message
            {
                SenderId = senderId,
                Text = message,
                SentAt = DateTime.UtcNow,
                EventId = eventId
            };

            await _context.Messages.AddAsync(msg);
            await _context.SaveChangesAsync();

            await Clients.Group(eventId.ToString())
                .SendAsync("ReceiveClassMessage", senderId, message);
        }

        // =========================
        // ONLINE STATUS
        // =========================

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return;

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsOnline = true;
                await _context.SaveChangesAsync();
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return;

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsOnline = false;
                await _context.SaveChangesAsync();
            }

            await base.OnDisconnectedAsync(exception);
        }

        // =========================
        // TYPING
        // =========================

        public async Task Typing(string receiverId)
        {
            var senderId = GetUserId();

            await Clients.User(receiverId)
                .SendAsync("Typing", senderId);
        }

        public async Task StopTyping(string receiverId)
        {
            var senderId = GetUserId();

            await Clients.User(receiverId)
                .SendAsync("StoppedTyping", senderId);
        }
    }
}