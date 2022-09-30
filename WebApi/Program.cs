using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Database;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiDataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:Store"]!);
});
builder.Services.AddDbContext<IdentityDataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:Identity"]!);
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityDataContext>();

#endregion

var app = builder.Build();

#region App

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

ApiSeedData.EnsurePopulated(app);
IdentitySeedData.EnsurePopulated(app);

app.Run();
