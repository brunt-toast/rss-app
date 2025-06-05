using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace RssApp.Native.Windows.Extensions.Microsoft.UI.Xaml.Controls;

internal static class ContentDialogExtensions
{
    public static async Task QueueShowAsync(this ContentDialog source, int retryIntervalMs = 100, CancellationToken cancellationToken = new())
    {
        while (true)
        {
            try
            {
                await source.ShowAsync();
                return;
            }
            catch (COMException)
            {
                await Task.Delay(retryIntervalMs, cancellationToken);
            }
        }
    }
}
