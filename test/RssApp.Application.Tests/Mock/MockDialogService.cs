using RssApp.Application.Services.Dialogs;

namespace RssApp.Application.Tests.Mock;

internal class MockDialogService : IDialogService
{
    public Task ShowErrorAsync(string title, string message)
    {
        ShowErrorRequested?.Invoke(this, new ShowErrorRequestedEventArgs(title, message));
        return Task.CompletedTask;
    }

    public event EventHandler<ConfirmationRequestedEventArgs>? ConfirmationRequested;
    public event EventHandler<ShowErrorRequestedEventArgs>? ShowErrorRequested;

    public async Task<bool> RequestConfirmationAsync(string title, string message)
    {
        var args = new ConfirmationRequestedEventArgs(title, message, "Confirm", "Cancel");
        ConfirmationRequested?.Invoke(this, args);
        return await args.ConfirmationSource.Task;
    }
}
