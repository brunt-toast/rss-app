using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Windows.Input;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Tests.Mock;
using RssApp.Application.ViewModels.FeedSubscription;

namespace RssApp.Application.Tests.ViewModels.FeedSubscription;

[TestClass]
public class FeedSubscriptionEditViewModelTests
{
    [TestMethod]
    public async Task Entity_ShouldBeChanged_AfterEdit()
    {
        var serviceBuilder = new ServiceCollection();
        serviceBuilder.AddSingleton<IFeedSubscriptionContextProvider, MockFeedSubscriptionContextProvider>();
        IServiceProvider services = serviceBuilder.BuildServiceProvider();

        const string testFeedName = "Test feed";
        const string testFeedUri = "https://example.com/feed.xml";
        var feedSubscription = new Core.FeedSubscription { FeedName = testFeedName, FeedUri = testFeedUri };
        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            await ctx.FeedSubscriptions.AddAsync(feedSubscription);
            await ctx.SaveChangesAsync();
        }

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            bool exists = await ctx.FeedSubscriptions.AnyAsync(x => x.FeedSubscriptionId == feedSubscription.FeedSubscriptionId);
            Assert.IsTrue(exists);
        }

        const string newFeedName = "Feed name has changed";
        var viewModel = new FeedSubscriptionEditViewModel(services);
        await viewModel.InitAsync(new FeedSubscriptionModel(feedSubscription));
        viewModel.FeedName = newFeedName;
        viewModel.SaveCommand.Execute();

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            var record = await ctx.FeedSubscriptions.SingleOrDefaultAsync(x => x.FeedSubscriptionId == feedSubscription.FeedSubscriptionId);
            ArgumentNullException.ThrowIfNull(record);
            Assert.IsTrue(record.FeedName == newFeedName);
        }
    }
}
