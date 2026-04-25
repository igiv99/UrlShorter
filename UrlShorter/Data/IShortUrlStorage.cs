using UrlShorter.Data.Dto;

namespace UrlShorter.Data;

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
    /// <param name="urlDto"><see cref="NewShortUrlDto"/></param>
    /// <param name="shortUrl">Связанная короткая ссылка</param>
    /// <returns>True - если ссылка сохранена, false - ошибка при сохранении ссылки</returns>
    public bool TrySaveUrl(NewShortUrlDto urlDto, out string shortUrl);
}