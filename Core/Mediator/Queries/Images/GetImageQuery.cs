using Amazon.S3.Model;
using MediatR;

namespace Core.Mediator.Queries.Images
{
    public sealed record GetImageQuery(string Key) : IRequest<GetObjectResponse?>;
}
