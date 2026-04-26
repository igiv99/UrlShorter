using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShorter.Contracts.Abstractions;
using UrlShorter.Contracts.Dto;
using UrlShorter.Data;
using UrlShorter.Filters.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UrlShorterContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddScoped<IShortUrlStorage, DbShortUrlStorage>();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapPost("/create", (CreateShortUrlRequest request, [FromServices] IShortUrlStorage storage) =>
{
    if (storage.TrySaveUrl(request, out var shortUrl))
    {
        return Results.Ok(shortUrl);
    }
    
    return Results.BadRequest();
});

app.MapGet("/find", ([FromQuery] Guid id, [FromServices] IShortUrlStorage storage) =>
{
    if (storage.TryGetShotUrlInfo(id, out var shortUrlInfo))
    {
        return Results.Ok(shortUrlInfo);
    }

    return Results.BadRequest();
});

app.MapGet("/redirect/{*shortUrl}", (string shortUrl, [FromServices] IShortUrlStorage storage) =>
{
    if (storage.TryGetDirectUrl(shortUrl, out var directUrl))
    {
        return Results.Redirect(directUrl, permanent: true);
    }
    
    return Results.BadRequest();
    
}).AddEndpointFilter<ShortUrlRequestValidationFilter>();

app.Run();
