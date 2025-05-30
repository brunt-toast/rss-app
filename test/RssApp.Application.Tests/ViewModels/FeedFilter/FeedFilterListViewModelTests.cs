using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Tests.Mock;
using RssApp.Application.ViewModels.FeedFilter;
using RssApp.Core.Enums.Flags;
using RssApp.Core.Junctions;

namespace RssApp.Application.Tests.ViewModels.FeedFilter;

[TestClass]
public class FeedFilterListViewModelTests
{
    [TestMethod]
    public async Task List_ShouldContainFilter_AfterCreated()
    {
        IServiceProvider services = new MockServiceProvider();

        Core.FeedSubscription feedSubscription = new()
        {
            FeedSubscriptionId = Guid.NewGuid(),
            FeedName = "Test feed",
            FeedUri = "https://example.com/test.rss"
        };

        Core.FeedFilter feedFilter = new()
        {
            FeedFilterId = Guid.NewGuid(),
            FilterRegex = "^(AI)",
            IsWhitelist = false,
            AppliesTo = FeedFilterAppliesToFlags.Title
        };

        FeedSubscriptionFeedFilter junction = new()
        {
            FeedSubscriptionId = feedSubscription.FeedSubscriptionId,
            FeedFilterId = feedFilter.FeedFilterId
        };

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            await ctx.FeedSubscriptions.AddAsync(feedSubscription);
            await ctx.FeedFilters.AddAsync(feedFilter);
            await ctx.FeedSubscriptionFeedFilters.AddAsync(junction);
            await ctx.SaveChangesAsync();
        }

        FeedFilterListViewModel viewModel = new(services);
        await viewModel.InitAsync();

        Assert.IsTrue(viewModel.FeedFilters.Any(x => x.FilterRegex == feedFilter.FilterRegex));
    }
}
