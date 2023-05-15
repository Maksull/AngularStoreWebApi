using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Core.Mediator.Queries.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests.Controllers
{
    public sealed class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediator = new();
            _controller = new(_mediator.Object);
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
            RegisterRequest registerRequest = new("First", "Second", "Name", "Email", "Password", "Password");

            _mediator.Setup(m => m.Send(It.IsAny<RegisterCommand>(), default))
                .ReturnsAsync(true);

            //Act
            var response = (_controller.Register(registerRequest).Result as OkResult)!;

            //Assert
            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void Register_WhenCalled_ReturnBadRequest()
        {
            //Arrange
            RegisterRequest registerRequest = new("First", "Second", "Name", "Email", "Password", "Password");

            _mediator.Setup(m => m.Send(It.IsAny<RegisterCommand>(), default))
                .ReturnsAsync(false);


            //Act
            var response = _controller.Register(registerRequest).Result;
            var result = (response as BadRequestResult)!;

            //Assert
            result.Should().BeOfType<BadRequestResult>();
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
