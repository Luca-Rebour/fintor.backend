namespace Application.Interfaces.Services
{
    public interface IPushNotificationService
    {
        Task SendAsync(IReadOnlyList<string> tokens, string title, string body, CancellationToken cancellationToken = default);
    }
}