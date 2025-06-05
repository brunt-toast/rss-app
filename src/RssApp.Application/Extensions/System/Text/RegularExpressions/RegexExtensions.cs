using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace RssApp.Application.Extensions.System.Text.RegularExpressions;

internal static class RegexExtensions
{
    public static bool TryParse(string expr, [NotNullWhen(true)] out Regex? result)
    {
        try
        {
            result = new Regex(expr);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
}
