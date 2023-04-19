using Amazon.S3.Model;
using Core.Mediator.Queries.Images;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Images
{
    public sealed class GetFileHandler : IRequestHandler<GetFileQuery, GetObjectResponse?>
    {
        private readonly IImageService _imageService;

        public GetFileHandler(IImageService imageService)
        {
            _imageService = imageService;
        }

        public Task<GetObjectResponse?> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            return _imageService.GetFile(request.Key);
        }
    }
}
