using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Collections.ObjectModel;
using RssApp.Application.Messages.FeedFilter;
using RssApp.Application.Models.FeedFilter;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;

namespace RssApp.Application.ViewModels.FeedFilter;

public partial class FeedFilterListViewModel : ObservableObject
{
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    public ObservableCollection<FeedFilterModel> FeedFilters { get; } = [];

    public FeedFilterListViewModel(IServiceProvider services)
    {
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();
        WeakReferenceMessenger.Default.Register<FeedFiltersChangedMessage>(this, OnFeedFiltersChangedMessage);
    }

    private async void OnFeedFiltersChangedMessage(object recipient, FeedFiltersChangedMessage message)
    {
        await RefreshFilters();
    }

    public async Task InitAsync()
    {
        await RefreshFilters();
    }

    private async Task RefreshFilters()
    {
        await using var ctx = _dbContextProvider.New();
        var models = ctx.FeedFilters.Select(x => new FeedFilterModel(x));
        await FeedFilters.ReplaceRangeAsync(models);
    }
}
