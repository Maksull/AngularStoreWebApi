using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
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
using Microsoft.AspNetCore.Hosting;
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
                    }
                    catch
                    {
                        throw new Exception("Api Migration failed");
                    }
                }
                using (var appContext = scope.ServiceProvider.GetRequiredService<IdentityDataContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();

                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                        var user = userManager.FindByNameAsync("Admin").Result!;

                        var result = userManager.AddToRoleAsync(user, "Admin").Result;

                    }
                    catch
                    {
                        throw new Exception("Identity Migration failed");
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
            services.AddDbContext<IdentityDataContext>(opts =>
            {
                opts.UseSqlServer(configuration["ConnectionStrings:Identity"]!);
            });

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IdentityDataContext>();

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

        }

        private static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IImageService, ImageService>();


            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IS3Service, S3Service>();
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
            if (environment.IsProduction())
            {
                services.AddAWSService<IAmazonS3>(new AWSOptions
                {
                    Credentials = new BasicAWSCredentials(configuration["AWS:AccessKeyId"], configuration["AWS:SecretAccessKey"])
                });
            }
            else
            {
                services.AddDefaultAWSOptions(configuration.GetAWSOptions());
                services.AddAWSService<IAmazonS3>();
            }
        }
    }
}
