namespace UrlShorter.Data.Entities;

/// <summary>
/// Базовый класс для определения сущностей
/// </summary>
/// <typeparam name="TId">Тип ID</typeparam>
public abstract class BaseEntity<TId> where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// Уникальный ID сущности
    /// </summary>
    public TId Id { get; set; }
    
    /// <summary>
    /// Дата и время создания сущности в системе
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}