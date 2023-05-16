using Amazon.S3.Model;
using Core.Mediator.Queries.Images;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Images
{
    public sealed class GetImageHandler : IRequestHandler<GetImageQuery, GetObjectResponse?>
    {
        private readonly IImageService _imageService;

        public GetImageHandler(IImageService imageService)
        {
            _imageService = imageService;
        }

        public Task<GetObjectResponse?> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            return _imageService.GetImage(request.Key);
        }
    }
}
