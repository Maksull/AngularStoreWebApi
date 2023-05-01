using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Mediator.Commands.Images
{
    public sealed record UploadFileCommand(IFormFile File) : IRequest<IFormFile?>;
}
