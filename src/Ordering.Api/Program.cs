using Microsoft.EntityFrameworkCore;
using Ordering.Api.Application.Endpoints;
using Ordering.Api.Core.Interfaces;
using Ordering.Api.Infrastructure.Data;
using Ordering.Api.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<OrderingDbContext>(options => options
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapOrderEndpoints();

app.Run();