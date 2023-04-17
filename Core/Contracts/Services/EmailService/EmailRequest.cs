namespace Core.Contracts.Services.EmailService
{
    public sealed record EmailRequest(string To, string Subject, string Body);
}
