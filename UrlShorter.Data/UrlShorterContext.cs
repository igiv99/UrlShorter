using Microsoft.EntityFrameworkCore;
using UrlShorter.Data.Entities;

namespace UrlShorter.Data;

/// <summary>
/// Конекст БД приложения
/// </summary>
public class UrlShorterContext(DbContextOptions<UrlShorterContext> options) : DbContext(options)
{
    public DbSet<ShortUrl> ShortUrls { get; set; }
}