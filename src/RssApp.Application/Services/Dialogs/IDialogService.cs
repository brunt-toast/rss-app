namespace RssApp.Application.Services.Dialogs;

public interface IDialogService
{
    public Task<bool> RequestConfirmationAsync(string title, string message);
    public event EventHandler<ConfirmationRequestedEventArgs>? ConfirmationRequested;
}

public class ConfirmationRequestedEventArgs : EventArgs
{
    public TaskCompletionSource<bool> ConfirmationSource { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    public string DialogTitle { get; }
    public string DialogContent { get; }
    public string ConfirmationText { get; }
    public string DeclineText { get; }

    public ConfirmationRequestedEventArgs(string dialogTitle, string dialogContent, string confirmationText, string declineText)
    {
        DialogTitle = dialogTitle;
        DialogContent = dialogContent;
        ConfirmationText = confirmationText;
        DeclineText = declineText;
    }

    public void SetConfirmed(bool result)
    {
        ConfirmationSource.TrySetResult(result);
    }
}
