using System.Collections.Concurrent;
using UrlShorter.Data.Dto;
using UrlShorter.Services;

namespace UrlShorter.Data;

/// <summary>
/// Хранилище коротких ссылок
/// </summary>
public class ShortUrlStorage
{
    private readonly ConcurrentDictionary<string, string> _storage = new();

    /// <summary>
    /// Получает оригинальную ссылку по короткой
    /// </summary>
    /// <param name="shortUrl">Короткая ссылка</param>
    /// <param name="directUrl">Оригинальная ссылка</param>
    /// <returns>True - если ссылка найдена, false - если ссылку не удалось найти или она была пустой строкой</returns>
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

    /// <summary>
    /// Сохраняет оригинальную ссылку
    /// </summary>
    /// <param name="urlDto"><see cref="NewShortUrlDto"/></param>
    /// <param name="shortUrl">Связанная короткая ссылка</param>
    /// <returns>True - если ссылка сохранена, false - ошибка при сохранении ссылки</returns>
    public bool TrySaveUrl(NewShortUrlDto urlDto, out string shortUrl)
    {
        shortUrl = ShortUrlGenerator.Generate(urlDto.ShortUrlPattern);
        return _storage.TryAdd(shortUrl, urlDto.DirectUrl);
    }
}