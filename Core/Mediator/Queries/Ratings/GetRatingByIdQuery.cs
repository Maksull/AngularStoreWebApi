﻿using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Ratings
{
    public sealed record GetRatingByIdQuery(Guid Id) : IRequest<Rating>;
}
