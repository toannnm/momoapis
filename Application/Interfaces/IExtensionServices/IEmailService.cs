using Domain.Entities;

namespace Application.Interfaces.IExtensionServices
{
    public interface IEmailService
    {
        Task SendEmail(User user, Order order);
    }
}