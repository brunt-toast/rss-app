using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Messages.FeedSubscription;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;

namespace RssApp.Application.ViewModels.FeedSubscription;

public partial class FeedSubscriptionEditViewModel : ObservableObject
{
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    private Guid _feedSubscriptionId;

    public ICommand SaveCommand { get; }

    [ObservableProperty] public partial string FeedName { get; set; } = "";
    [ObservableProperty] public partial string FeedUri { get; set; } = "";

    public FeedSubscriptionEditViewModel(IServiceProvider services)
    {
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();
        SaveCommand = new RelayCommand(Save);
    }

    public async Task InitAsync(FeedSubscriptionModel feedSubscriptionModel)
    {
        _feedSubscriptionId = feedSubscriptionModel.ToPoco().FeedSubscriptionId;

        await using var ctx = _dbContextProvider.New();
        var record = await ctx.FeedSubscriptions.SingleOrDefaultAsync(x => x.FeedSubscriptionId == _feedSubscriptionId);

        if (record is null)
        {
            throw new Exception($"Could not find a {nameof(Core.FeedSubscription)} record " +
                                $"with {nameof(Core.FeedSubscription.FeedSubscriptionId)} {_feedSubscriptionId}.");
        }

        FeedName = record.FeedName;
        FeedUri = record.FeedUri;
    }

    private async void Save()
    {
        await using var ctx = _dbContextProvider.New();
        var record = await ctx.FeedSubscriptions.SingleOrDefaultAsync(x => x.FeedSubscriptionId == _feedSubscriptionId);

        if (record is null)
        {
            throw new Exception($"Could not find a {nameof(Core.FeedSubscription)} record " +
                                $"with {nameof(Core.FeedSubscription.FeedSubscriptionId)} {_feedSubscriptionId}.");
        }

        record.FeedName = FeedName;
        record.FeedUri = FeedUri;

        await ctx.SaveChangesAsync();
        WeakReferenceMessenger.Default.Send(new FeedSubscriptionsChangedMessage());
    }
}
