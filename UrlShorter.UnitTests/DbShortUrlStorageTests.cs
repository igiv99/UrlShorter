using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using Moq.EntityFrameworkCore;
using UrlShorter.Contracts.Dto;
using UrlShorter.Contracts.Models;
using UrlShorter.Data;
using UrlShorter.Data.Entities;
using UrlShorter.Infrastructure;

namespace UrlShorter.UnitTests;

/// <summary>
/// Тесты для <see cref="DbShortUrlStorage"/>
/// </summary>
public class DbShortUrlStorageTests
{
    private const uint TermUnits = 3;
    private const string ExpiresAt = "2026-04-25";
    private const string UserShortUrlPattern = "images/";
    private const string DirectUrl =
        "https://primaryleap.co.uk/images/wikileap/post_levels/thumb_large/32d419f896ca782505b2d256a1ee902db75a5ba7iY.jpg";
    
    private readonly FakeLogger<DbShortUrlStorage> _logger = new();
    
    [Theory(DisplayName = "Проверяет успешное сохранение ссылки")]
    [InlineData(TermType.Custom, null, ExpiresAt)]
    [InlineData(TermType.Hourly, TermUnits, null)]
    [InlineData(TermType.Daily, TermUnits, null)]
    [InlineData(TermType.Monthly, TermUnits, null)]
    [InlineData(null, null, null)]
    public void SaveUrl_Success(TermType? termType, uint? termUnits, string? expiredDate)
    {
        ExpirationInfo? expirationInfo = null;
        if (termType != null)
        {
            DateTime.TryParse(expiredDate, out var customDateTime);
            expirationInfo = new ExpirationInfo(termType.Value, termUnits, customDateTime);
        }
        
        var request = new CreateShortUrlRequest(
            DirectUrl,
            UserShortUrlPattern,
            expirationInfo);
        
        var contextMock = CreateMockDbContext(new List<ShortUrl>());
        
        var storage = new DbShortUrlStorage(contextMock.Object, _logger);
        
        var result = storage.TrySaveUrl(request, out var response);
        
        Assert.True(result);
        Assert.NotNull(response);
        
        Assert.StartsWith(UserShortUrlPattern, response.ShortUrl);
    }
    
    [Theory(DisplayName = "Проверяет, что ссылка недоступна после истечения срока")]
    [InlineData(TermType.Custom, null, ExpiresAt)]
    [InlineData(TermType.Hourly, TermUnits, null)]
    [InlineData(TermType.Daily, TermUnits, null)]
    [InlineData(TermType.Monthly, TermUnits, null)]
    public void FailedReadShortUrl_UrlExpired(TermType termType, uint? termUnits, string? expiredDate)
    {
        DateTime.TryParse(expiredDate, out var customDateTime);
        var expirationInfo = new ExpirationInfo(termType, termUnits, customDateTime);
        
        var shortUrl = ShortUrlGenerator.Generate(UserShortUrlPattern);
        
        var expiresAt = expirationInfo.GetExpirationDate();
        
        var contextMock = CreateMockDbContext(new List<ShortUrl>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                ShortUrlValue = shortUrl,
                DirectUrlValue = DirectUrl,
                ExpiresAt = termType switch
                {
                    TermType.Custom => customDateTime,
                    TermType.Hourly => expiresAt.AddHours(-Convert.ToDouble(TermUnits)),
                    TermType.Daily => expiresAt.AddDays(-Convert.ToDouble(TermUnits)),
                    TermType.Monthly => expiresAt.AddMonths(-Convert.ToInt32(TermUnits)),
                    _ => throw new ArgumentOutOfRangeException(nameof(termType), termType, null)
                }
            }
        });
        
        var storage = new DbShortUrlStorage(contextMock.Object, _logger);
        
        var result = storage.TryGetDirectUrl(shortUrl, out var response);
        
        Assert.False(result);
        Assert.True(string.IsNullOrEmpty(response));
    }

    private Mock<UrlShorterContext> CreateMockDbContext(IList<ShortUrl> shortUrls)
    {
        var options = new DbContextOptionsBuilder<UrlShorterContext>().Options;
        var mockContext = new Mock<UrlShorterContext>(options);
        
        mockContext.Setup(x => x.ShortUrls).ReturnsDbSet(shortUrls);
        return mockContext;
    }
}