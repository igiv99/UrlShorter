namespace UrlShorter.Contracts.Models;

/// <summary>
/// Сведения о сроке истечения действия короткой ссылки
/// </summary>
public record ExpirationInfo
{
    private readonly TermType _expirationType;
    private readonly uint? _termUnits;
    private readonly DateTime? _customDateTime;

    public ExpirationInfo(TermType expirationType, uint? termUnits, DateTime? customDateTime)
    {
        _expirationType = expirationType;
        //флоу выбора даты и времени пользователем
        if (!termUnits.HasValue && _expirationType == TermType.Custom)
        {
            _customDateTime = customDateTime ??
                             throw new ArgumentException("Custom user date and time must be specified", nameof(customDateTime));
        }
        //флоу с фиксированными сроками
        if (termUnits.HasValue)
        {
            _termUnits = termUnits;
        }
    }

    /// <summary>
    /// Дата истечения срока действия ссылки
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public DateTime ExpirationDate
    {
        get
        {
            var dateTimeNow = DateTime.UtcNow;
            return _expirationType switch
            {
                TermType.Custom => _customDateTime!.Value,
                TermType.Hourly => dateTimeNow.AddHours(Convert.ToDouble(_termUnits)),
                TermType.Daily => dateTimeNow.AddDays(Convert.ToDouble(_termUnits)),
                TermType.Monthly => dateTimeNow.AddMonths(Convert.ToInt32(_termUnits)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

public enum TermType
{
    Hourly,
    Daily,
    Monthly,
    Custom
}