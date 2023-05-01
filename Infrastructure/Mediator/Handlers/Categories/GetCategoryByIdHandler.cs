using Core.Entities;
using Core.Mediator.Queries.Categories;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Categories
{
    public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryByIdHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetCategory(request.Id);
        }
    }
}
