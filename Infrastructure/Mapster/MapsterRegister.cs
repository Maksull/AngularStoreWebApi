using Core.Contracts.Controllers.Auth;
using Core.Contracts.Controllers.Categories;
using Core.Contracts.Controllers.Orders;
using Core.Contracts.Controllers.Products;
using Core.Contracts.Controllers.Suppliers;
using Core.Entities;
using Mapster;

namespace Infrastructure.Mapster
{
    public sealed class MapsterRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<CreateProductRequest, Product>()
                .Map(d => d.Images, s => s.Name + "/" + s.Img.FileName);
            config.ForType<UpdateProductRequest, Product>()
                .Map(d => d.Images, s => s.Img == null ? "" : s.Name + "/" + s.Img.FileName);


            config.ForType<CreateCategoryRequest, Category>();
            config.ForType<UpdateCategoryRequest, Category>();


            config.ForType<CreateSupplierRequest, Supplier>();
            config.ForType<UpdateSupplierRequest, Supplier>();


            config.ForType<CreateOrderRequest, Order>();
            config.ForType<UpdateOrderRequest, Order>();


            config.ForType<CreateCartLineRequest, CartLine>();
            config.ForType<UpdateCartLineRequest, CartLine>();


            config.ForType<RegisterRequest, User>()
                .Map(d => d.UserName, s => s.Username);

            config.ForType<RefreshTokenRequest, RefreshToken>();

            config.ForType<User, UserResponse>()
                .Map(d => d.Username, s => s.UserName);

        }
    }
}
