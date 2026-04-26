using UrlShorter.Contracts.Abstractions;
using UrlShorter.Contracts.Dto;
using UrlShorter.Data.Entities;
using UrlShorter.Infrastructure;

namespace UrlShorter.Data;

/// <summary>
/// Хранилище коротких ссылок в БД
/// </summary>
public class DbShortUrlStorage : IShortUrlStorage
{
    private readonly UrlShorterContext _context;

    public DbShortUrlStorage(UrlShorterContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public bool TryGetDirectUrl(string shortUrl, out string directUrl)
    {
        var entity = _context.ShortUrls.FirstOrDefault(x => x.ShortUrlValue == shortUrl);

        if (entity == null)
        {
            directUrl = string.Empty;
            return false;
        }
        
        directUrl = entity.DirectUrlValue;
        return !string.IsNullOrEmpty(directUrl);
    }

    /// <inheritdoc/>
    public bool TrySaveUrl(CreateShortUrlRequest urlRequest, out CreateShortUrlResponse? shortUrl)
    {
        var shortUrlValue = ShortUrlGenerator.Generate(urlRequest.ShortUrlPattern);
        var newEntity = new ShortUrl
        {
            Id = Guid.NewGuid(),
            ShortUrlValue = shortUrlValue,
            DirectUrlValue = urlRequest.DirectUrl,
        };

        try
        {
            _context.ShortUrls.Add(newEntity);
        
            _context.SaveChanges();
        }
        catch (Exception)
        {
            shortUrl = null;
            return false;
        }

        shortUrl = new(newEntity.Id, newEntity.ShortUrlValue);
        return true;
    }

    /// <inheritdoc/>
    public bool TryGetShotUrlInfo(Guid id, out ShortUrlDto? shortUrlInfo)
    {
        var entity = _context.Find<ShortUrl>(id);
        if (entity == null)
        {
            shortUrlInfo = null;
            return false;
        }
        
        shortUrlInfo = new ShortUrlDto(entity.ShortUrlValue, entity.DirectUrlValue, entity.ExpiresAt);
        return true;
    }
}