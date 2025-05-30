using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Collections.ObjectModel;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Tests.Mock;
using RssApp.Application.ViewModels.FeedFilter;
using RssApp.Core.Enums.Flags;

namespace RssApp.Application.Tests.ViewModels.FeedFilter;

[TestClass]
public class FeedFilterCreateViewModelTests
{
    [TestMethod]
    public async Task FeedFilter_ShouldExist_WhenCreated()
    {
        IServiceProvider services = new MockServiceProvider();

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            await ctx.FeedSubscriptions.AddAsync(new Core.FeedSubscription
            {
                FeedSubscriptionId = Guid.NewGuid(),
                FeedName = "Feed 1",
                FeedUri = "https://example.com/feed1.rss"
            });

            await ctx.SaveChangesAsync();
        }

        var viewModel = new FeedFilterCreateViewModel(services);
        await viewModel.InitAsync();

        viewModel.SelectedFeedSubscriptions.Add(viewModel.AvailableFeedSubscriptions.First());
        viewModel.AppliesToTitle = true;
        viewModel.FilterRegex = "!(AI)";
        viewModel.SaveCommand.Execute(null);

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            bool filterExists = ctx.FeedFilters.Any(x => x.AppliesTo == FeedFilterAppliesToFlags.Title
                                                         && x.FilterRegex == viewModel.FilterRegex);
            Assert.IsTrue(filterExists);
        }
    }

    [TestMethod]
    public async Task Junctions_ShouldExist_WhenCreated()
    {
        IServiceProvider services = new MockServiceProvider();

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            await ctx.FeedSubscriptions.AddAsync(new Core.FeedSubscription
            {
                FeedSubscriptionId = Guid.NewGuid(),
                FeedName = "Feed 1",
                FeedUri = "https://example.com/feed1.rss"
            });

            await ctx.FeedSubscriptions.AddAsync(new Core.FeedSubscription
            {
                FeedSubscriptionId = Guid.NewGuid(),
                FeedName = "Feed 2",
                FeedUri = "https://example.com/feed2.rss"
            });

            await ctx.FeedSubscriptions.AddAsync(new Core.FeedSubscription
            {
                FeedSubscriptionId = Guid.NewGuid(),
                FeedName = "Feed 3",
                FeedUri = "https://example.com/feed3.rss"
            });

            await ctx.SaveChangesAsync();
        }

        var viewModel = new FeedFilterCreateViewModel(services);
        await viewModel.InitAsync();

        await viewModel.SelectedFeedSubscriptions.ReplaceRangeAsync(viewModel.AvailableFeedSubscriptions);
        viewModel.SaveCommand.Execute(null);

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            Assert.IsTrue(ctx.FeedSubscriptionFeedFilters.Count() == 3);
        }
    }
}
