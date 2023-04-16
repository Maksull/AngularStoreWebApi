using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
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
        public static void ConfigureDI(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.ConfigureDbContext(configuration);
            services.ConfigureUnitOfWork();
            services.ConfigureServices();
            services.ConfigureAwsS3Bucket(configuration, environment);
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
        private static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IImageService, ImageService>();


            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IS3Service, S3Service>();
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
