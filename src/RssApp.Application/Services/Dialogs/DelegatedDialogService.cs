using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssApp.Application.Services.Dialogs;

public class DelegatedDialogService : IDialogService
{
    public async Task<bool> RequestConfirmationAsync(string title, string message)
    {
        ConfirmationRequestedEventArgs args = new (title, message, "Yes", "No");
        ConfirmationRequested?.Invoke(this, args);
        return await args.ConfirmationSource.Task;
    }

    public Task ShowErrorAsync(string title, string message)
    {
        ShowErrorRequested?.Invoke(this, new ShowErrorRequestedEventArgs(title, message));
        return Task.CompletedTask;
    }

    public event EventHandler<ConfirmationRequestedEventArgs>? ConfirmationRequested;
    public event EventHandler<ShowErrorRequestedEventArgs>? ShowErrorRequested;
}
