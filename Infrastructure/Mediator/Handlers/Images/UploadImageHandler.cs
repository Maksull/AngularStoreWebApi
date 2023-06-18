using App.Metrics;
using Core.Mediator.Commands.Images;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Mediator.Handlers.Images
{
    public sealed class UploadImageHandler : IRequestHandler<UploadImageCommand, IFormFile?>
    {
        private readonly IImageService _imageService;
        private readonly IMetrics _metrics;

        public UploadImageHandler(IImageService imageService, IMetrics metrics)
        {
            _imageService = imageService;
            _metrics = metrics;
        }

        public async Task<IFormFile?> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _imageService.UploadImage(request.File, request.File.FileName);

            _metrics.Measure.Counter.Increment(MetricsRegistry.UploadImageCounter);

            return image;
        }
    }
}
