using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Collections.ObjectModel;
using RssApp.Application.Messages.FeedSubscription;
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
        WeakReferenceMessenger.Default.Register<FeedSubscriptionsChangedMessage>(this, OnFeedSubscriptionsChangedMessage);
    }

    private async void OnFeedSubscriptionsChangedMessage(object recipient, FeedSubscriptionsChangedMessage message)
    {
        await RefreshFeedsAsync();
    }

    public async Task InitAsync()
    {
        await RefreshFeedsAsync();
    }

    private async Task RefreshFeedsAsync()
    {
        await using var ctx = _dbContextProvider.New();

        IEnumerable<Core.FeedSubscription> feedSubscriptions = ctx.FeedSubscriptions;
        IEnumerable<FeedSubscriptionModel> feedSubscriptionModels = feedSubscriptions.Select(x => new FeedSubscriptionModel(x));

        await Feeds.ReplaceRangeAsync(feedSubscriptionModels);
    }
}
