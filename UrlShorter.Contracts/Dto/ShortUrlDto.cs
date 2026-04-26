namespace UrlShorter.Contracts.Dto;

/// <summary>
/// DTO для чтения короткой ссылки
/// </summary>
/// <param name="ShortUrl">Значение короткой ссылки</param>
/// <param name="DirectUrl">Оригинальная ссылка</param>
/// <param name="ExpiresAt">Дата и время истечения срока действия короткой ссылки (null - бессрочное действие)</param>
public record ShortUrlDto(string ShortUrl, string DirectUrl, DateTime? ExpiresAt);