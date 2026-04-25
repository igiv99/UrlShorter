namespace UrlShorter.Services;

/// <summary>
/// Генератор коротких ссылок
/// </summary>
public static class ShortUrlGenerator
{
    /// <summary>
    /// Создаёт короткую ссылку на основе пользовательского паттерна
    /// </summary>
    /// <param name="shortUrlPattern"></param>
    /// <returns>Короткая ссылка в виде строки</returns>
    public static string Generate(string shortUrlPattern)
    {
        var token = Guid.NewGuid();
        return $"{shortUrlPattern}{token}";
    }
}