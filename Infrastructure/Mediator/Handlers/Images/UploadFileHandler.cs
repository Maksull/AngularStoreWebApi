﻿using Core.Mediator.Commands.Images;
using Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Mediator.Handlers.Images
{
    public sealed class UploadFileHandler : IRequestHandler<UploadImageCommand, IFormFile?>
    {
        private readonly IImageService _imageService;

        public UploadFileHandler(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<IFormFile?> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            return await _imageService.UploadImage(request.File, request.File.FileName);
        }
    }
}
