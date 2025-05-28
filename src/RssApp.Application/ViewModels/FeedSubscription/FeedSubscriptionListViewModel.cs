using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Collections.ObjectModel;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;

namespace RssApp.Application.ViewModels.FeedSubscription;

public partial class FeedSubscriptionListViewModel : ObservableObject
{
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    public ObservableCollection<FeedSubscriptionModel> Feeds { get; } = [];

    public FeedSubscriptionListViewModel(IServiceProvider services)
    {
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();
    }

    public async Task InitAsync()
    {
        await using var ctx = _dbContextProvider.New();

        IEnumerable<Core.FeedSubscription> feedSubscriptions = ctx.FeedSubscriptions;
        IEnumerable<FeedSubscriptionModel> feedSubscriptionModels = feedSubscriptions.Select(x => new FeedSubscriptionModel(x));

        await Feeds.ReplaceRangeAsync(feedSubscriptionModels);
    }
}
