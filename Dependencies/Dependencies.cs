using Amazon.S3;
using Core.Entities;
using Core.Validators.Products;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Mapster;
using Infrastructure.Mediator.Handlers.Products;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dependencies
{
    public static class Dependencies
    {
        public static IServiceCollection ConfigureDI(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.ConfigureDbContext(configuration);
            services.ConfigureUnitOfWork();
            services.ConfigureServices();
            services.ConfigureFluentValidation();
            services.ConfigureMapster();
            services.ConfigureMediatR();
            services.ConfigureAwsS3Bucket(configuration, environment);
            services.ConfigureRedisCache(configuration);

            return services;
        }

        public static WebApplication MigrateDb(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();

                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                        var user = userManager.FindByNameAsync("Admin").Result!;

                        var result = userManager.AddToRoleAsync(user, "Admin").Result;

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return app;
        }


        private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApiDataContext>(opts =>
            {
                opts.UseSqlServer(configuration["ConnectionStrings:Store"]!);
            });

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApiDataContext>();

        }

        private static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProductRepository, ProductRepository>()
                .AddScoped(provider => new Lazy<IProductRepository>(() => provider.GetRequiredService<IProductRepository>()));
            services.AddScoped<ICategoryRepository, CategoryRepository>()
                .AddScoped(provider => new Lazy<ICategoryRepository>(() => provider.GetRequiredService<ICategoryRepository>()));
            services.AddScoped<ISupplierRepository, SupplierRepository>()
                .AddScoped(provider => new Lazy<ISupplierRepository>(() => provider.GetRequiredService<ISupplierRepository>()));
            services.AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped(provider => new Lazy<IOrderRepository>(() => provider.GetRequiredService<IOrderRepository>()));
            services.AddScoped<IRatingRepository, RatingRepository>()
                .AddScoped(provider => new Lazy<IRatingRepository>(() => provider.GetRequiredService<IRatingRepository>()));

        }

        private static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IRatingService, RatingService>();

            services.AddScoped<ICacheService, CacheService>();

            services.AddScoped<IEmailService, EmailService>();
        }

        private static void ConfigureFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(CreateProductRequestValidator).Assembly);

            services.AddFluentValidationAutoValidation();
        }

        private static void ConfigureMapster(this IServiceCollection services)
        {
            TypeAdapterConfig config = new();
            config.Apply(new MapsterRegister());
            services.AddSingleton(config);

            services.AddSingleton<IMapper>(sp =>
            {
                return new ServiceMapper(sp, config);
            });
        }

        private static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetProductsHandler>());
        }

        private static void ConfigureAwsS3Bucket(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddAWSService<IAmazonS3>();
        }

        private static void ConfigureRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(opts =>
            {
                opts.Configuration = configuration.GetConnectionString("RedisCache");
            });
        }
    }
}
