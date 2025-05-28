using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Windows.Input;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Tests.Mock;
using RssApp.Application.ViewModels.FeedSubscription;

namespace RssApp.Application.Tests.ViewModels.FeedSubscription;

[TestClass]
public class FeedSubscriptionCreateViewModelTests
{
    [TestMethod]
    public void FeedSubscription_ShouldExist_WhenCreated()
    {
        var serviceBuilder = new ServiceCollection();
        serviceBuilder.AddSingleton<IFeedSubscriptionContextProvider, MockFeedSubscriptionContextProvider>();
        IServiceProvider services = serviceBuilder.BuildServiceProvider();
        const string testFeedName = "Test feed";
        const string testFeedUri = "https://example.com/feed.xml";
        FeedSubscriptionCreateViewModel viewModel = new(services) { FeedName = testFeedName, FeedUri = testFeedUri };

        viewModel.SaveCommand.Execute();

        using var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New();
        Assert.IsTrue(ctx.FeedSubscriptions.Any(x => x.FeedName == testFeedName && x.FeedUri == testFeedUri));
    }
}

