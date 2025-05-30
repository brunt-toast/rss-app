using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Windows.Input;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Tests.Mock;
using RssApp.Application.ViewModels.FeedSubscription;

namespace RssApp.Application.Tests.ViewModels.FeedSubscription;

[TestClass]
public class FeedSubscriptionDeleteViewModelTests
{
    [TestMethod]
    public async Task Entity_ShouldNotExist_AfterDelete()
    {
        IServiceProvider services = new MockServiceProvider();

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

        var deleteViewModel = new FeedSubscriptionDeleteViewModel(services);
        deleteViewModel.Init(new FeedSubscriptionModel(feedSubscription));
        deleteViewModel.DeleteCommand.Execute();

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            bool exists = await ctx.FeedSubscriptions.AnyAsync(x => x.FeedSubscriptionId == feedSubscription.FeedSubscriptionId);
            Assert.IsFalse(exists);
        }
    }
}
