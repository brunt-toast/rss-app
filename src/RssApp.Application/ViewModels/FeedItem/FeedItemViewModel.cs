using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RssApp.Application.Models.FeedItem;

namespace RssApp.Application.ViewModels.FeedItem;

public partial class FeedItemViewModel : ViewModelBase
{
    [ObservableProperty] public partial string LocalUri { get; private set; } = "https://example.com/";

    public ICommand OpenInBrowserCommand { get; }

    public FeedItemViewModel(IServiceProvider services) : base(services)
    {
        OpenInBrowserCommand = new RelayCommand(OpenInBrowser);
    }

    public async Task InitAsync(FeedItemModel feedItem)
    {
        string filePath = await feedItem.RenderToFileAsync();
        LocalUri = "file://" + filePath.Replace('\\', '/');
    }

    private void OpenInBrowser()
    {
        Process.Start(new ProcessStartInfo(LocalUri) { UseShellExecute = true });
    }
}
