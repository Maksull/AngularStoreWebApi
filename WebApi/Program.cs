using Dependencies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddCors(opts => opts.AddPolicy("StoreOrigins", policy =>
{
    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
}));

builder.Services.ConfigureDI(builder.Configuration, builder.Environment);

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecurityKey"]!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.Configure<MvcNewtonsoftJsonOptions>(opts =>
{
    opts.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

#endregion

builder.Services.AddCors(opts => opts.AddPolicy("StoreOrigins", policy =>
{
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));

builder.Services.AddDbContext<ApiDataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:Store"]!);
});
builder.Services.AddDbContext<IdentityDataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:Identity"]!);
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityDataContext>();


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IS3Service, S3Service>();


builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecurityKey"]!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.Configure<MvcNewtonsoftJsonOptions>(opts =>
{
    opts.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

if (builder.Environment.IsProduction())
{
    builder.Services.AddAWSService<IAmazonS3>(new AWSOptions
    {
        Credentials = new BasicAWSCredentials(builder.Configuration["AWS:AccessKeyId"], builder.Configuration["AWS:SecretAccessKey"])
    });
}

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
    builder.Services.AddAWSService<IAmazonS3>();
}


#endregion

var app = builder.Build();

#region App

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("StoreOrigins");

app.MapControllers();

#endregion

app.MigrateDb();

app.Run();
