using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Windows.Input;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Tests.Mock;
using RssApp.Application.ViewModels.FeedSubscription;

namespace RssApp.Application.Tests.ViewModels.FeedSubscription;

[TestClass]
public class FeedSubscriptionListViewModelTests
{
    [TestMethod]
    public async Task List_ShouldContainModel_WhenCreatedProgrammatically()
    {
        IServiceProvider services = new MockServiceProvider();

        const string testFeedName = "Test feed";
        const string testFeedUri = "https://example.com/feed.xml";
        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            await ctx.FeedSubscriptions.AddAsync(new Core.FeedSubscription { FeedName = testFeedName, FeedUri = testFeedUri });
            await ctx.SaveChangesAsync();
        }

        var viewModel = new FeedSubscriptionListViewModel(services);
        await viewModel.InitAsync();
        Assert.IsTrue(viewModel.Feeds.Any(x => x is { FeedName: testFeedName, FeedUri: testFeedUri }));
    }

    [TestMethod]
    public async Task List_ShouldContainModel_WhenCreatedByViewModel()
    {
        IServiceProvider services = new MockServiceProvider();
        const string testFeedName = "Test feed";
        const string testFeedUri = "https://example.com/feed.xml";

        FeedSubscriptionCreateViewModel createViewModel = new(services) { FeedName = testFeedName, FeedUri = testFeedUri };
        createViewModel.SaveCommand.Execute();

        var listViewModel = new FeedSubscriptionListViewModel(services);
        await listViewModel.InitAsync();
        Assert.IsTrue(listViewModel.Feeds.Any(x => x is { FeedName: testFeedName, FeedUri: testFeedUri }));
    }
}
