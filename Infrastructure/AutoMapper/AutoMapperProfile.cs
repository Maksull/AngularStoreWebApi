using AutoMapper;
using Core.Dto;
using Core.Entities;

namespace Infrastructure.AutoMapper
{
    public sealed class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDto, Product>().ConvertUsing<ProductDtoConverter>();
        }
    }
}
