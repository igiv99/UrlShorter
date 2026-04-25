namespace UrlShorter.Data.Dto;

/// <summary>
/// DTO для создания новой короткой ссылки
/// </summary>
/// <param name="DirectUrl">Оригинальная ссылка</param>
/// <param name="ShortUrlPattern">Пользовательский шаблон для короткой ссылки</param>
public record NewShortUrlDto(string DirectUrl, string ShortUrlPattern);