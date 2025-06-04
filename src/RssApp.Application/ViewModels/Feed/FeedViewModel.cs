using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CodeHollow.FeedReader;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Collections.ObjectModel;
using RssApp.Application.Models.FeedFilter;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Services.Dialogs;
using RssApp.Core.Enums.Flags;

namespace RssApp.Application.ViewModels.Feed;

public partial class FeedViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    public FeedViewModel(IServiceProvider services)
    {
        _dialogService = services.GetRequiredService<IDialogService>();
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();
    }

    public ObservableCollection<FeedItem> Items { get; } = [];

    public async Task InitAsync(FeedSubscriptionModel model)
    {
        CodeHollow.FeedReader.Feed feed;

        int retries = 0;
        const int maxRetries = 10;
        while (true)
        {
            if (retries >= maxRetries)
            {
                await _dialogService.ShowErrorAsync("Could not get feed", $"Failed to get the feed after {maxRetries} retries.");
                return;
            }

            try
            {
                feed = await FeedReader.ReadAsync(model.FeedUri);
                break;
            }
            catch (Exception)
            {
                retries++;
            }
        }

        await Items.ReplaceRangeAsync(feed.Items);
        try
        {
            await FilterItems(model);
        }
        catch (Exception ex)
        {

        }
    }

    private async Task FilterItems(FeedSubscriptionModel model)
    {
        await using var ctx = _dbContextProvider.New();

        var feedSubscriptionId = model.ToPoco().FeedSubscriptionId;

        var allFilters = await ctx.FeedFilters.ToListAsync();
        var relevantJunctions = await ctx.FeedSubscriptionFeedFilters.Where(x => x.FeedSubscriptionId == feedSubscriptionId).ToListAsync();
        var relevantFilters = allFilters
            .Where(x => relevantJunctions.Any(y => y.FeedFilterId == x.FeedFilterId))
            .Select(x => new FeedFilterModel(x))
            .ToList();

        foreach (FeedItem item in Items)
        {
            if (ShouldBeDiscarded(item, relevantFilters))
            {
                Items.Remove(item);
            }
        }
    }

    private bool ShouldBeDiscarded(FeedItem item, IEnumerable<FeedFilterModel> filters)
    {
        return filters.Any(x => ShouldBeDiscarded(item, x));
    }

    private bool ShouldBeDiscarded(FeedItem item, FeedFilterModel filter)
    {
        Regex pattern = new(filter.FilterRegex);

        if ((filter.AppliesTo & FeedFilterAppliesToFlags.Title) != 0)
        {
            bool match = pattern.IsMatch(item.Title);
            if (match || (!match && filter.IsWhitelist)) return true;
        }

        if ((filter.AppliesTo & FeedFilterAppliesToFlags.Uri) != 0)
        {
            bool match = pattern.IsMatch(item.Link);
            if (match || (!match && filter.IsWhitelist)) return true;
        }

        if ((filter.AppliesTo & FeedFilterAppliesToFlags.Body) != 0)
        {
            bool match = pattern.IsMatch(item.Content);
            if (match || (!match && filter.IsWhitelist)) return true;
        }

        return false;
    }
}
