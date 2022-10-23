using WebApi.Models.Dto;

namespace WebApi.Services.EmailService
{
    public interface IEmailService
    {
        void Send(EmailDto request);
    }
}
