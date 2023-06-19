using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Core.Mediator.Queries.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace WebApi.Tests.Controllers
{
    public sealed class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<Serilog.ILogger> _logger;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediator = new();
            _logger = new();
            _controller = new(_mediator.Object, _logger.Object);
        }


        #region Login

        [Fact]
        public void Login_WhenCalled_ReturnOk()
        {
            //Arrange
            LoginRequest loginRequest = new("First", "Password");
            JwtResponse jwtResponse = new("jwt", new());

            _mediator.Setup(m => m.Send(It.IsAny<LoginCommand>(), default))
                .ReturnsAsync(jwtResponse);

            //Act
            var response = (_controller.Login(loginRequest).Result as OkObjectResult)!;
            var result = response.Value as JwtResponse;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<JwtResponse>();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(jwtResponse);
        }

        [Fact]
        public void Login_WhenCalled_ReturnBadRequest()
        {
            //Arrange
            LoginRequest loginRequest = new("First", "Password");
            JwtResponse jwtResponse = new("jwt", new());

            _mediator.Setup(m => m.Send(It.IsAny<LoginCommand>(), default))
                .ReturnsAsync((JwtResponse)null!);


            //Act
            var response = _controller.Login(loginRequest).Result;
            var result = (response as BadRequestObjectResult)!;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().BeOfType<string>();
        }

        #endregion


        #region Register

        [Fact]
        public void Register_WhenCalled_ReturnOk()
        {
            //Arrange
            RegisterRequest registerRequest = new("First", "Second", "Name", "Email", "+380554447788", "Password", "Password");
            List<string> errors = new();

            _mediator.Setup(m => m.Send(It.IsAny<RegisterCommand>(), default))
                .ReturnsAsync(errors);

            //Act
            var response = (_controller.Register(registerRequest).Result as OkResult)!;

            //Assert
            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void Register_WhenCalled_ReturnBadRequest()
        {
            //Arrange
            RegisterRequest registerRequest = new("First", "Second", "Name", "Email", "+380554447788", "Password", "Password");
            List<string> errors = new()
            {
                "error"
            };

            _mediator.Setup(m => m.Send(It.IsAny<RegisterCommand>(), default))
                .ReturnsAsync(errors);


            //Act
            var response = _controller.Register(registerRequest).Result;
            var result = (response as BadRequestObjectResult)!;
            var value = (result.Value as RegisterFailed)!;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            value.Should().BeOfType<RegisterFailed>();
            value.Errors.Should().BeEquivalentTo(errors);
        }

        #endregion


        #region RefreshJwt

        [Fact]
        public void RefreshJwt_WhenCalled_ReturnOk()
        {
            //Arrange
            RefreshTokenRequest refreshTokenRequest = new("Token", DateTime.Now);
            JwtResponse jwtResponse = new("jwt", new());

            _mediator.Setup(m => m.Send(It.IsAny<RefreshCommand>(), default))
                .ReturnsAsync(jwtResponse);

            //Act
            var response = (_controller.RefreshJwt(refreshTokenRequest).Result as OkObjectResult)!;
            var result = response.Value as JwtResponse;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<JwtResponse>();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(jwtResponse);
        }

        [Fact]
        public void RefreshJwt_WhenCalled_ReturnBadRequest()
        {
            //Arrange
            RefreshTokenRequest refreshTokenRequest = new("Token", DateTime.Now);
            JwtResponse jwtResponse = new("jwt", new());

            _mediator.Setup(m => m.Send(It.IsAny<RefreshCommand>(), default))
                .ReturnsAsync((JwtResponse)null!);


            //Act
            var response = _controller.RefreshJwt(refreshTokenRequest).Result;
            var result = (response as UnauthorizedResult)!;

            //Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        #endregion


        #region GeUserData

        [Fact]
        public void GetUserData_WhenCalled_ReturnOk()
        {
            //Arrange
            UserResponse userResponse = new("First", "Last", "Username", "my_email@gog.co", "+10324114617");
            _mediator.Setup(m => m.Send(It.IsAny<GetUserDataQuery>(), default))
                .ReturnsAsync(userResponse);

            //Act
            var response = (_controller.GetUserData().Result as OkObjectResult)!;
            var result = response.Value as UserResponse;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<UserResponse>();
            result.Should().BeEquivalentTo(userResponse);
        }

        [Fact]
        public void GetUserData_WhenCalled_ReturnNotFound()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetUserDataQuery>(), default))
                .ReturnsAsync((UserResponse)null!);

            //Act
            var response = _controller.GetUserData().Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion


        #region ConfirmEmail

        [Fact]
        public void ConfirmEmail_WhenCalled_ReturnOk()
        {
            //Arrange
            ConfirmEmailCommand confirmEmailCommand = new(Guid.NewGuid().ToString(), "Token");
            _mediator.Setup(m => m.Send(It.IsAny<ConfirmEmailCommand>(), default))
                .ReturnsAsync(true);

            //Act
            var response = (_controller.ConfirmEmail(confirmEmailCommand.UserId, confirmEmailCommand.Token).Result as OkResult)!;

            //Assert
            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ConfirmEmail_WhenCalled_ReturnNotFound()
        {
            //Arrange
            ConfirmEmailCommand confirmEmailCommand = new(Guid.Empty.ToString(), "Token");
            _mediator.Setup(m => m.Send(It.IsAny<ConfirmEmailCommand>(), default))
                .ReturnsAsync(false);

            //Act
            var response = (_controller.ConfirmEmail(confirmEmailCommand.UserId, confirmEmailCommand.Token).Result as NotFoundResult)!;

            //Assert
            response.Should().BeOfType<NotFoundResult>();
        }

        #endregion


        #region ResetPassword

        [Fact]
        public void ResetPassword_WhenCalled_ReturnOk()
        {
            //Arrange
            ResetPasswordCommand resetPasswordCommand = new(Guid.NewGuid().ToString(), "username");
            _mediator.Setup(m => m.Send(It.IsAny<ResetPasswordCommand>(), default))
                .ReturnsAsync(true);

            //Act
            var response = (_controller.ResetPassword(resetPasswordCommand.UserId, resetPasswordCommand.Username).Result as OkResult)!;

            //Assert
            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ResetPassword_WhenCalled_ReturnNotFound()
        {
            //Arrange
            ResetPasswordCommand resetPasswordCommand = new(Guid.Empty.ToString(), "username");
            _mediator.Setup(m => m.Send(It.IsAny<ResetPasswordCommand>(), default))
                .ReturnsAsync(false);

            //Act
            var response = (_controller.ResetPassword(resetPasswordCommand.UserId, resetPasswordCommand.Username).Result as NotFoundResult)!;

            //Assert
            response.Should().BeOfType<NotFoundResult>();
        }

        #endregion


        #region ConfirmResetPassword

        [Fact]
        public void ConfirmResetPassword_WhenCalled_ReturnOk()
        {
            //Arrange
            ConfirmResetPasswordCommand confirmResetPasswordCommand = new(Guid.NewGuid().ToString(), "token", "newPassword");
            _mediator.Setup(m => m.Send(It.IsAny<ConfirmResetPasswordCommand>(), default))
                .ReturnsAsync(new List<string>());

            //Act
            var response = (_controller.ConfirmResetPassword(confirmResetPasswordCommand.UserId, confirmResetPasswordCommand.Token, confirmResetPasswordCommand.NewPassword)
                .Result as OkResult)!;

            //Assert
            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ConfirmResetPassword_WhenCalled_ReturnNotFound()
        {
            //Arrange
            List<string> errors = new()
            {
                "Test error",
            };
            ConfirmResetPasswordCommand confirmResetPasswordCommand = new(Guid.Empty.ToString(), "token", "newPassword");
            _mediator.Setup(m => m.Send(It.IsAny<ConfirmResetPasswordCommand>(), default))
                .ReturnsAsync(errors);

            //Assert
            var response = _controller.ConfirmResetPassword(confirmResetPasswordCommand.UserId, confirmResetPasswordCommand.Token, confirmResetPasswordCommand.NewPassword).Result;
            var result = (response as BadRequestObjectResult)!;
            var value = (result.Value as ConfirmResetPasswordFailed)!;

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            value.Should().BeOfType<ConfirmResetPasswordFailed>();
            value.Errors.Should().BeEquivalentTo(errors);
        }

        #endregion


        [Fact]
        public void Protected_WhenCalled_ReturnOk()
        {
            //Arrange

            //Act
            var response = (_controller.Protected() as OkResult)!;

            //Assert
            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void AdminProtected_WhenCalled_ReturnOk()
        {
            //Arrange

            //Act
            var response = (_controller.AdminProtected() as OkResult)!;

            //Assert
            response.Should().BeOfType<OkResult>();
        }
    }
}
