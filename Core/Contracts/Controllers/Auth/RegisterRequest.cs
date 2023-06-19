namespace Core.Contracts.Controllers.Auth
{
    public sealed record RegisterRequest(string FirstName, string LastName, string Username, string Email, string PhoneNumber, string Password, string ConfirmPassword);
}
