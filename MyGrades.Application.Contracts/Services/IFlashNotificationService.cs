namespace MyGrades.Application.Contracts.Services
{
    public interface IFlashNotificationService
    {
        Task SendFlashNotificationAsync(string userId, string message, string type);
    }
}
