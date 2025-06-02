using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Services.Dialogs;

namespace RssApp.Application.ViewModels.FeedSubscription;

public partial class FeedSubscriptionDeleteViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    private Guid _feedId;

    public ICommand DeleteCommand { get; }

    public FeedSubscriptionDeleteViewModel(IServiceProvider services)
    {
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();
        _dialogService = services.GetRequiredService<IDialogService>();

        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public void Init(FeedSubscriptionModel subscriptionModel)
    {
        _feedId = subscriptionModel.ToPoco().FeedSubscriptionId;
    }

    private async Task Delete()
    {
        if (!await _dialogService.RequestConfirmationAsync("Delete filter?", "Are you sure you want to delete this filter?"))
        {
            return;
        }

        await using var ctx = _dbContextProvider.New();
        var staged = await ctx.FeedSubscriptions.SingleOrDefaultAsync(x => x.FeedSubscriptionId == _feedId);
        if (staged is null)
        {
            return;
        }

        ctx.FeedSubscriptions.Remove(staged);
        await ctx.SaveChangesAsync();
    }
}
