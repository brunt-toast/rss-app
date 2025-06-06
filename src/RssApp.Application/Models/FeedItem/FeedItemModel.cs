using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RssApp.Application.Messages.FeedItem;
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

    public ICommand SelectCommand { get; }

    public FeedItemModel(CodeHollow.FeedReader.FeedItem feedItem)
    {
        _feedItem = feedItem;
        SelectCommand = new RelayCommand(Select);
    }

    private void Select()
    {
        WeakReferenceMessenger.Default.Send(new FeedItemSelectedMessage(this));
    }

    public async Task<string> RenderToFileAsync(string? filePath = null)
    {
        filePath ??= Path.Join(Path.GetTempPath(), $"{Guid.NewGuid()}.html");
        string html = await RazorRenderer.RenderFeedItemAsync(_feedItem);
        await File.WriteAllTextAsync(filePath, html);
        return filePath;
    }
}
