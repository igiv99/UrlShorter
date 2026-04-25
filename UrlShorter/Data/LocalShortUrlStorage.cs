using System.Collections.Concurrent;
using UrlShorter.Data.Dto;
using UrlShorter.Services;

namespace UrlShorter.Data;

/// <summary>
/// Хранилище коротких ссылок
/// </summary>
public class LocalShortUrlStorage : IShortUrlStorage
{
    private readonly ConcurrentDictionary<string, string> _storage = new();

    /// <inheritdoc/>
    public bool TryGetDirectUrl(string shortUrl, out string directUrl)
    {
        //гарантируем, что ссылка не будет null
        directUrl = string.Empty;
        
        if (!_storage.TryGetValue(shortUrl, out directUrl!))
        {
            return false;
        }
        
        return !string.IsNullOrEmpty(directUrl);
    }

    /// <inheritdoc/>
    public bool TrySaveUrl(NewShortUrlDto urlDto, out string shortUrl)
    {
        shortUrl = ShortUrlGenerator.Generate(urlDto.ShortUrlPattern);
        return _storage.TryAdd(shortUrl, urlDto.DirectUrl);
    }
}