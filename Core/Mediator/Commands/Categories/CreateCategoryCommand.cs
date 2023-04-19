using Core.Contracts.Controllers.Categories;
using Core.Entities;
using MediatR;

namespace Core.Mediator.Commands.Categories
{
    public sealed record CreateCategoryCommand(CreateCategoryRequest Category) : IRequest<Category>;
}
