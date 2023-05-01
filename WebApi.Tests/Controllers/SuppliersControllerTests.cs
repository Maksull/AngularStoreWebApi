﻿using Core.Contracts.Controllers.Suppliers;
using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Core.Mediator.Queries.Suppliers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests.Controllers
{
    public sealed class SuppliersControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly SuppliersController _controller;

        public SuppliersControllerTests()
        {
            _mediator = new();
            _controller = new(_mediator.Object);
        }

        #region GetSuppliers

        [Fact]
        public void GetSuppliers_WhenCalled_ReturnOk()
        {
            //Arrange
            var suppliers = new List<Supplier>()
            {
                new()
                {
                    SupplierId = 1, Name = "First", City = "CityFirst"
                },
                new()
                {
                    SupplierId = 2, Name = "Second", City = "CityFirst"
                },
                new()
                {
                    SupplierId = 3, Name = "Third", City = "CityFirst"
                },
            };
            _mediator.Setup(m => m.Send(It.IsAny<GetSuppliersQuery>(), default))
                .ReturnsAsync(suppliers);

            //Act
            var response = (_controller.GetSuppliers().Result as OkObjectResult)!;
            var result = response.Value as List<Supplier>;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<List<Supplier>>();
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(suppliers);
        }

        [Fact]
        public void GetSuppliers_WhenCalled_ReturnNotFound()
        {
            //Arrange
            var suppliers = new List<Supplier>();
            _mediator.Setup(m => m.Send(It.IsAny<GetSuppliersQuery>(), default))
                .ReturnsAsync(suppliers);


            //Act
            var response = _controller.GetSuppliers().Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetSuppliers_WhenException_ReturnProblem()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetSuppliersQuery>(), default))
                .Throws(new Exception("Test Exception"));

            //Act
            var response = (_controller.GetSuppliers().Result as ObjectResult)!;
            var result = response.Value as ProblemDetails;

            //Assert
            result.Should().BeOfType<ProblemDetails>();
            result.Should().Match<ProblemDetails>(r => r.Status == StatusCodes.Status500InternalServerError
                                                  && r.Detail == "Test Exception");
        }

        #endregion

        #region GetSupplier

        [Fact]
        public void GetSupplier_WhenCalled_ReturnOk()
        {
            //Arrange
            Supplier supplier = new()
            {
                SupplierId = 1,
                Name = "First",
                City = "CityFirst"
            };
            _mediator.Setup(m => m.Send(It.IsAny<GetSupplierByIdQuery>(), default))
                .ReturnsAsync(supplier);

            //Act
            var response = (_controller.GetSupplier(1).Result as OkObjectResult)!;
            var result = response.Value as Supplier;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<Supplier>();
            result.Should().BeEquivalentTo(supplier);
        }

        [Fact]
        public void GetSupplier_WhenCalled_ReturnNotFound()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetSupplierByIdQuery>(), default))
                .ReturnsAsync((Supplier)null!);

            //Act
            var response = _controller.GetSupplier(1).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetSupplier_WhenException_ReturnProblem()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetSupplierByIdQuery>(), default))
                .Throws(new Exception("Test Exception"));

            //Act
            var response = (_controller.GetSupplier(1).Result as ObjectResult)!;
            var result = response.Value as ProblemDetails;

            //Assert
            result.Should().BeOfType<ProblemDetails>();
            result.Should().Match<ProblemDetails>(r => r.Status == StatusCodes.Status500InternalServerError
                                                  && r.Detail == "Test Exception");
        }

        #endregion

        #region CreateSupplier

        [Fact]
        public void CreateSupplier_WhenCalled_ReturnOk()
        {
            //Arrange
            CreateSupplierRequest createSupplier = new("First", "CityFirst");

            _mediator.Setup(m => m.Send(It.IsAny<CreateSupplierCommand>(), default))
                .ReturnsAsync(new Supplier
                {
                    SupplierId = 1,
                    Name = "First",
                    City = "CityFirst"
                });

            //Act
            var response = _controller.CreateSupplier(createSupplier).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Supplier>();
        }

        [Fact]
        public void CreateSupplier_WhenException_ReturnProblem()
        {
            //Arrange
            CreateSupplierRequest createSupplier = new("First", "CityFirst");

            _mediator.Setup(m => m.Send(It.IsAny<CreateSupplierCommand>(), default))
                .Throws(new Exception("Test Exception"));

            //Act
            var response = (_controller.CreateSupplier(createSupplier).Result as ObjectResult)!;
            var result = response.Value as ProblemDetails;

            //Assert
            result.Should().BeOfType<ProblemDetails>();
            result.Should().Match<ProblemDetails>(r => r.Status == StatusCodes.Status500InternalServerError
                                                  && r.Detail == "Test Exception");
        }

        #endregion

        #region UpdateSupplier

        [Fact]
        public void UpdateSupplier_WhenCalled_ReturnOk()
        {
            //Arrange
            UpdateSupplierRequest updateSupplier = new(1, "First", "CityFirst");

            _mediator.Setup(m => m.Send(It.IsAny<UpdateSupplierCommand>(), default))
                .ReturnsAsync(new Supplier
                {
                    SupplierId = 1,
                    Name = "First",
                    City = "CityFirst"
                });

            //Act
            var response = _controller.UpdateSupplier(updateSupplier).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Supplier>();
        }

        [Fact]
        public void UpdateSupplier_WhenCalled_ReturnNotFound()
        {
            //Arrange
            UpdateSupplierRequest updateSupplier = new(1, "First", "CityFirst");

            _mediator.Setup(m => m.Send(It.IsAny<UpdateSupplierCommand>(), default))
                .ReturnsAsync((Supplier)null!);

            //Act
            var response = _controller.UpdateSupplier(updateSupplier).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void UpdateSupplier_WhenException_ReturnProblem()
        {
            //Arrange
            UpdateSupplierRequest updateSupplier = new(1, "First", "CityFirst");

            _mediator.Setup(m => m.Send(It.IsAny<UpdateSupplierCommand>(), default))
                .Throws(new Exception("Test Exception"));

            //Act
            var response = (_controller.UpdateSupplier(updateSupplier).Result as ObjectResult)!;
            var result = response.Value as ProblemDetails;

            //Assert
            result.Should().BeOfType<ProblemDetails>();
            result.Should().Match<ProblemDetails>(r => r.Status == StatusCodes.Status500InternalServerError
                                                  && r.Detail == "Test Exception");
        }

        #endregion

        #region DeleteSupplier

        [Fact]
        public void DeleteSupplier_WhenCalled_ReturnOk()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteSupplierCommand>(), default))
                .ReturnsAsync(new Supplier
                {
                    SupplierId = 1,
                    Name = "First",
                    City = "CityFirst"
                });

            //Act
            var response = _controller.DeleteSupplier(1).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Supplier>();
        }

        [Fact]
        public void DeleteSupplier_WhenCalled_ReturnNotFound()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteSupplierCommand>(), default))
                .ReturnsAsync((Supplier)null!);

            //Act
            var response = _controller.DeleteSupplier(1).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void DeleteSupplier_WhenException_ReturnProblem()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteSupplierCommand>(), default))
                .Throws(new Exception("Test Exception"));

            //Act
            var response = (_controller.DeleteSupplier(1).Result as ObjectResult)!;
            var result = response.Value as ProblemDetails;

            //Assert
            result.Should().BeOfType<ProblemDetails>();
            result.Should().Match<ProblemDetails>(r => r.Status == StatusCodes.Status500InternalServerError
                                                  && r.Detail == "Test Exception");
        }

        #endregion
    }
}