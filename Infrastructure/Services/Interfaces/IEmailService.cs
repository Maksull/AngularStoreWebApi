using Core.Contracts.Services.EmailService;

namespace Infrastructure.Services.Interfaces
{
    public interface IEmailService
    {
        void Send(EmailRequest request);
    }
}
