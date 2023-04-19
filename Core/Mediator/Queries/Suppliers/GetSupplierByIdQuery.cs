using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Suppliers
{
    public sealed record GetSupplierByIdQuery(long Id) : IRequest<Supplier?>;
}
