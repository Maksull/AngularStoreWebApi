using AutoMapper;
using WebApi.Models;
using WebApi.Models.Dto;

namespace WebApi.AutoMapper
{
    public sealed class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDto, Product>().ConvertUsing<ProductDtoConverter>();
        }
    }
}
