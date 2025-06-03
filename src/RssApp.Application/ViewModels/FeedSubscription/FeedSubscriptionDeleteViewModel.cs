using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Messages.FeedSubscription;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Services.Dialogs;

namespace RssApp.Application.ViewModels.FeedSubscription;

public partial class FeedSubscriptionDeleteViewModel : ObservableObject
{
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    private Guid _feedId;

    public ICommand DeleteCommand { get; }

    public FeedSubscriptionDeleteViewModel(IServiceProvider services)
    {
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();

        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public void Init(FeedSubscriptionModel subscriptionModel)
    {
        _feedId = subscriptionModel.ToPoco().FeedSubscriptionId;
    }

    private async Task Delete()
    {
        await using var ctx = _dbContextProvider.New();
        var staged = await ctx.FeedSubscriptions.SingleOrDefaultAsync(x => x.FeedSubscriptionId == _feedId);
        if (staged is null)
        {
            return;
        }

        ctx.FeedSubscriptions.Remove(staged);
        await ctx.SaveChangesAsync();

        WeakReferenceMessenger.Default.Send(new FeedSubscriptionsChangedMessage());
    }
}
