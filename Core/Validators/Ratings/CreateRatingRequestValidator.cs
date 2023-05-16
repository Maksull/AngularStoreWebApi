﻿using Core.Contracts.Controllers.Ratings;
using FluentValidation;

namespace Core.Validators.Ratings
{
    public sealed class CreateRatingRequestValidator : AbstractValidator<CreateRatingRequest>
    {
        public CreateRatingRequestValidator()
        {
            RuleFor(r => r.ProductId).GreaterThanOrEqualTo(1);
            RuleFor(r => r.Value).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
            RuleFor(r => r.Comment).NotNull();
        }
    }
}
