namespace UrlShorter.Contracts.Models;

public record ExpirationInfo
{
    public TermType ExpirationType { get; init; }
    public uint? TermUnits { get; init; }
    public DateTime? CustomDateTime { get; init; }

    public ExpirationInfo(TermType expirationType, uint? termUnits, DateTime? customDateTime)
    {
        ExpirationType = expirationType;
        //флоу выбора даты и времени пользователем
        if (!termUnits.HasValue && ExpirationType == TermType.Custom)
        {
            CustomDateTime = customDateTime ??
                             throw new ArgumentException("Custom user date and time must be specified", nameof(customDateTime));
        }
        //флоу с фиксированными сроками
        if (termUnits.HasValue)
        {
            TermUnits = termUnits;
        }
    }
    
    public DateTime GetExpirationDate()
    {
        var dateTimeNow = DateTime.UtcNow;
        return ExpirationType switch
        {
            TermType.Custom => CustomDateTime!.Value,
            TermType.Hourly => dateTimeNow.AddHours(Convert.ToDouble(TermUnits)),
            TermType.Daily => dateTimeNow.AddDays(Convert.ToDouble(TermUnits)),
            TermType.Monthly => dateTimeNow.AddMonths(Convert.ToInt32(TermUnits)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum TermType
{
    Hourly,
    Daily,
    Monthly,
    Custom
}