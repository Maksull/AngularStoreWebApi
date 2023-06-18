using Amazon.S3.Model;
using App.Metrics;
using Core.Mediator.Queries.Images;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Images
{
    public sealed class GetImageHandler : IRequestHandler<GetImageQuery, GetObjectResponse?>
    {
        private readonly IImageService _imageService;
        private readonly IMetrics _metrics;

        public GetImageHandler(IImageService imageService, IMetrics metrics)
        {
            _imageService = imageService;
            _metrics = metrics;
        }

        public async Task<GetObjectResponse?> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            var image = await _imageService.GetImage(request.Key);

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetImageCounter);

            return image;
        }
    }
}
