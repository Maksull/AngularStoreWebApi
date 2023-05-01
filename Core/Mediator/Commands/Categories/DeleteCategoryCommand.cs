using Core.Entities;
using MediatR;

namespace Core.Mediator.Commands.Categories
{
    public sealed record DeleteCategoryCommand(long Id) : IRequest<Category?>;
}
