using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Categories;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Categories
{
    public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMetrics _metrics;

        public GetCategoryByIdHandler(ICategoryService categoryService, IMetrics metrics)
        {
            _categoryService = categoryService;
            _metrics = metrics;
        }

        public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategory(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetCategoryByIdCounter);

            return category;
        }
    }
}
