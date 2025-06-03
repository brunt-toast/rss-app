using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Models.FeedFilter;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;

namespace RssApp.Application.ViewModels.FeedFilter;

public partial class FeedFilterEditViewModel : ObservableObject
{
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    public FeedFilterModel Model { get; private set; } = null!;

    public ICommand SaveCommand { get; }

    public FeedFilterEditViewModel(IServiceProvider services)
    {
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();
        SaveCommand = new RelayCommand(Save);
    }

    public async Task InitAsync(FeedFilterModel feedFilterModel)
    {
        Model = feedFilterModel;

        await using var ctx = _dbContextProvider.New();
        var record = await ctx.FeedFilters.SingleOrDefaultAsync(x => x.FeedFilterId == Model.ToPoco().FeedFilterId);

        if (record is null)
        {
            throw new Exception($"Could not find a {nameof(Core.FeedFilter)} record " +
                                $"with {nameof(Core.FeedFilter.FeedFilterId)} {Model.ToPoco().FeedFilterId}.");
        }
    }

    private async void Save()
    {
        await using var ctx = _dbContextProvider.New();
        var record = await ctx.FeedFilters.SingleOrDefaultAsync(x => x.FeedFilterId == Model.ToPoco().FeedFilterId);
        ArgumentNullException.ThrowIfNull(record);

        record.AppliesTo = Model.AppliesTo;
        record.FilterRegex = Model.FilterRegex;
        record.IsWhitelist = Model.IsWhitelist;

        await ctx.SaveChangesAsync();
    }
}
