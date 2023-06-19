using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Categories;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Categories
{
    public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Category?>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMetrics _metrics;

        public DeleteCategoryHandler(ICategoryService categoryService, IMetrics metrics)
        {
            _categoryService = categoryService;
            _metrics = metrics;
        }

        public async Task<Category?> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.DeleteCategory(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.DeleteCategoryCounter);

            return category;
        }
    }
}
