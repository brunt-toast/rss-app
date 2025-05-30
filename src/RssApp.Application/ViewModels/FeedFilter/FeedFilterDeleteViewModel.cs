using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Models.FeedFilter;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Core.Junctions;

namespace RssApp.Application.ViewModels.FeedFilter;

public partial class FeedFilterDeleteViewModel : ObservableObject
{
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;
    private Guid _feedFilterId;

    public ICommand DeleteCommand { get; }

    public FeedFilterDeleteViewModel(IServiceProvider services)
    {
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();

        DeleteCommand = new RelayCommand(Delete);
    }

    public void Init(FeedFilterModel feedFilterModel)
    {
        _feedFilterId = feedFilterModel.ToPoco().FeedFilterId;
    }

    private async void Delete()
    {
        await using var ctx = _dbContextProvider.New();

        Core.FeedFilter? stagedFilter = await ctx.FeedFilters.SingleOrDefaultAsync(x => x.FeedFilterId == _feedFilterId);
        IEnumerable<FeedSubscriptionFeedFilter> stagedJunctions = ctx.FeedSubscriptionFeedFilters.Where(x => x.FeedFilterId == _feedFilterId);

        if (stagedFilter is not null)
        {
            ctx.FeedFilters.Remove(stagedFilter);
        }

        ctx.FeedSubscriptionFeedFilters.RemoveRange(stagedJunctions);

        await ctx.SaveChangesAsync();
    }
}
