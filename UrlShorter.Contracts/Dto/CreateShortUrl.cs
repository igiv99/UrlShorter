namespace UrlShorter.Contracts.Dto;

/// <summary>
/// Request для создания новой короткой ссылки
/// </summary>
/// <param name="DirectUrl">Оригинальная ссылка</param>
/// <param name="ShortUrlPattern">Пользовательский шаблон для короткой ссылки</param>
public record CreateShortUrlRequest(string DirectUrl, string ShortUrlPattern);

/// <summary>
/// Response создания короткой ссылки
/// </summary>
/// <param name="Id">ID короткой ссылки в системе</param>
/// <param name="ShortUrl">Короткая ссылка</param>
public record CreateShortUrlResponse(Guid Id, string ShortUrl);