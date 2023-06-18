using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Categories;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Categories
{
    public sealed class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMetrics _metrics;

        public GetCategoriesHandler(ICategoryService categoryService, IMetrics metrics)
        {
            _categoryService = categoryService;
            _metrics = metrics;
        }

        public Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = _categoryService.GetCategories();

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetCategoriesCounter);

            return Task.FromResult(categories);
        }
    }
}
