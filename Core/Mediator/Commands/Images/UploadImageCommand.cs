using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Mediator.Commands.Images
{
    public sealed record UploadImageCommand(IFormFile File) : IRequest<IFormFile?>;
}
