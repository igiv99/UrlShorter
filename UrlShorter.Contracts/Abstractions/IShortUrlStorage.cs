using UrlShorter.Contracts.Dto;

namespace UrlShorter.Contracts.Abstractions;

public interface IShortUrlStorage
{
    /// <summary>
    /// Получает оригинальную ссылку по короткой
    /// </summary>
    /// <param name="shortUrl">Короткая ссылка</param>
    /// <param name="directUrl">Оригинальная ссылка</param>
    /// <returns>True - если ссылка найдена, false - если ссылку не удалось найти или она была пустой строкой</returns>
    public bool TryGetDirectUrl(string shortUrl, out string directUrl);

    /// <summary>
    /// Сохраняет оригинальную ссылку
    /// </summary>
    /// <param name="urlRequest">Запрос на создание короткой ссылки <see cref="CreateShortUrlRequest"/></param>
    /// <param name="shortUrl">Результат операции <see cref="CreateShortUrlResponse"/></param>
    /// <returns>True - если ссылка сохранена, false - ошибка при сохранении ссылки</returns>
    public bool TrySaveUrl(CreateShortUrlRequest urlRequest, out CreateShortUrlResponse? shortUrl);

    /// <summary>
    /// Получает сведения о короткой ссылке
    /// </summary>
    /// <param name="id">ID короткой ссылки в системе</param>
    /// <param name="shortUrlInfo">Сведения о короткой ссылке <see cref="ShortUrlDto"/></param>
    /// <returns>True - если сущность найдена, false - если не удалось найти</returns>
    public bool TryGetShotUrlInfo(Guid id, out ShortUrlDto? shortUrlInfo);
}