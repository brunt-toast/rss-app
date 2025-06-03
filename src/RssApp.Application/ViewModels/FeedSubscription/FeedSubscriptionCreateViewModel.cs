using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Messages.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;

namespace RssApp.Application.ViewModels.FeedSubscription;

public partial class FeedSubscriptionCreateViewModel : ObservableObject
{
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    [ObservableProperty] public partial string FeedName { get; set; } = "";
    [ObservableProperty] public partial string FeedUri { get; set; } = "";

    public ICommand SaveCommand { get; }

    public FeedSubscriptionCreateViewModel(IServiceProvider services)
    {
        SaveCommand = new RelayCommand(Save);
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();
    }

    private async void Save()
    {
        var newPoco = new Core.FeedSubscription { FeedName = FeedName, FeedUri = FeedUri };
        await using var ctx = _dbContextProvider.New();
        ctx.FeedSubscriptions.Add(newPoco);
        await ctx.SaveChangesAsync();

        WeakReferenceMessenger.Default.Send(new FeedSubscriptionsChangedMessage());
    }
}
