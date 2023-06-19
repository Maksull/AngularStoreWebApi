using Core.Contracts.Controllers.Auth;
using Core.Entities;
using Infrastructure.Mapster;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
using Moq;
using System.Security.Claims;

namespace Infrastructure.Tests.Services
{
    public sealed class AuthServiceTests
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IEmailService> _emailService;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mapper = GetMapper();
            _configuration = GetConfiguration();
            _userManager = new(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _emailService = new();
            _authService = new(_mapper, _configuration, _userManager.Object, _emailService.Object);
        }


        #region Login

        [Fact]
        public async Task Login_WhenUserExists_And_CorrectPassword_ReturnJwtResponse()
        {
            // Arrange
            LoginRequest login = new("Username", "Password");

            User user = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            _userManager.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _userManager.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _userManager.Setup(u => u.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            var result = (await _authService.Login(login))!;

            // Assert
            result.Should().BeOfType<JwtResponse>();
            result.Jwt.Should().BeOfType<string>();
            result.Jwt.Should().NotBeEmpty();
            result.RefreshToken.Should().BeOfType<RefreshToken>();
            result.RefreshToken.Token.Should().BeOfType<string>();
            result.RefreshToken.Token.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Login_WhenUserExists_And_EmailNotConfirmed_ReturnNull()
        {
            // Arrange
            LoginRequest login = new("Username", "Password");

            User user = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            _userManager.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _userManager.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _userManager.Setup(u => u.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act
            var result = await _authService.Login(login);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Login_WhenUserExists_And_WrongPassword_ReturnNull()
        {
            // Arrange
            LoginRequest login = new("Username", "Password");

            User user = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            _userManager.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _userManager.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = (await _authService.Login(login))!;

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Login_WhenUserNotExists_ReturnNull()
        {
            // Arrange
            LoginRequest login = new("Username", "Password");

            _userManager.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = (await _authService.Login(login))!;

            // Assert
            result.Should().BeNull();
        }

        #endregion


        #region GetUserData

        [Fact]
        public async Task GetUserData_WhenUserExists_ReturnUserData()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            User user = new()
            {
                Id = id,
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                RefreshToken = "",
                RefreshTokenExpired = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            User[] users = { user };

            var mock = users.AsQueryable().BuildMock();
            _userManager.Setup(m => m.Users)
                .Returns(mock);

            var userRequest = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, id)
            }));

            // Act
            var result = (await _authService.GetUserData(userRequest))!;

            // Assert
            result.Should().BeOfType<UserResponse>();
            result.FirstName.Should().BeEquivalentTo(user.FirstName);
            result.LastName.Should().BeEquivalentTo(user.FirstName);
            result.Username.Should().BeEquivalentTo(user.UserName);
            result.Email.Should().BeEquivalentTo(user.Email);
            result.PhoneNumber.Should().BeEquivalentTo(user.PhoneNumber);

        }

        [Fact]
        public async Task GetUserData_WhenUserNotExists_ReturnNull()
        {
            //Arrange
            User[] users = Array.Empty<User>();

            var mock = users.AsQueryable().BuildMock();
            _userManager.Setup(m => m.Users)
                .Returns(mock);

            var userRequest = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            }));

            // Act
            var result = await _authService.GetUserData(userRequest);

            // Assert
            result.Should().BeNull();
        }

        #endregion


        #region Register

        [Fact]
        public async Task Register_WhenSucceeded_ReturnEmptyList()
        {
            // Arrange
            RegisterRequest registerRequest = new("First", "Second", "Name", "Email", "PhoneNumber", "Password", "Password");

            _userManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = (await _authService.Register(registerRequest))!;

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Register_WhenNotSucceeded_ReturnErrors()
        {
            // Arrange
            RegisterRequest registerRequest = new("First", "Second", "Name", "Email", "PhoneNumber", "Password", "Password");

            _userManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = (await _authService.Register(registerRequest))!;

            // Assert
            result.Should().BeOfType<List<string>>();
        }

        #endregion


        #region ConfirmEmail

        [Fact]
        public async Task ConfirmEmail_WhenSucceeded_ReturnEmptyList()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            string token = "Token";
            User user = new()
            {
                Id = id,
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = true,
                RefreshToken = "",
                RefreshTokenExpired = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManager.Setup(m => m.ConfirmEmailAsync(user, It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = (await _authService.ConfirmEmail(id, token))!;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ConfirmEmail_WhenUserNotExist_ReturnFalse()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            string token = "Token";
            User user = new()
            {
                Id = id,
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = true,
                RefreshToken = "",
                RefreshTokenExpired = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            // Act
            var result = (await _authService.ConfirmEmail(id, token))!;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ConfirmEmail_WhenNotSucceeded_ReturnFalse()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            string token = "Token";
            User user = new()
            {
                Id = id,
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = true,
                RefreshToken = "",
                RefreshTokenExpired = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _userManager.Setup(m => m.ConfirmEmailAsync(user, It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            // Act
            var result = (await _authService.ConfirmEmail(id, token))!;

            // Assert
            result.Should().BeFalse();
        }

        #endregion


        #region ResetPassword

        [Fact]
        public async Task ResetPassword_When_SearchByIdSucceeded_ReturnTrue()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            User user = new()
            {
                Id = id,
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = true,
                RefreshToken = "",
                RefreshTokenExpired = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var result = (await _authService.ResetPassword(id, null))!;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ResetPassword_When_SearchByUsernameSucceeded_ReturnTrue()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            User user = new()
            {
                Id = id,
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = true,
                RefreshToken = "",
                RefreshTokenExpired = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            _userManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var result = (await _authService.ResetPassword(null, user.UserName))!;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ResetPassword_When_UserNotFound_ReturnFalse()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            User user = new()
            {
                Id = id,
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = true,
                RefreshToken = "",
                RefreshTokenExpired = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            // Act
            var result = (await _authService.ResetPassword(user.Id, user.UserName))!;

            // Assert
            result.Should().BeFalse();
        }

        #endregion


        #region ConfirmResetPassword

        [Fact]
        public async Task ConfirmResetPassword_WhenSucceeded_ReturnEmptyList()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            User user = new()
            {
                Id = id,
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = true,
                RefreshToken = "",
                RefreshTokenExpired = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManager.Setup(m => m.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = (await _authService.ConfirmResetPassword(id, "token", "newPassword"))!.ToList();

            // Assert
            result.Should().BeOfType<List<string>>();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ConfirmResetPassword_When_UserNotExist_ReturnListWithError()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            User user = new()
            {
                Id = id,
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = true,
                RefreshToken = "",
                RefreshTokenExpired = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = (await _authService.ConfirmResetPassword(id, "token", "newPassword"))!;

            // Assert
            result.Should().BeOfType<List<string>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task ConfirmResetPassword_WhenFailed_ReturnListWithErrors()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            User user = new()
            {
                Id = id,
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = true,
                RefreshToken = "",
                RefreshTokenExpired = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            IdentityError[] errors =
            {
                new(){
                    Description = "TestError1",
                },
                new(){
                    Description = "TestError2",
                },
            };

            _userManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManager.Setup(m => m.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors));

            // Act
            var result = (await _authService.ConfirmResetPassword(id, "token", "newPassword"))!;

            // Assert
            result.Should().BeOfType<List<string>>();
            result.Should().HaveCount(2);
        }

        #endregion


        #region Refresh

        [Fact]
        public async Task Refresh_WhenUserExists_ReturnJwtResponse()
        {
            // Arrange
            RefreshTokenRequest refreshTokenRequest = new("Token", DateTime.Now);

            User user = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Username",
                NormalizedUserName = "USERNAME",
                Email = "email",
                NormalizedEmail = "EMAIL",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                RefreshToken = refreshTokenRequest.Token,
                RefreshTokenExpired = refreshTokenRequest.Expired,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            User[] users = { user };
            var mock = users.AsQueryable().BuildMock();

            _userManager.Setup(m => m.Users)
                .Returns(mock);

            // Act
            var result = (await _authService.Refresh(refreshTokenRequest))!;

            // Assert
            result.Should().BeOfType<JwtResponse>();
            result.Jwt.Should().BeOfType<string>();
            result.Jwt.Should().NotBeEmpty();
            result.RefreshToken.Should().BeOfType<RefreshToken>();
            result.RefreshToken.Token.Should().BeOfType<string>();
            result.RefreshToken.Token.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Refresh_WhenNotSucceeded_ReturnNull()
        {
            // Arrange
            RefreshTokenRequest refreshTokenRequest = new("Token", DateTime.Now);
            User[] users = { };
            var mock = users.AsQueryable().BuildMock();

            _userManager.Setup(m => m.Users)
                .Returns(mock);

            // Act
            var result = (await _authService.Refresh(refreshTokenRequest))!;

            // Assert
            result.Should().BeNull();
        }

        #endregion


        private static Mapper GetMapper()
        {
            TypeAdapterConfig config = new();
            config.Apply(new MapsterRegister());

            return new Mapper(config);
        }

        private static IConfiguration GetConfiguration()
        {
            Dictionary<string, string?> inMemorySettings = new() {
                {"JwtSettings:SecurityKey", "MyTestsAuthServiceSecurityKey"},
                {"JwtSettings:ExpiresInMinutes", "30"},
            };

            return new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        }
    }
}
