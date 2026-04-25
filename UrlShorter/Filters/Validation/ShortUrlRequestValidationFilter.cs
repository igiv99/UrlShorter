using System.Text.RegularExpressions;

namespace UrlShorter.Filters.Validation;

/// <summary>
/// Фильтр для валидации короткой ссылки
/// </summary>
partial class ShortUrlRequestValidationFilter : IEndpointFilter
{
    [GeneratedRegex("^[a-z]*/")]
    private static partial Regex ShortUrlRegex();
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var shortUrl = context.GetArgument<string>(0);
        
        var validationErrorDsc = string.Empty;
        if (string.IsNullOrEmpty(shortUrl))
        {
            validationErrorDsc = "Empty shortUrl";
            
        } else if (!ShortUrlRegex().IsMatch(shortUrl))
        {
            validationErrorDsc = "Invalid shortUrl format";
        }

        if (!string.IsNullOrEmpty(validationErrorDsc))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["shortUrl"] = [validationErrorDsc]
            });
        }
        
        return await next(context);
    }
}