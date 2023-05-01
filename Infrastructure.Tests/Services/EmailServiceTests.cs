using Core.Contracts.Services.EmailService;
using Infrastructure.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Tests.Services
{
    public sealed class EmailServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            _configuration = GetConfiguration();
            _emailService = new(_configuration);
        }

        [Fact]
        public void Send_WhenCalled_ShouldSendAnEmail()
        {
            //Arrange
            EmailRequest emailRequest = new(Environment.GetEnvironmentVariable("EmailConfiguration:EmailUsername", EnvironmentVariableTarget.User)!, "You have made an order", $"Your orderId is TestId");
            
            //Act
            Action action = () => _emailService.Send(emailRequest);

            //Assert
            action.Should().NotThrow();
        }

        private static IConfiguration GetConfiguration()
        {
            Dictionary<string, string?> inMemorySettings = new() {
                {"EmailConfiguration:EmailUsername", Environment.GetEnvironmentVariable("EmailConfiguration:EmailUsername", EnvironmentVariableTarget.User)},
                {"EmailConfiguration:EmailHost", Environment.GetEnvironmentVariable("EmailConfiguration:EmailHost", EnvironmentVariableTarget.User)},
                {"EmailConfiguration:EmailPassword", Environment.GetEnvironmentVariable("EmailConfiguration:EmailPassword", EnvironmentVariableTarget.User)},
            };

            return new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        }
    }
}
