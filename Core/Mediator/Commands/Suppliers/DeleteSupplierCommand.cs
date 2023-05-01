using Core.Entities;
using MediatR;

namespace Core.Mediator.Commands.Suppliers
{
    public sealed record DeleteSupplierCommand(long Id) : IRequest<Supplier?>;
}
