using UrlShorter.Data;
using UrlShorter.Data.Dto;
using UrlShorter.Filters.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var storage = new ShortUrlStorage();

app.MapPost("/create", (NewShortUrlDto dto) =>
{
    if (!storage.TrySaveUrl(dto, out var shortUrl))
    {
        return Results.BadRequest();
    }
    
    return Results.Ok(shortUrl);
});

app.MapGet("/redirect/{*shortUrl}", (string shortUrl) =>
{
    if (storage.TryGetDirectUrl(shortUrl, out var directUrl))
    {
        return Results.Redirect(directUrl, permanent: true);
    }
    
    return Results.BadRequest();
    
}).AddEndpointFilter<ShortUrlRequestValidationFilter>();

app.Run();
