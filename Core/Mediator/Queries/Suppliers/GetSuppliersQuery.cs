using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Suppliers
{
    public sealed record GetSuppliersQuery() : IRequest<IEnumerable<Supplier>>;
}
