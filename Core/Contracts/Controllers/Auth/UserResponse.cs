namespace Core.Contracts.Controllers.Auth
{
    public sealed record UserResponse(string FirstName, string LastName, string Username, string Email, string PhoneNumber);
}
