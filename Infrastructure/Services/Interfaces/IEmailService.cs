using Core.Dto;

namespace Infrastructure.Services.Interfaces
{
    public interface IEmailService
    {
        void Send(EmailDto request);
    }
}
