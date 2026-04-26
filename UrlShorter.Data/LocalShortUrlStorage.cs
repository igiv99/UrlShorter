using System.Collections.Concurrent;
using UrlShorter.Contracts.Abstractions;
using UrlShorter.Contracts.Dto;
using UrlShorter.Data.Entities;
using UrlShorter.Infrastructure;

namespace UrlShorter.Data;

/// <summary>
/// Локальное хранилище коротких ссылок
/// </summary>
public class LocalShortUrlStorage : IShortUrlStorage
{
    private readonly ConcurrentDictionary<string, ShortUrl> _storage = new();

    /// <inheritdoc/>
    public bool TryGetDirectUrl(string shortUrl, out string directUrl)
    {
        if (_storage.TryGetValue(shortUrl, out var entity))
        {
            directUrl = entity.DirectUrlValue;
            return !string.IsNullOrEmpty(directUrl);   
        }
        
        directUrl = string.Empty;
        return false;
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
        
        var isSuccess = _storage.TryAdd(shortUrlValue, newEntity);

        if (isSuccess)
        {
            shortUrl = new(newEntity.Id, newEntity.ShortUrlValue);
            return true;
        }
        
        shortUrl = null;
        return false;
    }

    /// <inheritdoc/>
    public bool TryGetShotUrlInfo(Guid id, out ShortUrlDto? shortUrlInfo)
    {
        var entity = _storage.Values.FirstOrDefault(x => x.Id == id);
        if (entity == null)
        {
            shortUrlInfo = null;
            return false;
        }
        
        shortUrlInfo = new ShortUrlDto(entity.ShortUrlValue, entity.DirectUrlValue, entity.ExpiresAt);
        return true;
    }
}