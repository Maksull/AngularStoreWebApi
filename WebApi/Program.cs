using Amazon;
using Dependencies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AngularStore API",
        Version = "v1",
        Description = "An API for AngularStore application",
        Contact = new OpenApiContact
        {
            Name = "MyName",
            Url = new Uri("https://twitter.com/"),
        },
    });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Please enter token",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(opts => opts.AddPolicy("StoreOrigins", policy =>
{
    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
}));

builder.Services.ConfigureDI(builder.Configuration, builder.Environment);
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

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


builder.Host.UseSerilog((context, configuration) =>
{
    configuration
       .ReadFrom.Configuration(builder.Configuration)
       .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticsearchConfiguration:Uri"]!))
       {
           IndexFormat = $"angular-store-logs-{builder.Environment.EnvironmentName}-{DateTime.UtcNow:yyyy-MM}",
           AutoRegisterTemplate = true,
           NumberOfShards = 2,
           NumberOfReplicas = 1,
       });
});

builder.Configuration.AddSecretsManager(region: RegionEndpoint.EUNorth1,
    configurator: options =>
    {
        options.SecretFilter = entry => entry.Name.StartsWith($"{builder.Environment.EnvironmentName}_WebApi_");
        options.KeyGenerator = (_, s) => s
            .Replace($"{builder.Environment.EnvironmentName}_WebApi_", string.Empty)
            .Replace("__", ":");
        options.PollingInterval = TimeSpan.FromSeconds(10);
    });

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

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

#endregion

app.MigrateDb();

app.Run();