using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShorter.Data.Entities;

namespace UrlShorter.Data.Configurations;

/// <summary>
/// Конфигурация для <see cref="ShortUrl"/>
/// </summary>
public class ShortUrlConfiguration : IEntityTypeConfiguration<ShortUrl>
{
    public void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ShortUrlValue)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.ShortUrlValue);
        
        builder.Property(x => x.DirectUrlValue)
            .IsRequired()
            .HasMaxLength(300);
    }
}