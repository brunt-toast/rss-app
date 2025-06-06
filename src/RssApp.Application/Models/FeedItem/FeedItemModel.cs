using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using RssApp.Lib.Rendering;

namespace RssApp.Application.Models.FeedItem;

public class FeedItemModel
{
    private readonly CodeHollow.FeedReader.FeedItem _feedItem;

    public string Title => _feedItem.Title;
    public string Author => _feedItem.Author;
    public DateTime? PublishingDate => _feedItem.PublishingDate;
    public ICollection<string> Categories => _feedItem.Categories;
    public string Link => _feedItem.Link;
    public string Content => _feedItem.Content;

    public ICommand OpenInBrowserCommand { get; }

    public FeedItemModel(CodeHollow.FeedReader.FeedItem feedItem)
    {
        _feedItem = feedItem;
        OpenInBrowserCommand = new AsyncRelayCommand(OpenFeedAsync);
    }

    private async Task OpenFeedAsync()
    {
        string html = await RazorRenderer.RenderFeedItemAsync(_feedItem);
        string filePath = Path.Join(Path.GetTempPath(), $"{Guid.NewGuid()}.html");
        await File.WriteAllTextAsync(filePath, html);
        Process.Start(new ProcessStartInfo("file://" + filePath.Replace('\\', '/')) { UseShellExecute = true });
    }
}
