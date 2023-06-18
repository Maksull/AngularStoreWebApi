using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Categories;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Categories
{
    public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Category?>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMetrics _metrics;

        public UpdateCategoryHandler(ICategoryService categoryService, IMetrics metrics)
        {
            _categoryService = categoryService;
            _metrics = metrics;
        }

        public async Task<Category?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.UpdateCategory(request.Category);

            _metrics.Measure.Counter.Increment(MetricsRegistry.UpdateCategoryCounter);

            return category;
        }
    }
}
