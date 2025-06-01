using System.Collections.ObjectModel;
using CodeHollow.FeedReader;
using CommunityToolkit.Mvvm.ComponentModel;
using RssApp.Application.Extensions.System.Collections.ObjectModel;
using RssApp.Application.Models.FeedSubscription;

namespace RssApp.Application.ViewModels.Feed;

public partial class FeedViewModel : ObservableObject
{
    ObservableCollection<FeedItem> Items { get; } = [];

    public async Task InitAsync(FeedSubscriptionModel model)
    {
        var feed = await FeedReader.ReadAsync(model.FeedUri);
        await Items.ReplaceRangeAsync(feed.Items);
    }
}
