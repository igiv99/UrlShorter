namespace UrlShorter.Data.Entities;

/// <summary>
/// Сущность короткой ссылки
/// </summary>
public class ShortUrl : BaseEntity<Guid>
{
    /// <summary>
    /// Значение короткой ссылки
    /// </summary>
    public required string ShortUrlValue { get; set; }
    
    /// <summary>
    /// Оригинальная ссылка
    /// </summary>
    public required string DirectUrlValue { get; set; }
    
    /// <summary>
    /// Дата и время истечения действия короткой ссылки
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}