using RssApp.Application.Extensions.System.Text.RegularExpressions;
using RssApp.Application.Models.FeedFilter;

namespace RssApp.Application.Validations.FeedFilter;

internal class FeedFilterModelValidator : IValidator<FeedFilterModel>
{
    public static Task<(bool Success, string Message)> ValidateAsync(IServiceProvider services, FeedFilterModel item)
    {
        if (item.AppliesTo == 0)
        {
            return Task.FromResult((false, "The filter must apply to at least one property of the feed entry"));
        }

        if (!RegexExtensions.TryParse(item.FilterRegex, out _))
        {
            return Task.FromResult((false, "The given filter is not a valid regular expression"));
        }

        return Task.FromResult((true, ""));
    }
}
