using EmailService.Models;

namespace EmailService.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(Receipt receipt);
    }
}
