using Application.Models;

namespace Application.Contracts.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
