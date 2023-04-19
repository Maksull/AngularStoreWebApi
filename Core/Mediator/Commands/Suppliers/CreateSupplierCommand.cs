using Core.Contracts.Controllers.Suppliers;
using Core.Entities;
using MediatR;

namespace Core.Mediator.Commands.Suppliers
{
    public sealed record CreateSupplierCommand(CreateSupplierRequest Supplier) : IRequest<Supplier>;
}
