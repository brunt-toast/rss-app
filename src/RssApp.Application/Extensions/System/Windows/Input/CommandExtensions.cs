using System.Windows.Input;

namespace RssApp.Application.Extensions.System.Windows.Input;

internal static class CommandExtensions
{
    public static void Execute(this ICommand command)
    {
        command.Execute(null);
    }
}
