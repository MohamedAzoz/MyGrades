using Microsoft.AspNetCore.SignalR;
using MyGrades.Application.Contracts.Services;

namespace MyGrades.API.Hubs
{
    public class FlashNotificationService : IFlashNotificationService
    {
        // استخدام IHubContext لبث الإشعارات
        private readonly IHubContext<NotificationHub> _hubContext;
        public FlashNotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendFlashNotificationAsync(string userId, string message, string type)
        {
            // 1. إعداد كائن الإشعار
            var notification = new
            {
                Message = message,
                Type = type, // (Success, Error, Warning)
                Timestamp = DateTime.UtcNow
            };

            // 2. الإرسال إلى مستخدم معين
            // .User(userId) تحدد أن الإشعار سيذهب فقط للمستخدم الذي يحمل هذا الـ userId
            // "ReceiveNotification" هو اسم الـ Method التي يجب أن يستمع إليها العميل (Front-end)
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }
    }
}
